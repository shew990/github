using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.Process
 *   文件名:     Process_PaoGuangViewModel
 *   说明:       
 *   创建时间:   2014/2/21 15:21:50
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class Process_PaoGuangViewModel : ViewModelBase
    {
        private int STATIC_STATUS = 3;//3为抛光
        private StockLotRule _stockLotRule = null;

        private StockDetail SaveEntity = null;
        private RoleRule _roleRule = null;
        public Process_PaoGuangViewModel()
        {

            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        ReadComTest();
                        CurrentStockLot = new StockLot();
                        TXTLOTNOISEnabled = true;
                        //  GlassID = string.Empty;
                        CountInfo = string.Empty;//统计初始化


                          Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            HOLDVisibility = Visibility.Collapsed;
 
                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_roleRule.str减薄后检验 + _roleRule.str接触HOLD))
                                HOLDVisibility = Visibility.Visible;

                        }
                        else
                        {
                            HOLDVisibility = Visibility.Visible;
    
                        }

                        if (!string.IsNullOrWhiteSpace(param.Tag))
                        {
                            CurrentStockLot.LotNo = param.Tag;
                            _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
                        }
                        break;

                    case CmdName.SaveGlassID:
                        SaveEntity = param.Entity as StockDetail;
                        if (SaveEntity.IsFanGong)
                        {
                            SaveEntity.Status = 8;
                            SaveEntity.FanGongNum += 1;
                        }
                        else
                            SaveEntity.Status = STATIC_STATUS;
                        _stockLotRule.ModifyStockDetailListAsyns(SaveEntity);
                        break;
                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "Process_PaoGuangViewModel");
            _stockLotRule = new StockLotRule();
            _roleRule = new RoleRule();
            _stockLotRule.GetStockLotEntityByLotNoCompleted += (s, e) =>
            {
                if (e.Cancelled || e.Results == null)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    if (!e.Results.IsJianBao)
                    {
                        Common.MessageBox.Show("该LOTNO不进行抛光");
                        return;
                    }
                    TXTLOTNOISEnabled = false;
                    _lineNumber = 1;
                    CurrentStockLot = e.Results as StockLot;
                    CurrentStockLot.StockDetails = CurrentStockLot.StockDetails.OrderByDescending(p => p.PaoGuangDT).ToArray();
                    CountInfo = string.Format("            入库数：{0} PCS     HOLD数：{3}     当前已抛光数：{1} PCS      当前返工数：{2} PCS", _stockLotRule.StockInQty, _stockLotRule.OperaterQty, _stockLotRule.FanGongQty, _stockLotRule.HOLDQty);
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.Enter,
                        Target = "Process_PaoGuangView",
                    });

                }
            };

            //验证GlassID方法调用成功后
            _stockLotRule.CheckStockDetailStatusCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                    isDispose = false;
                }
                else
                {
                    StockDetail entity = e.Results as StockDetail;
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdViewName = CmdViewName.ToolScanGlassID_PaoGuangView,
                        Entity = entity,
                        Tag = CurrentStockLot.ImageHOLD,
                        cmdName = CmdName.New,
                        Target = "Sell",
                    });
                }
            };
            _stockLotRule.ModifyStockDetailListCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    List<StockDetail> sd = CurrentStockLot.StockDetails.ToList();
                    SaveEntity.StockLot = CurrentStockLot;

                    StockDetail tmpentity = sd.FirstOrDefault(p => p.GlassID == SaveEntity.GlassID);
                    if (tmpentity != null)
                        sd.Remove(tmpentity);

                    sd.Add(SaveEntity);

                    CurrentStockLot.StockDetails = sd.OrderByDescending(p => p.StockInDT).ToArray();

                    int StockInQty = 0;
                    int OperaterQty = 0;
                    int FanGongQty = 0;
                    int HOLDQty = 0;
                    string ErrMsg = string.Empty;
                    if (_stockLotRule.GetStockLotEntityByLotNoTotal(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll, ref StockInQty, ref OperaterQty, ref FanGongQty, ref HOLDQty, ref ErrMsg))
                        CountInfo = string.Format("            入库数：{0} PCS     HOLD数：{3}     当前已抛光数：{1} PCS      当前返工数：{2} PCS", StockInQty, OperaterQty, FanGongQty, HOLDQty);



                    //  _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
                    GlassID = string.Empty;
                }
                isDispose = false;
            };
            //修改HOLD成功后回调事件
            _stockLotRule.ChangeStockDetail_HOLDCompleted += (s, e) =>
            {
                bool bHOLD = (bool)e.UserState;
                if (e.Cancelled)
                {
                    CurrentDetail.IsHOLD = !bHOLD;
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    if (bHOLD)
                    {
                        Common.MessageBox.Show(CurrentDetail.GlassID + " 设置HOLD状态成功");
                    }
                    else
                    {
                        Common.MessageBox.Show(CurrentDetail.GlassID + " 取消HOLD状态成功");
                    }
                }
            };

            ScanComExecute = (s, e) =>
            {
                GlassID = s.ToString();
                _stockLotRule.CheckStockDetailStatusAsyns(GlassID, CurrentStockLot.ID, STATIC_STATUS);
                //   AddStockDetailAsyns();
            };
        }

        #region 界面绑定字段
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

        private DelegateCommand _cmdPageLoad;
        public DelegateCommand CmdPageLoad
        {
            get
            {
                if (_cmdPageLoad == null)
                {
                    _cmdPageLoad = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Target = "Process_PaoGuangView",
                        });
                    });
                }
                return _cmdPageLoad;
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
        private StockLot _currentStockLot;

        public StockLot CurrentStockLot
        {
            get { return _currentStockLot; }
            set
            {
                _currentStockLot = value;
                // _lineNumber = 1;
                this.RaisePropertyChanged("CurrentStockLot");
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
        private string _countInfo;
        /// <summary>
        ///   入库数：100PCS  当前已减薄数：20PCS
        /// </summary>
        public string CountInfo
        {
            get { return _countInfo; }
            set
            {
                _countInfo = value;
                this.RaisePropertyChanged("CountInfo");
            }
        }

        private StockDetail _currentDetail;

        /// <summary>
        /// 列表选择项
        /// </summary>
        public StockDetail CurrentDetail
        {
            get { return _currentDetail; }
            set
            {
                _currentDetail = value;
                this.RaisePropertyChanged("CurrentDetail");
            }
        }

        private Visibility _hOLDVisibility;
        /// <summary>
        /// 
        /// </summary>
        public Visibility HOLDVisibility
        {
            get { return _hOLDVisibility; }
            set
            {
                _hOLDVisibility = value;
                this.RaisePropertyChanged("HOLDVisibility");
            }
        }
        #endregion
        private DelegateCommand _cmdSorting;
        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand CmdSorting
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
                                _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
                            }
                        }
                    });
                }
                return _cmdLotOperater;
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
                        _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
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
                                //StockDetail entity = new StockDetail();
                                //entity.GlassID = GlassID;
                                //entity.Qty = 1;
                                //entity.StockLotID = CurrentStockLot.ID;
                                //  entity.AccountID=curren
                                //  _stockLotRule.UpdateStockDetailStatusAsyns(GlassID,CurrentStockLot.ID, 3);//todo是否验证写死了

                                _stockLotRule.CheckStockDetailStatusAsyns(GlassID, CurrentStockLot.ID, STATIC_STATUS);
                            }
                        }
                    });
                }
                return _cmdDetailOperater;
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
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定取消该GlassID " + CurrentDetail.GlassID + " 的HOLD状态吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            CurrentDetail.IsHOLD = false;

                            _stockLotRule.ChangeStockDetail_HOLDAsyns(CurrentDetail);
                        }
                    });
                }
                return _cmdModifyHOLD;
            }
        }
    }
}
