using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.StockIn
 *   文件名:     StockInMainViewModel
 *   说明:       
 *   创建时间:   2014/2/11 17:10:43
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class StockInMainViewModel : ViewModelBase
    {
        private FormworkRule _formworkRule = null;
        private StockLotRule _stockLotRule = null;
        private RoleRule _rule = null;
        public StockInMainViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _rule = new RoleRule();
            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        IsCheck = (bool)param.Entity;
                        _stockLotRule.QtyCount = 0;
                        CurrentStockLot = new StockLot();
                        LotNoEnabled = true;

                        StockInHOLD = false;
                        DuMoHOLD = false;
                        PaoGuangHOLD = false;
                        JianBaoHOLD = false;


                        DuMoImgHOLD = false;
                        PaoGuangImgHOLD = false;
                        JianBaoImgHOLD = false;

                        SumQty = 0;// CurrentStockLot.StockDetails.Count();
                        SurQty = 0;
                        TXTLOTNOISEnabled = true;
                        HOLDQty = "    HOLD数：0 PCS    ";
                        ReadComTest();
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            HOLDVisibility = Visibility.Collapsed;
                            EndStockLotVisibility = Visibility.Collapsed;
                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_rule.str扫描入库 + _rule.str接触HOLD))
                                HOLDVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_rule.str扫描入库 + _rule.str入库结束))
                                EndStockLotVisibility = Visibility.Visible;
                        }
                        else
                        {
                            HOLDVisibility = Visibility.Visible;
                            EndStockLotVisibility = Visibility.Visible;
                        }
                        if (!string.IsNullOrWhiteSpace(param.Tag))
                        {
                            CurrentStockLot.LotNo = param.Tag;
                            _stockLotRule.GetStockLotEntityByLotNo_ExAsyns(CurrentStockLot.LotNo, 1, IsCheckAll);
                        }
                        break;
                    case CmdName.SaveGlassID:
                        StockDetail entity = param.Entity as StockDetail;
                        AddStockDetailAsyns(entity);
                        break;
                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "StockInMainViewModel");

            _formworkRule = new FormworkRule();
            _formworkRule.GetFormworkListCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        FormWorkCollection = new ObservableCollection<string>(e.Results.Select(p => p.ProductModel));
                    }
                };
            _stockLotRule = new StockLotRule();
            _stockLotRule.GetStockLotEntityByLotNo_ExCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    TXTLOTNOISEnabled = false;
                    LineNumber = 1;
                    CurrentStockLot = e.Results as StockLot;
                    ControlsEnabled = (CurrentStockLot == null);
                    LotNoEnabled = ControlsEnabled;
                    if (!ControlsEnabled)
                    {
                        HOLDQty = "    HOLD数：" + _stockLotRule.HOLDQty_Ex + " PCS    ";
                        if (CurrentStockLot.StockDetails != null)//如果它的Glass明细不为空
                        {
                          //  if (_stockLotRule.QtyCount <= 0)  2014年5月6日 12:56:26  无意义
                          //  {
                                SumQty = _stockLotRule.IStockInQty;// CurrentStockLot.StockDetails.Count();
                                SurQty = CurrentStockLot.PCSQty - SumQty;
                           // }
                            CurrentStockLot.StockDetails = CurrentStockLot.StockDetails.OrderByDescending(p => p.StockInDT).ToArray();
                        }
                        if (CurrentStockLot.DetailInfoHOLD != null)
                        {
                            StockInHOLD = CurrentStockLot.DetailInfoHOLD.Contains("入库");
                            DuMoHOLD = CurrentStockLot.DetailInfoHOLD.Contains("镀膜");
                            PaoGuangHOLD = CurrentStockLot.DetailInfoHOLD.Contains("抛光");
                            JianBaoHOLD = CurrentStockLot.DetailInfoHOLD.Contains("减薄");
                        }
                        if (CurrentStockLot.ImageHOLD != null)
                        {
                            DuMoImgHOLD = CurrentStockLot.ImageHOLD.Contains("镀膜");
                            PaoGuangImgHOLD = CurrentStockLot.ImageHOLD.Contains("抛光");
                            JianBaoImgHOLD = CurrentStockLot.ImageHOLD.Contains("减薄");
                        }
                        CurrentFormwork = CurrentStockLot.ProModel;
                        if (CurrentStockLot.Status > 0 && CurrentStockLot.Status < 8)
                        {
                            Common.MessageBox.Show("该LotNo已结束");
                            return;
                        }

                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.Enter,
                            Target = "StockInMainView",
                        });
                    }
                    else
                    {
                        // _formworkRule.GetFormWorkListAsyns();
                    }
                }
            };

            _stockLotRule.AddStockDetailCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                        GlassID = string.Empty;
                    }
                    else
                    {
                        GlassID = string.Empty;
                        SumQty = _stockLotRule.QtyCount;
                        SurQty = CurrentStockLot.PCSQty - SumQty;

                        _stockLotRule.GetStockLotEntityByLotNo_ExAsyns(CurrentStockLot.LotNo, 1, IsCheckAll);
                    }
                    isDispose = false;
                };
            _stockLotRule.GetStockDetailListCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        //ObservableCollection<StockDetail> tmppdetails = new ObservableCollection<StockDetail>(e.Results);
                        CurrentStockLot.StockDetails = e.Results.OrderByDescending(p => p.CreateDt).ToArray();
                        LineNumber = 1;
                    }
                };
            _stockLotRule.AddStockLotCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        CurrentStockLot.ID = _stockLotRule.StockID;
                        SumQty = 0;
                        SurQty = CurrentStockLot.PCSQty - SumQty;
                        ControlsEnabled = false;
                    }
                };
            _stockLotRule.ModifyStockDetailListCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                        _stockLotRule.GetStockLotEntityByLotNo_ExAsyns(CurrentStockLot.LotNo, 1, IsCheckAll);
                };
            ScanComExecute = (s, e) =>
               {
                   string ErrMsg = string.Empty;
                   if (!_stockLotRule.CheckStockDetail_In(s.ToString(), CurrentStockLot.ID, IsCheck, ref ErrMsg))
                   {
                       //if (ErrMsg.Equals("特殊处理:退货"))
                       //{
                       //    if (!(Common.MessageBox.Show("该ID曾经退货，确定再次入库吗？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes))
                       //    {
                       //        isDispose = false;
                       //        return;
                       //    }
                       //}
                       //else
                       //{

                       Common.MessageBox.Show(ErrMsg);
                       isDispose = false;
                       return;
                       //  }
                   }
                   GlassID = s.ToString();
                   //   AddStockDetailAsyns();
                   _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                   {
                       cmdViewName = CmdViewName.ToolScanGlassID_StockInView,
                       cmdName = CmdName.New,
                       Entity = CurrentStockLot,
                       Target = "Sell",
                   });
               };
        }

        #region 界面绑定字段

        private string _holdQty;
        /// <summary>
        ///   
        /// </summary>
        public string HOLDQty
        {
            get { return _holdQty; }
            set
            {
                _holdQty = value;
                this.RaisePropertyChanged("HOLDQty");
            }
        }
        public bool _tXTLOTNOISEnabled;
        public bool TXTLOTNOISEnabled
        {
            get { return _tXTLOTNOISEnabled; }
            set
            {
                _tXTLOTNOISEnabled = value;
                this.RaisePropertyChanged("TXTLOTNOISEnabled");
            }
        }


        public bool _isCheckAll;
        public bool IsCheckAll
        {
            get { return _isCheckAll; }
            set
            {
                _isCheckAll = value;
                this.RaisePropertyChanged("IsCheckAll");
            }
        }
        private ObservableCollection<string> _formworkCollection;
        public ObservableCollection<string> FormWorkCollection
        {
            get { return _formworkCollection; }
            set
            {
                _formworkCollection = value;
                this.RaisePropertyChanged("FormWorkCollection");
            }
        }
        private Visibility _hOLDVisibility;
        public Visibility HOLDVisibility
        {
            get { return _hOLDVisibility; }
            set
            {
                _hOLDVisibility = value;
                this.RaisePropertyChanged("HOLDVisibility");
            }
        }
        private Visibility _endStockLotVisibility;
        public Visibility EndStockLotVisibility
        {
            get { return _endStockLotVisibility; }
            set
            {
                _endStockLotVisibility = value;
                this.RaisePropertyChanged("EndStockLotVisibility");
            }
        }
        private string _currentFormwork;
        public string CurrentFormwork
        {
            get { return _currentFormwork; }
            set
            {
                _currentFormwork = value;
                this.RaisePropertyChanged("CurrentFormwork");
            }
        }

        private string _lotNo;
        public string LotNo
        {
            get { return _lotNo; }
            set
            {
                _lotNo = value;
                this.RaisePropertyChanged("LotNo");
            }
        }
        private bool _isJianBao;
        public bool IsJianBao
        {
            get { return _isJianBao; }
            set
            {
                _isJianBao = value;
                this.RaisePropertyChanged("IsJianBao");
            }
        }
        private bool _isDuMo;
        public bool IsDuMo
        {
            get { return _isDuMo; }
            set
            {
                _isDuMo = value;
                this.RaisePropertyChanged("IsDuMo");
            }
        }
        private string _glassID;
        /// <summary>
        /// 扫描到的GlassID
        /// </summary>
        public string GlassID
        {
            get { return _glassID; }
            set
            {
                _glassID = value;
                this.RaisePropertyChanged("GlassID");
            }
        }

        private string _pCSQty;
        public string PCSQty
        {
            get { return _pCSQty; }
            set
            {
                _pCSQty = value;
                this.RaisePropertyChanged("PCSQty");
            }
        }

        private int _surQty;
        /// <summary>
        /// 剩余未扫描数量
        /// </summary>
        public int SurQty
        {
            get { return _surQty; }
            set
            {
                _surQty = value;
                this.RaisePropertyChanged("SurQty");
            }
        }
        private int _sumQty;
        public int SumQty
        {
            get { return _sumQty; }
            set
            {
                _sumQty = value;
                this.RaisePropertyChanged("SumQty");
            }
        }
        private int _lineNumber = 1;
        public int LineNumber
        {
            get { return _lineNumber++; }
            set
            {
                _lineNumber = value;
                this.RaisePropertyChanged("LineNumber");
            }
        }
        private StockLot _currentStockLot;

        public StockLot CurrentStockLot
        {
            get { return _currentStockLot; }
            set
            {
                _currentStockLot = value;
                _lineNumber = 1;
                this.RaisePropertyChanged("CurrentStockLot");
            }
        }
        private StockDetail _currentDetail;

        public StockDetail CurrentDetail
        {
            get { return _currentDetail; }
            set
            {
                _currentDetail = value;
                this.RaisePropertyChanged("CurrentDetail");
            }
        }

        private bool _controlsEnabled;
        public bool ControlsEnabled
        {
            get { return _controlsEnabled; }
            set
            {
                _controlsEnabled = value;
                this.RaisePropertyChanged("ControlsEnabled");
            }
        }
        private bool _lotNoEnabled;
        public bool LotNoEnabled
        {
            get { return _lotNoEnabled; }
            set
            {
                _lotNoEnabled = value;
                this.RaisePropertyChanged("LotNoEnabled");
            }
        }
        #endregion

        #region 品质信息绑定字段
        private bool _stockInHOLD;
        /// <summary>
        /// 入库
        /// </summary>
        public bool StockInHOLD
        {
            get { return _stockInHOLD; }
            set
            {
                _stockInHOLD = value;
                this.RaisePropertyChanged("StockInHOLD");
            }
        }
        private bool _jianbaoHOLD;
        public bool JianBaoHOLD
        {
            get { return _jianbaoHOLD; }
            set
            {
                _jianbaoHOLD = value;
                this.RaisePropertyChanged("JianBaoHOLD");
            }
        }
        private bool _paoGuangHOLD;
        public bool PaoGuangHOLD
        {
            get { return _paoGuangHOLD; }
            set
            {
                _paoGuangHOLD = value;
                this.RaisePropertyChanged("PaoGuangHOLD");
            }
        }
        private bool _duMoHOLD;
        public bool DuMoHOLD
        {
            get { return _duMoHOLD; }
            set
            {
                _duMoHOLD = value;
                this.RaisePropertyChanged("DuMoHOLD");
            }
        }

        private bool _jianbaoImgHOLD;
        public bool JianBaoImgHOLD
        {
            get { return _jianbaoImgHOLD; }
            set
            {
                _jianbaoImgHOLD = value;
                this.RaisePropertyChanged("JianBaoImgHOLD");
            }
        }
        private bool _paoGuangImgHOLD;
        public bool PaoGuangImgHOLD
        {
            get { return _paoGuangImgHOLD; }
            set
            {
                _paoGuangImgHOLD = value;
                this.RaisePropertyChanged("PaoGuangImgHOLD");
            }
        }
        private bool _duMoImgHOLD;
        public bool DuMoImgHOLD
        {
            get { return _duMoImgHOLD; }
            set
            {
                _duMoImgHOLD = value;
                this.RaisePropertyChanged("DuMoImgHOLD");
            }
        }
        #endregion

        #region 绑定事件
        private DelegateCommand _cmdPageLoad;
        public DelegateCommand CmdPageLoad
        {
            get
            {
                if (_cmdPageLoad == null)
                {
                    _cmdPageLoad = new DelegateCommand(() =>
                    {
                        // GetAccountsAsyns();
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        _formworkRule.GetFormWorkListAsyns(queryList);
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Target = "StockInMainView",
                        });
                    });
                }
                return _cmdPageLoad;
            }
        }
        private DelegateCommand<RoutedEventArgs> _cmdLotOperater;
        /// <summary>
        /// LotNo文本框回车绑定事件
        /// </summary>
        public DelegateCommand<RoutedEventArgs> CmdLotOperater
        {
            get
            {
                if (_cmdLotOperater == null)
                {
                    _cmdLotOperater = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        var key = e as KeyEventArgs;
                        if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        {
                            if (string.IsNullOrWhiteSpace(CurrentStockLot.LotNo))
                            {
                                Common.MessageBox.Show("请填写LOT NO");
                                return;
                            }
                            if (Common.MessageBox.Show("确定输入该LOTNO?", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                            {
                                _stockLotRule.GetStockLotEntityByLotNo_ExAsyns(CurrentStockLot.LotNo, 1, IsCheckAll);
                            }
                        }
                    });
                }
                return _cmdLotOperater;
            }
        }
        private DelegateCommand _cmdSorting;
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand  CmdSorting
        {
            get
            {
                if (_cmdSorting == null)
                {
                    _cmdSorting = new DelegateCommand(() =>
                    {
                        _lineNumber = 1;
                    });
                }
                return _cmdSorting;
            }
        }
        private DelegateCommand _cmdShowAll;
        public DelegateCommand CmdShowAll
        {
            get
            {
                if (_cmdShowAll == null)
                {

                    _cmdShowAll = new DelegateCommand(() =>
                    {
                        if (string.IsNullOrWhiteSpace(CurrentStockLot.LotNo))
                        {
                            Common.MessageBox.Show("请填写LOT NO");
                            return;
                        }
                        _stockLotRule.GetStockLotEntityByLotNo_ExAsyns(CurrentStockLot.LotNo, 1, IsCheckAll);
                    });
                }
                return _cmdShowAll;
            }
        }
        private DelegateCommand<RoutedEventArgs> _cmdDetailOperater;
        /// <summary>
        /// LotNo文本框回车绑定事件
        /// </summary>
        public DelegateCommand<RoutedEventArgs> CmdDetailOperater
        {
            get
            {
                if (_cmdDetailOperater == null)
                {
                    _cmdDetailOperater = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        var key = e as KeyEventArgs;
                        if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        {
                            if (!string.IsNullOrWhiteSpace(GlassID))
                            {
                                if (CurrentStockLot == null)
                                {
                                    Common.MessageBox.Show("请先输入LOTNO，确认型号及数量，再开始扫描");
                                    return;
                                }
                                string ErrMsg = string.Empty;
                                if (!_stockLotRule.CheckStockDetail_In(GlassID, CurrentStockLot.ID, IsCheck, ref ErrMsg))
                                {
                                    Common.MessageBox.Show(ErrMsg);
                                    return;
                                }


                                //  AddStockDetailAsyns();
                                _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                                {
                                    cmdViewName = CmdViewName.ToolScanGlassID_StockInView,
                                    Entity = CurrentStockLot,
                                    cmdName = CmdName.New,
                                    Target = "Sell",
                                });

                            }
                        }
                    });
                }
                return _cmdDetailOperater;
            }
        }
        private DelegateCommand<RoutedEventArgs> _cmdProDicOperater;
        public DelegateCommand<RoutedEventArgs> CmdProDicOperater
        {
            get
            {
                if (_cmdProDicOperater == null)
                {
                    _cmdProDicOperater = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        var key = e as KeyEventArgs;
                        if (key == null || key.Key == Key.Enter)//当key为空，或按键为回车键时
                        {
                            if (string.IsNullOrWhiteSpace(LotNo))
                            {
                                Common.MessageBox.Show("请填写LotNo号");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(CurrentFormwork))
                            {
                                Common.MessageBox.Show("请选择型号");
                                return;
                            }
                            int iQty = 0;
                            if (!int.TryParse(PCSQty, out iQty))
                            {
                                Common.MessageBox.Show("数量输入不正确");
                                return;
                            }
                            if ((!IsJianBao) && (!IsDuMo))//如果减薄和镀膜都没有选择
                            {
                                Common.MessageBox.Show("减薄和镀膜工序必须选择一种或一种以上");

                            }
                            string strHOLD = string.Empty;
                            if (StockInHOLD)
                                strHOLD += "入库,";
                            if (JianBaoHOLD)
                                strHOLD += "减薄,";
                            if (PaoGuangHOLD)
                                strHOLD += "抛光,";
                            if (DuMoHOLD)
                                strHOLD += "镀膜,";

                            CurrentStockLot = new StockLot();
                            CurrentStockLot.LotNo = LotNo;
                            CurrentStockLot.ProModel = CurrentFormwork;
                            CurrentStockLot.PCSQty = iQty;
                            CurrentStockLot.IsDuMo = IsDuMo;
                            CurrentStockLot.IsJianBao = IsJianBao;
                            CurrentStockLot.DetailInfoHOLD = strHOLD;
                            _stockLotRule.AddStockLotAsyns(CurrentStockLot);
                        }
                    });
                }
                return _cmdProDicOperater;
            }
        }

        private DelegateCommand _cmdNewLotNo;
        public DelegateCommand CmdNewLotNo
        {
            get
            {
                if (_cmdNewLotNo == null)
                {
                    _cmdNewLotNo = new DelegateCommand(() =>
                    {
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定开始新的入库吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            LotNo = string.Empty;
                            CurrentFormwork = null;
                            PCSQty = string.Empty;
                            CurrentStockLot = null;
                            //    ControlsEnabled = true;
                            SumQty = 0;
                            SurQty = 0;
                            IsJianBao = true;
                            IsDuMo = true;
                            LotNoEnabled = true;
                            GlassID = string.Empty;
                        }
                    });
                }
                return _cmdNewLotNo;
            }
        }


        private DelegateCommand _cmdEndLotNo;
        public DelegateCommand CmdEndLotNo
        {
            get
            {
                if (_cmdEndLotNo == null)
                {

                    _cmdEndLotNo = new DelegateCommand(() =>
                    {

                        if (CurrentStockLot == null)
                        {
                            Common.MessageBox.Show("当前没有可结束的Lot No");
                            return;
                        }
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定结束该LotNo吗？ 结束之后将不能继续对该LotNo进行GlassID扫描", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            string ErrMsg = string.Empty;
                            if (!_stockLotRule.EndStockLot(CurrentStockLot.ID, ref ErrMsg))
                            {
                                Common.MessageBox.Show(ErrMsg);
                            }
                            else
                            {
                                Common.MessageBox.Show("结束成功");
                            }
                        }
                    });
                }
                return _cmdEndLotNo;
            }
        }

        private DelegateCommand _cmdExprotExcel;
        public DelegateCommand CmdExprotExcel
        {
            get
            {
                if (_cmdExprotExcel == null)
                {

                    _cmdExprotExcel = new DelegateCommand(() =>
                    {


                    });
                }
                return _cmdExprotExcel;
            }
        }
        //CmdModifyHOLD
        private DelegateCommand _cmdModifyHOLD;
        public DelegateCommand CmdModifyHOLD
        {
            get
            {
                if (_cmdModifyHOLD == null)
                {

                    _cmdModifyHOLD = new DelegateCommand(() =>
                    {
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定解除该GlassID的HOLD状态吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            CurrentDetail.IsHOLD = false;
                            _stockLotRule.ModifyStockDetailListAsyns(CurrentDetail);
                        }
                    });
                }
                return _cmdModifyHOLD;
            }
        }

        private DelegateCommand _cmdTuiHuo;
        public DelegateCommand CmdTuiHuo
        {
            get
            {
                if (_cmdTuiHuo == null)
                {

                    _cmdTuiHuo = new DelegateCommand(() =>
                    {
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "退货会彻底删除该GlassID，确定退货吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            //   CurrentDetail.Status = -1;
                            //  CurrentDetail.IsHOLD = false;
                            //  _stockLotRule.ModifyStockDetailListAsyns(CurrentDetail);

                            string ErrMsg = string.Empty;
                            if (_stockLotRule.DelStockDetailAndTuihuoCount(CurrentDetail.ID, ref ErrMsg))
                            {
                                Common.MessageBox.Show("退货成功");
                                _stockLotRule.GetStockLotEntityByLotNo_ExAsyns(CurrentStockLot.LotNo, 1, IsCheckAll);
                            }
                            else
                            {
                                Common.MessageBox.Show(ErrMsg);
                            }
                        }
                    });
                }
                return _cmdTuiHuo;
            }
        }
        #endregion
        private bool _isCheck;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                _isCheck = value;
                this.RaisePropertyChanged("IsCheck");
            }
        }
        #region 私有方法
        private void AddStockDetailAsyns(StockDetail entity)
        {


            //  StockDetail entity = new StockDetail();
            entity.GlassID = GlassID.ToUpper();
            entity.Qty = 1;
            entity.StockLotID = CurrentStockLot.ID;
            entity.Status = 1;
            entity.StockInDT = DateTime.Now;

            // entity.IsHOLD=isHo
            //  entity.AccountID=curren
            _stockLotRule.AddStockDetailAsyns(entity, IsCheck);


        }
        #endregion
    }
}
