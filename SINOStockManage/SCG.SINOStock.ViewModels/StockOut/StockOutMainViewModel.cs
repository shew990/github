using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;



/**
 *   命名空间:   SCG.SINOStock.ViewModels.StockOut
 *   文件名:     StockOutMainViewModel
 *   说明:       
 *   创建时间:   2014/2/21 16:28:41
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class StockOutMainViewModel : ViewModelBase
    {
        private int STATIC_STATUS = 4;//表明该Model是处理哪个流程 4为镀膜
        private StockLotRule _stockLotRule = null;
        private FormworkRule _formWorkRule = null;
        private StockBoxRule _stockBoxRule = null;
        private TrayRule _trayRule = null;
        private StockDetail SaveEntity = null;//用以保存扫描的GlassID
        public StockOutMainViewModel()
        {
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _stockBoxRule = new StockBoxRule();
            _trayRule = new TrayRule();
            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        CurrentStockLotList = null;
                        CurrentStockDetailList = null;
                        CurrentStockLot = new StockLot();
                        CurrentStockDetail = new StockDetail();
                        ScanStockDetail = null;
                        // _lotNo = string.Empty;
                        CurrentFormwork = null;
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.Refresh,
                            Entity = null,
                            Target = "StockOutMainView",
                        });

                        ReadComTest();

                        if (!string.IsNullOrWhiteSpace(param.Tag))
                        {
                            CurrentStockLot.LotNo = param.Tag;
                            _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
                        }
                        else
                            GetUnPrintData();
                        break;
                    case CmdName.SaveGlassID:

                        //一箱开始
                        if (ScanStockDetail == null || ScanStockDetail.Count() <= 0)
                        {
                            string ErrMsg = string.Empty;
                            CurrentStockBox = _stockBoxRule.GetMaxStockBox_Ex(ref ErrMsg);
                        }
                        SaveEntity = param.Entity as StockDetail;
                        SaveEntity.Status = STATIC_STATUS;
                        SaveEntity.DuMoNum = JiTaiHao;
                        SaveEntity.StockBoxID = CurrentStockBox.ID;
                        _stockLotRule.ModifyStockDetailListAsyns(SaveEntity);
                        break;
                    case CmdName.SendPrintData_Box:

                        PrintBoxBarCode(param);
                        break;
                    case CmdName.SendPrintData_Tray:

                        PrintTrayBarCode(param);
                        break;
                    case CmdName.SendPrintData_Box_Again:

                        PrintBoxBarCode_Again(param);
                        break;
                    case CmdName.SendPrintData_Tray_Again:

                        PrintTrayBarCode_Again(param);
                        break;
                    case CmdName.Close:

                        if (ScanStockDetail != null && ScanStockDetail.Count() > 0)
                        {
                            Common.MessageBox.Show("当前正在进行的镀膜（出库）工序中有未打印外箱标签的GlassID，请打印后继续。");
                            return;
                            //if (Common.MessageBox.Show("当前正在进行的镀膜（出库）工序中有未打印外箱标签的GlassID，确定放弃操作返回主界面？", Common.LED_MessageBoxButton.YesNo) != Common.LED_MessageBoxResult.Yes)
                            //    return;
                            //else
                            //{
                            //    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            //    {
                            //        cmdViewName = CmdViewName.MainView,
                            //        Target = "Sell",

                            //    });
                            //}
                        }
                        else
                        {
                            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            {
                                cmdViewName = param.cmdViewName,
                                Target = "Sell",

                            });
                        }
                        break;
                    default:

                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "StockOutMainViewModel");
            _stockLotRule = new StockLotRule();
            _formWorkRule = new FormworkRule();
            _stockLotRule.GetStockLotEntityByLotNoCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    if (CurrentStockLotList == null)
                        CurrentStockLotList = new ObservableCollection<StockLot>();

                    if (CurrentStockLotList.Count() >= 1)
                    {
                        if (CurrentStockLotList[0].ProModel != e.Results.ProModel)
                        {
                            Common.MessageBox.Show("只允许同一型号的LOTNO进行多批次操作");
                            return;
                        }
                        //判断是否允许混批
                        if (e.Results.GuanKong.Equals("单独管控品"))
                        {
                            Common.MessageBox.Show("当前LOTNO " + e.Results.LotNo + " 为单独管控品，不能混批");
                            return;
                        }

                    }

                    CurrentStockLotList.Add(e.Results);
                    RefreshStockDetails();

                    //通知界面更新LotNO列表
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.Refresh,
                        Entity = CurrentStockLotList.Select(p => p.ID).ToList(),
                        Target = "StockOutMainView",
                    });

                }
            };
            _stockLotRule.GetStockLotEntityListByLotNoCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    CurrentStockLotList = new ObservableCollection<StockLot>(e.Results);
                    RefreshStockDetails();
                }
            };
            _formWorkRule.GetFormWorkByProModelCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        CurrentFormwork = e.Results as FormWork;


                        //这里加是否满箱判断原因：当得到上次异常蛮像的数据时，这个异步方法还未执行完毕
                        if (CurrentFormwork != null && ScanStockDetail != null && ScanStockDetail.SelectMany(p => p.Item).Count() >= CurrentFormwork.BoxPCSQty)
                        {
                            MessageBox.Show("BOX已满，点击确定打印外箱凭条");
                            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            {
                                cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                                Entity = PrintType.Box,
                                Tag = CurrentStockLot.ID.ToString(),
                                cmdName = CmdName.New,
                                Target = "Sell",
                                Entity1 = CurrentStockBox,
                            });

                        }
                    }
                };
            //_stockLotRule.UpdateStockDetailStatusCompleted += (s, e) =>
            //{
            //    if (e.Cancelled)
            //    {
            //        Common.MessageBox.Show(e.Error.Message);
            //    }
            //    else
            //    {
            //        _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS);
            //        GlassID = string.Empty;
            //    }
            //};
            _stockLotRule.CheckStockDetailStatus_OutCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                        isDispose = false;
                    }
                    else
                    {


                        CurrentStockDetail = e.Results as StockDetail;
                        var tmpStockLot = CurrentStockLotList.FirstOrDefault(p => p.ID == CurrentStockDetail.StockLotID);
                        //_stockLotRule.ModifyStockDetailListAsyns(CurrentStockDetail);
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ToolScanGlassID_StockOutView,
                            Entity = CurrentStockDetail,
                            Tag = tmpStockLot.ImageHOLD,
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
                        if (ScanStockDetail == null)
                            ScanStockDetail = new ObservableCollection<PrintHelperEx>();



                        PrintHelperEx ex = ScanStockDetail.FirstOrDefault(p => p.Value == CurrentStockDetail.StockLot.LotNo);
                        if (ex == null)
                        {
                            ex = new PrintHelperEx();
                            ex.Value = CurrentStockDetail.StockLot.LotNo;
                            ScanStockDetail.Add(ex);
                        }
                        ex.Item.Add(CurrentStockDetail);

                        GlassID = string.Empty;


                        if (CurrentFormwork != null && ScanStockDetail.SelectMany(p => p.Item).Count() >= CurrentFormwork.BoxPCSQty)
                        {
                            MessageBox.Show("BOX已满，点击确定打印外箱凭条");
                            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            {
                                cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                                Entity = PrintType.Box,
                                Tag = CurrentStockLot.ID.ToString(),
                                cmdName = CmdName.New,
                                Target = "Sell",
                                Entity1 = CurrentStockBox,
                            });

                        }

                        // _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
                        //_stockLotRule.GetStockLotEntityListByLotNoAsyns(CurrentStockLotList.Select(p => p.ID).ToList(), STATIC_STATUS, IsCheckAll);

                        StockLot tmpsl = CurrentStockLotList.FirstOrDefault(p => p.LotNo == ex.Value);
                        List<StockDetail> sd = tmpsl.StockDetails.ToList();
                        sd.Add(SaveEntity);
                        tmpsl.StockDetails = sd.ToArray();
                        RefreshStockDetails();

                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.Refresh,
                            Entity = CurrentStockLotList.Select(p => p.ID).ToList(),
                            Target = "StockOutMainView",
                        });
                    }
                    isDispose = false;
                };
            ScanComExecute = (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(JiTaiHao))
                {
                    Common.MessageBox.Show("必须输入机台号才能开始扫描");
                    isDispose = false;
                    return;
                }
                GlassID = s.ToString();
                _stockLotRule.CheckStockDetailStatus_OutAsyns(GlassID, CurrentStockLotList.Select(p => p.ID).ToList(), STATIC_STATUS);
                // AddStockDetailAsyns();
            };

        }
        #region 界面绑定字段

        private string _jiTaiHao;
        public string JiTaiHao
        {
            get { return _jiTaiHao; }
            set
            {
                _jiTaiHao = value;
                this.RaisePropertyChanged("JiTaiHao");
            }
        }
        private int _pCSQty;
        public int PCSQty
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
        private FormWork _currentFormwork;
        public FormWork CurrentFormwork
        {
            get { return _currentFormwork; }
            set
            {
                _currentFormwork = value;
                this.RaisePropertyChanged("CurrentFormwork");
            }
        }
        //private string _lotNo;
        //public string LotNo
        //{
        //    get { return _lotNo; }
        //    set
        //    {
        //        _lotNo = value;
        //        this.RaisePropertyChanged("LotNo");
        //    }
        //}

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
        private ObservableCollection<StockLot> _currentStockLotList;

        public ObservableCollection<StockLot> CurrentStockLotList
        {
            get { return _currentStockLotList; }
            set
            {
                _currentStockLotList = value;
                //      _lineNumber = 1;
                this.RaisePropertyChanged("CurrentStockLotList");
            }
        }
        private ObservableCollection<PrintHelperEx> _scanStockDetail;

        public ObservableCollection<PrintHelperEx> ScanStockDetail
        {
            get { return _scanStockDetail; }
            set
            {
                _scanStockDetail = value;
                this.RaisePropertyChanged("ScanStockDetail");
            }
        }
        private StockLot _currentStockLot;

        public StockLot CurrentStockLot
        {
            get { return _currentStockLot; }
            set
            {
                _currentStockLot = value;
                //      _lineNumber = 1;
                this.RaisePropertyChanged("CurrentStockLot");
            }
        }
        private ObservableCollection<StockDetail> _currentStockDetailList;

        public ObservableCollection<StockDetail> CurrentStockDetailList
        {
            get { return _currentStockDetailList; }
            set
            {
                _currentStockDetailList = value;
                _lineNumber = 1;
                this.RaisePropertyChanged("CurrentStockDetailList");
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
        private StockDetail _currentStockDetail;
        public StockDetail CurrentStockDetail
        {
            get { return _currentStockDetail; }
            set
            {
                _currentStockDetail = value;
                this.RaisePropertyChanged("CurrentStockDetail");
            }
        }
        private StockBox _currentStockBox;
        public StockBox CurrentStockBox
        {
            get { return _currentStockBox; }
            set
            {
                _currentStockBox = value;
                this.RaisePropertyChanged("CurrentStockBox");
            }
        }

        //private ObservableCollection<StockDetail> _scanStockDetail;
        //public ObservableCollection<StockDetail> ScanStockDetail
        //{
        //    get { return _scanStockDetail; }
        //    set
        //    {
        //        _scanStockDetail = value;
        //        this.RaisePropertyChanged("ScanStockDetail");
        //    }

        //}
        #endregion
        #region 界面处理事件
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
                            GetCurrentStockLot();
                        }
                    });
                }
                return _cmdLotOperater;
            }
        }
        private void GetCurrentStockLot()
        {
            if (string.IsNullOrWhiteSpace(CurrentStockLot.LotNo))
            {
                Common.MessageBox.Show("请填写LOT NO");
                return;
            }
            if ((CurrentStockLotList != null && CurrentStockLotList.Count() >= 10))
            {
                Common.MessageBox.Show("多批次镀膜至多只允许10个LotNO同时镀膜");
                return;
            }
            if (CurrentStockLotList != null && CurrentStockLotList.Any(p => p.LotNo.ToUpper() == CurrentStockLot.LotNo.ToUpper()))
            {
                Common.MessageBox.Show("当前LOT NO已填写");
                return;
            }
            if (CurrentStockLotList != null && CurrentStockLotList.Count() > 0 && CurrentStockLotList[0].GuanKong.Equals("单独管控品"))
            {
                Common.MessageBox.Show("之前输入的LOTNO " + CurrentStockLotList[0].LotNo + " 为单独管控品，不可混批");
                return;
            }
            _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);
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
        private DelegateCommand _cmdShowAll;
        public DelegateCommand CmdShowAll
        {
            get
            {
                if (_cmdShowAll == null)
                {

                    _cmdShowAll = new DelegateCommand(() =>
                    {
                        if (CurrentStockLotList != null && CurrentStockLotList.Count() > 0)
                            _stockLotRule.GetStockLotEntityListByLotNoAsyns(CurrentStockLotList.Select(p => p.ID).ToList(), STATIC_STATUS, IsCheckAll);
                    });
                }
                return _cmdShowAll;
            }
        }
        private DelegateCommand _cmdForceStockBox;
        /// <summary>
        /// 强制打印StockBox
        /// </summary>
        public DelegateCommand CmdForceStockBox
        {
            get
            {
                if (_cmdForceStockBox == null)
                {
                    _cmdForceStockBox = new DelegateCommand(() =>
                    {
                        if (CurrentStockLotList == null || CurrentStockLotList.Count() <= 0)
                        {
                            Common.MessageBox.Show("请先输入至少一个LotNO之后回车");
                            return;
                        }
                        if (ScanStockDetail == null || ScanStockDetail.Count() <= 0)
                        {
                            Common.MessageBox.Show("至少扫描一个GlassID才能进行强制打印");
                            return;
                        }
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                            Entity = PrintType.ForceBox,
                            Tag = CurrentStockLot.ID.ToString(),
                            cmdName = CmdName.New,
                            Target = "Sell",
                            Entity1 = CurrentStockBox,
                        });
                    });

                }
                return _cmdForceStockBox;
            }
        }
        private DelegateCommand _cmdForceTray;
        /// <summary>
        /// 强制打印StockBox
        /// </summary>
        public DelegateCommand CmdForceTray
        {
            get
            {
                if (_cmdForceTray == null)
                {
                    _cmdForceTray = new DelegateCommand(() =>
                    {
                        if (CurrentStockLotList == null || CurrentStockLotList.Count() <= 0)
                        {
                            Common.MessageBox.Show("请先输入至少一个LotNO之后回车");
                            return;
                        }
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                            Entity = PrintType.ForceTray,
                            Tag = CurrentStockLot.ID.ToString(),
                            cmdName = CmdName.New,
                            Target = "Sell",
                            IsQiangDa = true,
                        });
                    });

                }
                return _cmdForceTray;
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
                            Target = "StockOutMainView",
                        });
                        JiTaiHao = string.Empty;
                        GlassID = string.Empty;
                    });
                }
                return _cmdPageLoad;
            }
        }
        private DelegateCommand _cmdAgainTray;
        /// <summary>
        /// 强制打印Tray
        /// </summary>
        public DelegateCommand CmdAgainTray
        {
            get
            {
                if (_cmdAgainTray == null)
                {
                    _cmdAgainTray = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AgainEnterNoPrint,
                            Entity = 1,
                            cmdName = CmdName.New,
                            Target = "Sell",

                        });
                    });

                }
                return _cmdAgainTray;
            }
        }
        private DelegateCommand _cmdAgainStockBox;
        /// <summary>
        /// 强制打印StockBox
        /// </summary>
        public DelegateCommand CmdAgainStockBox
        {
            get
            {
                if (_cmdAgainStockBox == null)
                {
                    _cmdAgainStockBox = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AgainEnterNoPrint,
                            Entity = 0,
                            cmdName = CmdName.New,
                            Target = "Sell",

                        });
                    });

                }
                return _cmdAgainStockBox;
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
                                if (CurrentStockLotList == null || CurrentStockLotList.Count() <= 0)
                                {
                                    Common.MessageBox.Show("请先输入LOTNO，确认型号及数量，再开始扫描");
                                    GlassID = string.Empty;
                                    return;
                                }
                                if (string.IsNullOrWhiteSpace(JiTaiHao))
                                {
                                    Common.MessageBox.Show("必须输入机台号才能开始扫描");
                                    isDispose = false;
                                    return;
                                }
                                //StockDetail entity = new StockDetail();
                                //entity.GlassID = GlassID;
                                //entity.Qty = 1;
                                //entity.StockLotID = CurrentStockLot.ID;
                                //  entity.AccountID=curren
                                //   _stockLotRule.UpdateStockDetailStatusAsyns(GlassID,CurrentStockLot.ID, STATIC_STATUS);//todo是否验证写死了
                                _stockLotRule.CheckStockDetailStatus_OutAsyns(GlassID, CurrentStockLotList.Select(p => p.ID).ToList(), STATIC_STATUS);

                            }
                        }
                    });
                }
                return _cmdDetailOperater;
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
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定开始新的出库吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            //LotNo = string.Empty;
                            CurrentFormwork = null;
                            PCSQty = 0;
                            // CurrentStockLot = null;
                            CurrentStockLotList = null;
                            CurrentStockDetailList = null;
                            ControlsEnabled = true;
                            SumQty = 0;
                            SurQty = 0;
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

                        //if (CurrentStockLot == null)
                        //{
                        //    Common.MessageBox.Show("当前没有可结束的Lot No");
                        //    return;
                        //}
                        //if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定结束该LotNo吗？ 结束之后将不能继续对该LotNo继续扫描", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        //{
                        //    //    _stockLotRule.
                        //}
                    });
                }
                return _cmdEndLotNo;
            }
        }
        #endregion


        #region 私有方法
        private void PrintBoxBarCode(CmdEventParam param)
        {

            try
            {
                StockBox sb = param.Entity as StockBox;
                string ErrMsg = string.Empty;
                // PrintItemType[] strArray = new PrintItemType[200];

                List<string> tmpLotNos = ScanStockDetail.Select(p => p.Value).Distinct().ToList();
                int[] iList = ScanStockDetail.SelectMany(p => p.Item).Select(p => p.ID).Distinct().ToArray();
                bool IsPrintTray = false;
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\BOXFormat.txt";
                if (!File.Exists(path))
                {
                    Common.MessageBox.Show("打印格式文件不存在，请检查");
                    return;
                }
                if (_stockBoxRule.ModifyStockBox(sb, iList, CurrentStockLotList.Select(p => p.ID).ToArray(), ref IsPrintTray, ref ErrMsg))
                {
                    //strArray[0] = new PrintItemType();
                    //strArray[0].Value = _currentStockLot.ProModel;

                    //strArray[1] = new PrintItemType();
                    //strArray[1].Value = DateTime.Now.ToString("yyyyMMdd");
                    //strArray[2] = new PrintItemType();
                    //strArray[2].Value = sb.BarCode;




                    //int i = 3;
                    //foreach (PrintHelperEx item in ScanStockDetail)
                    //{
                    //    strArray[i] = new PrintItemType();
                    //    strArray[i].Value = item.Value;
                    //    strArray[i].Type = PrintTypeEnum.LOTNO;
                    //    foreach (StockDetail detial in item.Item)
                    //    {
                    //        i++;
                    //        strArray[i] = new PrintItemType();
                    //        strArray[i].Value = detial.GlassID;
                    //        strArray[i].Type = PrintTypeEnum.GlassID;
                    //    }
                    //    i++;
                    //}
                    //for (int j = 3; j < strArray.Length; j++)
                    //{
                    //    if (strArray[j] == null)
                    //        strArray[j] = new PrintItemType();
                    //}


                    //PrintHelp.PrintStockBox_TXT(sb.BarCode, _currentStockLot.ProModel, ScanStockDetail.ToArray(), ref ErrMsg)

                    if (!PrintHelp.PrintStockBox_EXT(sb.BarCode, CurrentStockLotList[0].ProModel, ScanStockDetail.ToArray(), ref ErrMsg))
                        Common.MessageBox.Show(ErrMsg);
                    else
                    {
                        ScanStockDetail.Clear();
                        //  FormIsBusy = false;

                        if (IsPrintTray)//如果需要打印托号标签
                        {
                            // PrintTrayBarCode(PrintType.Tray);
                            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            {
                                cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                                Entity = PrintType.Tray,
                                Tag = CurrentStockLot.ID.ToString(),
                                cmdName = CmdName.New,
                                Target = "Sell",
                                IsQiangDa = false,
                            });
                        }
                    }
                    //  if (!IsPrintTray)
                    //   {
                    //  RefreshStockDetails();
                    foreach (int item in iList)
                    {
                        var tmpdetail = CurrentStockDetailList.FirstOrDefault(p => p.ID == item);
                        if (tmpdetail != null)
                            tmpdetail.StockBox = sb;
                    }
                    // foreach (var item in tmpLotNos)
                    // {
                    //    var tmpLot=  CurrentStockLotList.FirstOrDefault(p => p.LotNo == item);
                    ////  if(tmpLot)

                    // }

                    // _stockLotRule.GetStockLotEntityListByLotNoAsyns(CurrentStockLotList.Select(p => p.ID).ToList(), STATIC_STATUS, IsCheckAll);
                    //    }
                }
                else
                    Common.MessageBox.Show(ErrMsg);
            }
            catch (Exception ex)
            {
                Common.MessageBox.Show(ex.Message);
            }
        }
        private void PrintTrayBarCode(CmdEventParam param)
        {
            try
            {
                Tray sb = new Tray();
                string[] strArray = new string[40];
                string ErrMsg = string.Empty;
                string strBarCode = param.Entity as string;
                sb.BarCode = strBarCode;

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\TrayFormat.txt";
                if (!File.Exists(path))
                {
                    Common.MessageBox.Show("打印格式文件不存在，请检查");
                    return;
                }
                int iQty = 0;// IsPrintTray = false;
                string[] boxBarCodes = null;
                if (_trayRule.Add_(sb, CurrentStockLotList.Select(p => p.ID).ToArray(), param.IsQiangDa, ref iQty, ref boxBarCodes, ref ErrMsg))
                {

                    strArray[0] = CurrentStockLotList[0].ProModel;
                    strArray[1] = iQty.ToString();
                    strArray[2] = strBarCode;

                    //StringBuilder sb1 = new StringBuilder();//测试
                    //   sb1.Append(strBarCode);
                    if (boxBarCodes != null && boxBarCodes.Count() > 0)
                    {
                        int i = 3;
                        foreach (var item in boxBarCodes)
                        {
                            strArray[i] = item;
                            i++;
                            //   sb1.Append(item + ",");
                        }
                    }

                    // MessageBox.Show("打印成功");
                    // todo:为了方便  注释本段代码


                    //  TestReadTxt.WriteData(@"C:\Users\ende\Desktop\printtest.txt", sb1.ToString(), ref ErrMsg);
                    //PrintHelp.PrintStockTray_TXT(strArray, ref ErrMsg)

                    //if (!PrintHelp.PrintStockTray_TXT(strArray, ref ErrMsg))
                    if (!PrintHelp.PrintStockTray_EXT(strArray, ref ErrMsg))
                        Common.MessageBox.Show(ErrMsg);
                    else
                    {
                        // entityIDList.Clear();
                        //todo:这边为什么会有clear()的代码FormIsBusy = false;

                    }


                }
                else
                {
                    MessageBox.Show(ErrMsg);
                }
                //_stockLotRule.GetStockLotEntityListByLotNoAsyns(CurrentStockLotList.Select(p => p.ID).ToList(), STATIC_STATUS, IsCheckAll);
            }
            catch (Exception ex)
            {
                Common.MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 重打外箱标签
        /// </summary>
        /// <param name="param"></param>
        private void PrintBoxBarCode_Again(CmdEventParam param)
        {

            try
            {
                string BarCode = param.Entity as string;
                string ErrMsg = string.Empty;
                StockBox box = _stockBoxRule.GetStockBoxToBarCode(BarCode, ref ErrMsg);

                if (box.IsModify != null && box.IsModify.Value)
                    //BarCode += "M";
                    BarCode += "*";


                List<PrintHelperEx> lstPrint = new List<PrintHelperEx>();
                if (box.StockDetails != null && box.StockDetails.Count() > 0)
                {
                    foreach (var item in box.StockDetails)
                    {

                        var tmpex = lstPrint.FirstOrDefault(p => p.Value == item.StockLot.LotNo);
                        if (tmpex == null)
                        {
                            tmpex = new PrintHelperEx();
                            tmpex.Value = item.StockLot.LotNo;
                            lstPrint.Add(tmpex);
                        }
                        tmpex.Item.Add(item);
                    }
                }
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\BOXFormat.txt";
                if (!File.Exists(path))
                {
                    Common.MessageBox.Show("打印格式文件不存在，请检查");
                    return;
                }
                //   PrintItemType[] strArray = new PrintItemType[200];

                //strArray[0] = new PrintItemType();
                //strArray[0].Value = "";
                //if (box.StockDetails != null && box.StockDetails.Count() > 0)
                //    strArray[0].Value = box.StockDetails.First().StockLot.ProModel;

                //strArray[1] = new PrintItemType();
                //strArray[1].Value = DateTime.Now.ToString("yyyyMMdd");
                //strArray[2] = new PrintItemType();
                //strArray[1].Value = BarCode;


                //int i = 3;
                //foreach (PrintHelperEx item in lstPrint)
                //{
                //    strArray[i] = new PrintItemType();
                //    strArray[i].Value = item.Value;
                //    strArray[i].Type = PrintTypeEnum.LOTNO;

                //    foreach (StockDetail detial in item.Item)
                //    {
                //        i++;
                //        strArray[i] = new PrintItemType();
                //        strArray[i].Value = detial.GlassID;
                //        strArray[i].Type = PrintTypeEnum.GlassID;
                //    }
                //    i++;
                //}

                //for (int j = 3; j < strArray.Length; j++)
                //{
                //    if (strArray[j] == null)
                //        strArray[j] = new PrintItemType();
                //}

                string strProModel = "";
                if (box.StockDetails != null && box.StockDetails.Count() > 0)
                    strProModel = box.StockDetails.First().StockLot.ProModel;


                if (!PrintHelp.PrintStockBox_EXT(BarCode, strProModel, lstPrint.ToArray(), ref ErrMsg))
                    Common.MessageBox.Show(ErrMsg);
            }
            catch (Exception ex)
            {
                Common.MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 重打托号标签
        /// </summary>
        /// <param name="param"></param>
        private void PrintTrayBarCode_Again(CmdEventParam param)
        {
            try
            {
                string BarCode = param.Entity as string;
                string ErrMsg = string.Empty;
                string[] strArray = new string[40];
                int Qty = 0;
                string ProMode = string.Empty;
                Tray entity = _trayRule.GetTrayByBarCode(BarCode, ref Qty, ref ProMode, ref ErrMsg);
                if (entity == null)
                {
                    Common.MessageBox.Show(ErrMsg);
                    return;
                }

                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\TrayFormat.txt";
                if (!File.Exists(path))
                {
                    Common.MessageBox.Show("打印格式文件不存在，请检查");
                    return;
                }
                int iQty = 0;// IsPrintTray = false;
                //  string[] boxBarCodes = null;

                strArray[0] = ProMode;
                strArray[1] = Qty.ToString();
                strArray[2] = BarCode;


                if (entity.StockBoxes != null && entity.StockBoxes.Count() > 0)
                {
                    int i = 3;
                    foreach (var item in entity.StockBoxes)
                    {
                        strArray[i] = item.BarCode;
                        i++;
                    }
                }
                //if (!PrintHelp.PrintStockTray_TXT(strArray, ref ErrMsg))
                if (!PrintHelp.PrintStockTray_EXT(strArray, ref ErrMsg))
                    Common.MessageBox.Show(ErrMsg);
            }
            catch (Exception ex)
            {
                Common.MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void RefreshStockDetails()
        {
            ControlsEnabled = false;
            _lineNumber = 1;
            if (!ControlsEnabled)
            {
                if (CurrentStockLotList.Count > 0)
                {
                    var details = CurrentStockLotList.SelectMany(p => p.StockDetails).OrderByDescending(p => p.DuMoDT);
                    // PCSQty = CurrentStockLotList.Sum(p => p.PCSQty);
                    //  SumQty = 0;
                    if (details != null && details.Count() > 0)
                    {
                        //   SumQty = details.Count();
                        CurrentStockDetailList = new ObservableCollection<StockDetail>(details);
                    }
                    //   SurQty = PCSQty - SumQty;

                }

                if (CurrentFormwork == null)
                    _formWorkRule.GetFormWorkByProModelAsyns(CurrentStockLotList[0].ProModel);
            }
            else
            {
                // _formworkRule.GetFormWorkListAsyns();
            }
        }


        /// <summary>
        /// 获取未打印的数据
        /// </summary>
        private void GetUnPrintData()
        {
            string ErrMsg = string.Empty;
            StockBox tmpStockBox = _stockBoxRule.GetStocBoxkEntityByIsUnPrint(ref ErrMsg);
            if (tmpStockBox != null)
            {
                if (Common.MessageBox.Show("是否继续扫描上次因异常退出而未满箱或未打印外箱标签的数据？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                {
                    //string ErrMsg = string.Empty;
                    //CurrentStockBox = _stockBoxRule.GetStocBoxkEntityByIsUnPrint(ref ErrMsg);
                    //if (CurrentStockBox != null)//如果找到了
                    {
                        CurrentStockBox = tmpStockBox;
                        //var LotNoList = tmpStockBox.StockDetails.Select(p => p.StockLot.LotNo).Distinct();
                        List<string> LotNOList = new List<string>();
                        ScanStockDetail = new ObservableCollection<PrintHelperEx>();
                        foreach (var item in CurrentStockBox.StockDetails)//循环去查到当前LotNO
                        {

                            if (!LotNOList.Any(p => p.Equals(item.StockLot.LotNo)))
                            {

                                CurrentStockLot.LotNo = item.StockLot.LotNo;
                                _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);

                                LotNOList.Add(item.StockLot.LotNo);
                                PrintHelperEx ex = new PrintHelperEx();
                                ex.Value = item.StockLot.LotNo;
                                ScanStockDetail.Add(ex);
                            }
                            PrintHelperEx tmpEx = ScanStockDetail.FirstOrDefault(p => p.Value.Equals(item.StockLot.LotNo));
                            tmpEx.Item.Add(item);

                        }


                        //判断是否可以直接打印
                        if (CurrentFormwork != null && ScanStockDetail.SelectMany(p => p.Item).Count() >= CurrentFormwork.BoxPCSQty)
                        {
                            MessageBox.Show("BOX已满，点击确定打印外箱凭条");
                            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                            {
                                cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                                Entity = PrintType.Box,
                                Tag = CurrentStockLot.ID.ToString(),
                                cmdName = CmdName.New,
                                Target = "Sell",
                                Entity1 = CurrentStockBox,
                            });

                        }
                    }

                }
                else
                {

                    CurrentStockBox = tmpStockBox;
                    //var LotNoList = tmpStockBox.StockDetails.Select(p => p.StockLot.LotNo).Distinct();
                    List<string> LotNOList = new List<string>();
                    ScanStockDetail = new ObservableCollection<PrintHelperEx>();
                    foreach (var item in CurrentStockBox.StockDetails)//循环去查到当前LotNO
                    {

                        if (!LotNOList.Any(p => p.Equals(item.StockLot.LotNo)))
                        {

                            CurrentStockLot.LotNo = item.StockLot.LotNo;
                            _stockLotRule.GetStockLotEntityByLotNoAsyns(CurrentStockLot.LotNo, STATIC_STATUS, IsCheckAll);

                            LotNOList.Add(item.StockLot.LotNo);
                            PrintHelperEx ex = new PrintHelperEx();
                            ex.Value = item.StockLot.LotNo;
                            ScanStockDetail.Add(ex);
                        }
                        PrintHelperEx tmpEx = ScanStockDetail.FirstOrDefault(p => p.Value.Equals(item.StockLot.LotNo));
                        tmpEx.Item.Add(item);

                    }
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdViewName = CmdViewName.ToolEnterNoToPrintView,
                        Entity = PrintType.ForceBox,
                        Tag = CurrentStockLot.ID.ToString(),
                        cmdName = CmdName.New,
                        Target = "Sell",
                        Entity1 = CurrentStockBox,
                    });
                }
            }

        }
    }

}
