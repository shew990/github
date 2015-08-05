using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

/**
 *   命名空间:   SCG.SINOStock.ViewModels.StockIn
 *   文件名:     StockLotMainViewModel
 *   说明:       
 *   创建时间:   2014/3/4 18:34:23
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class StockLotMainViewModel : ViewModelBase
    {
        private StockLotRule _rule = null;
        private RoleRule _roleRule = null;
        public StockLotMainViewModel()
        {

            _roleRule = new RoleRule();

            _rule = new StockLotRule();
            _rule.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("IsBusy"))
                {
                    IsBusy = _rule.IsBusy;
                }
            };
            _rule.GetStockLotList_TwoCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                        StockLotCollection = null;
                    }
                    else
                    {
                        _lineNumber = 1;
                        StockLotCollection = new ObservableCollection<StockLot>(e.Results);
                    }
                };
            _rule.DeleteStockLotCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("删除成功");
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        if (EndDt < StartDt)
                        {
                            Common.MessageBox.Show("开始时间不能大于结束时间");
                            return;
                        }
                        queryList.Add("StartDt", StartDt.ToString());
                        queryList.Add("EndDt", EndDt.AddDays(1).ToString());
                        _rule.GetStockLotList_TwoAsyns(queryList);
                    }
                };
            _rule.HOLDAllToNewStockLotCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("转移成功");
                        Dictionary<string, string> queryList = new Dictionary<string, string>();

                        queryList.Add("StartDt", StartDt.ToString());
                        queryList.Add("EndDt", EndDt.AddDays(1).ToString());
                        _rule.GetStockLotList_TwoAsyns(queryList);
                    }
                };
            //订阅
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        GlassSearchId = string.Empty;
                        EndDt = DateTime.Now;
                        StartDt = EndDt.AddMonths(-1);

                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "StockLotMainViewModel");
        }
        private ObservableCollection<StockLot> _stockLotCollection;
        public ObservableCollection<StockLot> StockLotCollection
        {
            get { return _stockLotCollection; }
            set
            {
                _stockLotCollection = value;
                this.RaisePropertyChanged("StockLotCollection");
            }
        }
        private DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set
            {
                _startDt = value;
                this.RaisePropertyChanged("StartDt");
            }
        }
        private Visibility _deleteVisibility;
        public Visibility DeleteVisibility
        {
            get { return _deleteVisibility; }
            set
            {
                _deleteVisibility = value;
                this.RaisePropertyChanged("DeleteVisibility");
            }
        }
        private Visibility _stockInVisibility;
        public Visibility StockInVisibility
        {
            get { return _stockInVisibility; }
            set
            {
                _stockInVisibility = value;
                this.RaisePropertyChanged("StockInVisibility");
            }
        }
        private Visibility _stockIn_NOVisibility;
        public Visibility StockIn_NOVisibility
        {
            get { return _stockIn_NOVisibility; }
            set
            {
                _stockIn_NOVisibility = value;
                this.RaisePropertyChanged("StockIn_NOVisibility");
            }
        }
        private Visibility _jianBaoVisibility;
        public Visibility JianBaoVisibility
        {
            get { return _jianBaoVisibility; }
            set
            {
                _jianBaoVisibility = value;
                this.RaisePropertyChanged("JianBaoVisibility");
            }
        }
        private Visibility _paoGuangVisibility;
        public Visibility PaoGuangVisibility
        {
            get { return _paoGuangVisibility; }
            set
            {
                _paoGuangVisibility = value;
                this.RaisePropertyChanged("PaoGuangVisibility");
            }
        }
        private Visibility _duMoVisibility;
        public Visibility DuMoVisibility
        {
            get { return _duMoVisibility; }
            set
            {
                _duMoVisibility = value;
                this.RaisePropertyChanged("DuMoVisibility");
            }
        }
        private Visibility _zhuanYiVisibility;
        public Visibility ZhuanYiVisibility
        {
            get { return _zhuanYiVisibility; }
            set
            {
                _zhuanYiVisibility = value;
                this.RaisePropertyChanged("ZhuanYiVisibility");
            }
        }
        private Visibility _detailVisibility;
        public Visibility DetailVisibility
        {
            get { return _detailVisibility; }
            set
            {
                _detailVisibility = value;
                this.RaisePropertyChanged("DetailVisibility");
            }
        }
        private DateTime _endDt;
        public DateTime EndDt
        {
            get { return _endDt; }
            set
            {
                _endDt = value;
                this.RaisePropertyChanged("EndDt");
            }
        }
        private StockLot _currentStockLot;
        public StockLot CurrentStockLot
        {
            get { return _currentStockLot; }
            set
            {
                _currentStockLot = value;
                this.RaisePropertyChanged("CurrentStockLot");
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
                        if (EndDt < StartDt)
                        {
                            Common.MessageBox.Show("开始时间不能大于结束时间");
                            return;
                        }
                        queryList.Add("StartDt", StartDt.ToString());
                        queryList.Add("EndDt", EndDt.AddDays(1).ToString());
                        if (!string.IsNullOrWhiteSpace(GlassSearchId))
                        {
                            queryList.Add("GlassID", GlassSearchId);
                        }

                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            DeleteVisibility = Visibility.Collapsed;
                            StockInVisibility = Visibility.Collapsed;
                            StockIn_NOVisibility = Visibility.Collapsed;
                            JianBaoVisibility = Visibility.Collapsed;
                            PaoGuangVisibility = Visibility.Collapsed;
                            DuMoVisibility = Visibility.Collapsed;
                            ZhuanYiVisibility = Visibility.Collapsed;
                            DetailVisibility = Visibility.Collapsed;

                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str删除))
                                DeleteVisibility = Visibility.Visible;

                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str入库))
                                StockInVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str入库_无))
                                StockIn_NOVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str减薄))
                                JianBaoVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str抛光))
                                PaoGuangVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str镀膜))
                                DuMoVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.strGLASSID明细))
                                DetailVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str转移GLASSID))
                                ZhuanYiVisibility = Visibility.Visible;
                        }
                        else
                        {
                            DeleteVisibility = Visibility.Visible;
                            StockInVisibility = Visibility.Visible;
                            StockIn_NOVisibility = Visibility.Visible;
                            JianBaoVisibility = Visibility.Visible;
                            PaoGuangVisibility = Visibility.Visible;
                            DuMoVisibility = Visibility.Visible;
                            ZhuanYiVisibility = Visibility.Visible;
                            DetailVisibility = Visibility.Visible;

                        }
                        _rule.GetStockLotList_TwoAsyns(queryList);
                    });
                }
                return _cmdPageLoad;
            }
        }
        private DelegateCommand _cmdDetail;
        public DelegateCommand CmdDetail
        {
            get
            {
                if (_cmdDetail == null)
                {
                    _cmdDetail = new DelegateCommand(() =>
                    {
                        // GetAccountsAsyns();
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = CurrentStockLot.LotNo,
                            cmdViewName = CmdViewName.StockLotDetailView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdDetail;
            }
        }
        private DelegateCommand _cmdDelete;
        public DelegateCommand CmdDelete
        {
            get
            {
                if (_cmdDelete == null)
                {
                    _cmdDelete = new DelegateCommand(() =>
                    {
                        if (Common.MessageBox.Show("确定删除该LOTNO吗（删除之后，所属的GlassID也将被删除）？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            _rule.DeleteStockLotAsyns(CurrentStockLot.ID);
                        }
                    });
                }
                return _cmdDelete;
            }
        }

        #region 2014年5月29日 11:05:55  新增GlassID查询
        private string _glassSearchID;
        public string GlassSearchId
        {
            get { return _glassSearchID; }
            set
            {
                _glassSearchID = value;
                this.RaisePropertyChanged("GlassSearchId");
            }
        }
        #endregion
        private DelegateCommand _cmdDAllToNewStockLot;
        public DelegateCommand HOLDAllToNewStockLot
        {
            get
            {
                if (_cmdDAllToNewStockLot == null)
                {
                    _cmdDAllToNewStockLot = new DelegateCommand(() =>
                    {
                        if (CurrentStockLot.Status != 1)
                        {
                            Common.MessageBox.Show("当前GlassID尚未入库结束，不能进行HOLD转移");
                            return;
                        }
                        if (Common.MessageBox.Show("将该HOLD下所有状态为HOLD的glassID转移到新的LOTNO下么？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            _rule.HOLDAllToNewStockLotAsyns(CurrentStockLot.ID);
                        }
                    });
                }
                return _cmdDAllToNewStockLot;
            }
        }


        #region 跳转到工序流程
        private DelegateCommand _cmdGotoStockIn;
        public DelegateCommand CmdGotoStockIn
        {
            get
            {
                if (_cmdGotoStockIn == null)
                {
                    _cmdGotoStockIn = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            cmdViewName = CmdViewName.StockInMainView,
                            Entity = true,
                            Tag = CurrentStockLot.LotNo,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoStockIn;
            }
        }
        private DelegateCommand _cmdGotoStockIn_NO;
        public DelegateCommand CmdGotoStockIn_NO
        {
            get
            {
                if (_cmdGotoStockIn_NO == null)
                {
                    _cmdGotoStockIn_NO = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            cmdViewName = CmdViewName.StockInMainView,
                            Entity = false,
                            Tag = CurrentStockLot.LotNo,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoStockIn_NO;
            }
        }
        private DelegateCommand _cmdGotoJianBao;
        public DelegateCommand CmdGotoJianBao
        {
            get
            {
                if (_cmdGotoJianBao == null)
                {
                    _cmdGotoJianBao = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = CurrentStockLot.LotNo,
                            cmdViewName = CmdViewName.Process_JianBaoView,
                            Tag = CurrentStockLot.LotNo,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoJianBao;
            }
        }
        private DelegateCommand _cmdGotoPaoGuang;
        public DelegateCommand CmdGotoPaoGuang
        {
            get
            {
                if (_cmdGotoPaoGuang == null)
                {
                    _cmdGotoPaoGuang = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = CurrentStockLot.LotNo,
                            cmdViewName = CmdViewName.Process_PaoGuangView,
                            Tag = CurrentStockLot.LotNo,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoPaoGuang;
            }
        }
        private DelegateCommand _cmdGotoDuMo;
        public DelegateCommand CmdGotoDuMo
        {
            get
            {
                if (_cmdGotoDuMo == null)
                {
                    _cmdGotoDuMo = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = CurrentStockLot.LotNo,
                            cmdViewName = CmdViewName.StockOutMainView,
                            Tag = CurrentStockLot.LotNo,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoDuMo;
            }
        }
        #endregion
    }
}
