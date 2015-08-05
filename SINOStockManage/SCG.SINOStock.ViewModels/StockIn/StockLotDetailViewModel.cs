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
 *   文件名:     StockLotDetailViewModel
 *   说明:       
 *   创建时间:   2014/3/4 19:06:51
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class StockLotDetailViewModel : ViewModelBase
    {

        private StockLotRule _rule = null;
        private RoleRule _roleRule = null;
        private QualityInfoRule _quInforule;
        public StockLotDetailViewModel()
        {
            _quInforule = new QualityInfoRule();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _rule = new StockLotRule();
            _roleRule = new RoleRule();
            _rule.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("IsBusy"))
                {
                    IsBusy = _rule.IsBusy;
                }
            };
            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {

                GlassSearchId = string.Empty;
                string LotNo = param.Entity as string;
                _rule.GetStockLotEntityByLotNoAsyns(LotNo, 99, true);
                Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                if (!loginAcount.LoginNumber.Equals("admin"))
                {
                    ExportVisibility = Visibility.Collapsed;
                    HOLDVisibility = Visibility.Collapsed;
                    UnHOLDVisibility = Visibility.Collapsed;

                    string roleDetail = loginAcount.Role.RoleDetail;
                    if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str导出))
                        ExportVisibility = Visibility.Visible;
                    if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.strHOLD))
                        HOLDVisibility = Visibility.Visible;
                    if (roleDetail.Contains(_roleRule.str查询GlassID + _roleRule.str接触HOLD))
                        UnHOLDVisibility = Visibility.Visible;
                }
                else
                {
                    ExportVisibility = Visibility.Visible;
                    HOLDVisibility = Visibility.Visible;
                    UnHOLDVisibility = Visibility.Visible;
                }


                //Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                //if (!loginAcount.LoginNumber.Equals("admin"))
                //{
                //    HOLDVisibility = Visibility.Collapsed;
                //    EndStockLotVisibility = Visibility.Collapsed;
                //    string roleDetail = loginAcount.Role.RoleDetail;
                //    if (roleDetail.Contains(_rule.str扫描入库 + _rule.str接触HOLD))
                //        HOLDVisibility = Visibility.Visible;
                //    if (roleDetail.Contains(_rule.str扫描入库 + _rule.str入库结束))
                //        EndStockLotVisibility = Visibility.Visible;
                //}
                //else
                //{
                //    HOLDVisibility = Visibility.Visible;
                //    EndStockLotVisibility = Visibility.Visible;
                //}




                string ErrMsg = string.Empty;
                List<QualityInfo> lst = _quInforule.GetQualityInfoList(new Dictionary<string, string>(), ref ErrMsg);
                if (lst != null)
                {
                    StockInList = new ObservableCollection<string>(lst.Where(p => p.InfoType == "入库").Select(p => p.Name));
                    StockInList.Insert(0, "全部");
                    JianBaoList = new ObservableCollection<string>(lst.Where(p => p.InfoType == "减薄").Select(p => p.Name));
                    JianBaoList.Insert(0, "全部");
                    PaoGuangList = new ObservableCollection<string>(lst.Where(p => p.InfoType == "抛光").Select(p => p.Name));
                    PaoGuangList.Insert(0, "全部");
                    DuMoList = new ObservableCollection<string>(lst.Where(p => p.InfoType == "镀膜").Select(p => p.Name));
                    DuMoList.Insert(0, "全部");


                    StockInSelect = "全部";
                    JianBaoSelect = "全部";
                    PaoGuangSelect = "全部";
                    DuMoSelect = "全部";
                }




            }, ThreadOption.UIThread, true, p => p.Target == "StockLotDetailViewModel");




            _rule.GetStockLotEntityByLotNoCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    _lineNumber = 1;
                    CurrentStockLot = e.Results;
                    //LineNumber = 1;

                    StockDetailCollection = new ObservableCollection<StockDetail>(CurrentStockLot.StockDetails);

                    if (CurrentStockLot.StockDetails != null)
                    {
                        double operaterCount = CurrentStockLot.StockDetails.Where(p => p.Status > 0).Count();
                        double holdCount = CurrentStockLot.StockDetails.Where(p => p.IsHOLD).Count();
                        LotNoInfo = string.Format("当前LotNO：{0}    总GlassID数：{1} ，实际操作数：{2} ，其中HOLD数：{3} 。另：退货数为：{4}", CurrentStockLot.LotNo, CurrentStockLot.PCSQty, operaterCount, holdCount, CurrentStockLot.TuiHuoCount);
                        double stockinCount = CurrentStockLot.StockDetails.Where(p => p.StockInDT != null).Count();
                        StockInInfo = string.Format("已完成入库检验的GlassID：{0}，所占比例：{1}%", stockinCount, (stockinCount / operaterCount) * 100);
                        double jianbaoCount = CurrentStockLot.StockDetails.Where(p => p.JianBaoDT != null).Count();
                        JianBaoInfo = string.Format("已完成减薄后检验的GlassID：{0}，所占比例：{1}%", jianbaoCount, (jianbaoCount / operaterCount) * 100);
                        double paoguangCount = CurrentStockLot.StockDetails.Where(p => p.PaoGuangDT != null).Count();
                        PaoGuangInfo = string.Format("已完成抛光后检验的GlassID：{0}，所占比例：{1}%", paoguangCount, (paoguangCount / operaterCount) * 100);
                        double dumoCount = CurrentStockLot.StockDetails.Where(p => p.DuMoDT != null).Count();
                        DuMoInfo = string.Format("已完成镀膜后检验的GlassID：{0}，所占比例：{1}%", dumoCount, (dumoCount / operaterCount) * 100);

                    }


                }
            };
            //修改HOLD成功后回调事件
            _rule.ChangeStockDetail_HOLDCompleted += (s, e) =>
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
                   //     CurrentDetail.IsHOLD = (bool)e.UserState;
                    }
                };
        }
        private Visibility _exportVisibility;
        public Visibility ExportVisibility
        {
            get { return _exportVisibility; }
            set
            {
                _exportVisibility = value;
                this.RaisePropertyChanged("ExportVisibility");
            }
        }
       
        private Visibility _hOLDVisibility;
        /// <summary>
        /// 是否显示HOLD按钮
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
        private Visibility _unHOLDVisibility;
        /// <summary>
        /// 是否显示解除HOLD按钮
        /// </summary>
        public Visibility UnHOLDVisibility
        {
            get { return _unHOLDVisibility; }
            set
            {
                _unHOLDVisibility = value;
                this.RaisePropertyChanged("UnHOLDVisibility");
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

        private ObservableCollection<StockDetail> _stockDetailCollection;
        public ObservableCollection<StockDetail> StockDetailCollection
        {
            get { return _stockDetailCollection; }
            set
            {
                _stockDetailCollection = value;
                this.RaisePropertyChanged("StockDetailCollection");
            }
        }
        private string _stockInInfo;
        public string StockInInfo
        {
            get { return _stockInInfo; }
            set
            {
                _stockInInfo = value;
                this.RaisePropertyChanged("StockInInfo");
            }
        }
        private string _jianBaoInfo;
        public string JianBaoInfo
        {
            get { return _jianBaoInfo; }
            set
            {
                _jianBaoInfo = value;
                this.RaisePropertyChanged("JianBaoInfo");
            }
        }
        private string _duMoInfo;
        public string DuMoInfo
        {
            get { return _duMoInfo; }
            set
            {
                _duMoInfo = value;
                this.RaisePropertyChanged("DuMoInfo");
            }
        }
        private string _paoGuangInfo;
        public string PaoGuangInfo
        {
            get { return _paoGuangInfo; }
            set
            {
                _paoGuangInfo = value;
                this.RaisePropertyChanged("PaoGuangInfo");
            }
        }
        private string _lotNoInfo;
        public string LotNoInfo
        {
            get { return _lotNoInfo; }
            set
            {
                _lotNoInfo = value;
                this.RaisePropertyChanged("LotNoInfo");
            }
        }
        private ObservableCollection<string> _stockInList;
        public ObservableCollection<string> StockInList
        {
            get { return _stockInList; }
            set
            {
                _stockInList = value;
                this.RaisePropertyChanged("StockInList");
            }
        }

        private ObservableCollection<string> _jianBaoList;
        public ObservableCollection<string> JianBaoList
        {
            get { return _jianBaoList; }
            set
            {
                _jianBaoList = value;
                this.RaisePropertyChanged("JianBaoList");
            }
        }
        private ObservableCollection<string> _paoGuangList;
        public ObservableCollection<string> PaoGuangList
        {
            get { return _paoGuangList; }
            set
            {
                _paoGuangList = value;
                this.RaisePropertyChanged("PaoGuangList");
            }
        }
        private ObservableCollection<string> _duMoList;
        public ObservableCollection<string> DuMoList
        {
            get { return _duMoList; }
            set
            {
                _duMoList = value;
                this.RaisePropertyChanged("DuMoList");
            }
        }
        private string _stockInSelect;
        public string StockInSelect
        {
            get { return _stockInSelect; }
            set
            {
                _stockInSelect = value;
                this.RaisePropertyChanged("StockInSelect");
            }
        }

        private string _jianBaoSelect;
        public string JianBaoSelect
        {
            get { return _jianBaoSelect; }
            set
            {
                _jianBaoSelect = value;
                this.RaisePropertyChanged("JianBaoSelect");
            }
        }


        private string _paoGuangSelect;
        public string PaoGuangSelect
        {
            get { return _paoGuangSelect; }
            set
            {
                _paoGuangSelect = value;
                this.RaisePropertyChanged("PaoGuangSelect");
            }
        }


        private string _duMoSelect;
        public string DuMoSelect
        {
            get { return _duMoSelect; }
            set
            {
                _duMoSelect = value;
                this.RaisePropertyChanged("DuMoSelect");
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
        private DelegateCommand _cmdExoprt;
        public DelegateCommand CmdExoprt
        {
            get
            {
                if (_cmdExoprt == null)
                {
                    _cmdExoprt = new DelegateCommand(() =>
                    {
                        // GetAccountsAsyns();
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ToolExportGlassIDsView,
                            Target = "Sell",
                            Entity = CurrentStockLot.StockDetails.ToList(),
                        });
                    });
                }
                return _cmdExoprt;
            }
        }
        private DelegateCommand _cmdQuery;
        public DelegateCommand CmdQuery
        {
            get
            {
                if (_cmdQuery == null)
                {
                    _cmdQuery = new DelegateCommand(() =>
                    {

                        StockDetailCollection = new ObservableCollection<StockDetail>(CurrentStockLot.StockDetails);
                        if ((!string.IsNullOrWhiteSpace(StockInSelect)) && StockInSelect != "全部")
                        {
                            StockDetailCollection = new ObservableCollection<StockDetail>(StockDetailCollection.Where(p => p.StockInInfo != null && p.StockInInfo.Contains(StockInSelect)));
                        }
                        if ((!string.IsNullOrWhiteSpace(JianBaoSelect)) && JianBaoSelect != "全部")
                        {
                            StockDetailCollection = new ObservableCollection<StockDetail>(StockDetailCollection.Where(p => p.JianBaoInfo != null && p.JianBaoInfo.Contains(JianBaoSelect)));
                        }
                        if ((!string.IsNullOrWhiteSpace(PaoGuangSelect)) && PaoGuangSelect != "全部")
                        {
                            StockDetailCollection = new ObservableCollection<StockDetail>(StockDetailCollection.Where(p => p.PaoGuangInfo != null && p.PaoGuangInfo.Contains(PaoGuangSelect)));
                        }
                        if ((!string.IsNullOrWhiteSpace(DuMoSelect)) && DuMoSelect != "全部")
                        {
                            StockDetailCollection = new ObservableCollection<StockDetail>(StockDetailCollection.Where(p => p.DuMoInfo != null && p.DuMoInfo.Contains(DuMoSelect)));
                        }
                        if (!string.IsNullOrWhiteSpace(GlassSearchId))
                        {
                            StockDetailCollection = new ObservableCollection<StockDetail>(StockDetailCollection.Where(p => p.GlassID.Contains(GlassSearchId.ToUpper())));
                        }
                        _lineNumber = 1;
                    });
                }
                return _cmdQuery;
            }
        }
        private DelegateCommand _cmdGotoList;
        public DelegateCommand CmdGotoList
        {
            get
            {
                if (_cmdGotoList == null)
                {
                    _cmdGotoList = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.Manager,
                            cmdViewName = CmdViewName.StockLotMainView,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdGotoList;
            }
            set { _cmdGotoList = value; }
        }

        #region　2015年3月26日14:45:16 新增功能：HOLD与解除HOLD

        private DelegateCommand _cmdHOLD;
        /// <summary>
        /// 为GlassID增加Hold状态
        /// </summary>
        public DelegateCommand CmdHOLD
        {
            get
            {
                if (_cmdHOLD == null)
                {
                    _cmdHOLD = new DelegateCommand(() =>
                    {
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            string roleDetail = loginAcount.Role.RoleDetail;
                            switch (CurrentDetail.Status)
                            {
                                case 1:
                                    if (!roleDetail.Contains(_roleRule.str入库) && !roleDetail.Contains(_roleRule.str入库_无))
                                    {
                                        Common.MessageBox.Show("无权设置入库HOLD状态！");
                                        return;
                                    }

                                    break;
                                case 2:
                                    if (!roleDetail.Contains(_roleRule.str减薄后检验))
                                    {
                                        Common.MessageBox.Show("无权设置减薄HOLD状态！");
                                        return;
                                    }
                                    break;
                                case 3:
                                    if (!roleDetail.Contains(_roleRule.str抛光后检验))
                                    {
                                        Common.MessageBox.Show("无权设置抛光HOLD状态！");
                                        return;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定为该GlassID "+CurrentDetail.GlassID+" 设置HOLD状态吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            CurrentDetail.IsHOLD = true;
                            _rule.ChangeStockDetail_HOLDAsyns(CurrentDetail);
                        }
                    });
                }
                return _cmdHOLD;
            }
            set { _cmdHOLD = value; }
        }
        private DelegateCommand _cmdUnHOLD;
        /// <summary>
        /// 为GlassID 解除HOLD
        /// </summary>
        public DelegateCommand CmdUnHOLD
        {
            get
            {
                if (_cmdUnHOLD == null)
                {
                    _cmdUnHOLD = new DelegateCommand(() =>
                    {
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            string roleDetail = loginAcount.Role.RoleDetail;
                            switch (CurrentDetail.Status)
                            {
                                case 1:
                                    if (!roleDetail.Contains(_roleRule.str入库) && !roleDetail.Contains(_roleRule.str入库_无))
                                    {
                                        Common.MessageBox.Show("无权解除入库HOLD状态！");
                                        return;
                                    }
                                        
                                    break;
                                case 2:
                                    if (!roleDetail.Contains(_roleRule.str减薄后检验))
                                    {
                                        Common.MessageBox.Show("无权解除减薄HOLD状态！");
                                        return;
                                    }
                                    break;
                                case 3:
                                    if (!roleDetail.Contains(_roleRule.str抛光后检验))
                                    {
                                        Common.MessageBox.Show("无权解除抛光HOLD状态！");
                                        return;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (Common.MessageBox.Show(new Common.LED_DialogParameters("询问", "确定取消该GlassID "+CurrentDetail.GlassID+" 的HOLD状态吗？", Common.LED_MessageBoxButton.YesNo)) == Common.LED_MessageBoxResult.Yes)
                        {
                            CurrentDetail.IsHOLD = false;
                            _rule.ChangeStockDetail_HOLDAsyns(CurrentDetail);
                        }
                    });
                }
                return _cmdUnHOLD;
            }
            set { _cmdUnHOLD = value; }
        }

        private string GetStrDetailStatus(int iStatus)
        {
            string strResult = string.Empty;
            switch (iStatus)
            {
                case 0:
                    strResult = "未入库";
                    break;
                case 1:
                    strResult = "已入库";
                    break;
                case 2:
                    strResult = "减薄后";
                    break;
                case 3:
                    strResult = "抛光后";
                    break;
                case 4:
                    strResult = "镀膜后";
                    break;
                case 5:
                    strResult = "已出库";
                    break;
                case 8:
                    strResult = "返工中";
                    break;
                case -1:

                    strResult = "退货";
                    break;
                default:
                    break;
            }
            return strResult;
        }
        #endregion
    }
}
