using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Common;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

/**
 *   命名空间:   SCG.SINOStock.ViewModels
 *   文件名:     MenuMainViewModel
 *   说明:       
 *   创建时间:   2013/12/11 16:04:23
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class MenuMainViewModel : ViewModelBase
    {
        private RoleRule _rule = null;
        public MenuMainViewModel()
        {
            _rule = new RoleRule();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                if (!loginAcount.LoginNumber.Equals("admin"))
                {
                    AccountVisibility = Visibility.Collapsed;
                    RoleVisibility = Visibility.Collapsed;
                    FormworkVisibility = Visibility.Collapsed;
                    StockInVisibility = Visibility.Collapsed;
                    StockInNoVisibility = Visibility.Collapsed;
                    JianBaoVisibility = Visibility.Collapsed;
                    PaoGuangVisibility = Visibility.Collapsed;
                    DuMoVisibility = Visibility.Collapsed;
                    DuMoNoVisibility = Visibility.Collapsed;
                    ImportVisibility = Visibility.Collapsed;
                    GlassIDVisibility = Visibility.Collapsed;
                    QualityInfoVisibility = Visibility.Collapsed;
                    GlassIDModifyVisibility = Visibility.Collapsed;


                    string roleMain = loginAcount.Role.RoleMain;
                    if (roleMain.Contains(_rule.str账户管理))
                        AccountVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str角色管理))
                        RoleVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str模版管理))
                        FormworkVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str扫描入库))
                        StockInVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str扫描入库_无对比))
                        StockInNoVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str减薄后检验))
                        JianBaoVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str抛光后检验))
                        PaoGuangVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str镀膜后检验))
                        DuMoVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str镀膜后检验_无对比))
                        DuMoNoVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str导入GlassID))
                        ImportVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str查询GlassID))
                        GlassIDVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.str品质信息管理))
                        QualityInfoVisibility = Visibility.Visible;
                    if (roleMain.Contains(_rule.strGLassID后台))
                        GlassIDModifyVisibility = Visibility.Visible;
                }
                else
                {
                    AccountVisibility = Visibility.Visible;
                    RoleVisibility = Visibility.Visible;
                    FormworkVisibility = Visibility.Visible;
                    StockInVisibility = Visibility.Visible;
                    StockInNoVisibility = Visibility.Visible;
                    JianBaoVisibility = Visibility.Visible;
                    PaoGuangVisibility = Visibility.Visible;
                    DuMoVisibility = Visibility.Visible;
                    DuMoNoVisibility = Visibility.Visible;
                    ImportVisibility = Visibility.Visible;
                    GlassIDVisibility = Visibility.Visible;
                    QualityInfoVisibility = Visibility.Visible;
                    GlassIDModifyVisibility = Visibility.Visible;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "MenuMainViewModel");
        }
        #region 权限控制
        private Visibility _accountVisibility;
        public Visibility AccountVisibility
        {
            get { return _accountVisibility; }
            set
            {
                _accountVisibility = value;
                this.RaisePropertyChanged("AccountVisibility");
            }
        }
        private Visibility _roleVisibility;
        public Visibility RoleVisibility
        {
            get { return _roleVisibility; }
            set
            {
                _roleVisibility = value;
                this.RaisePropertyChanged("RoleVisibility");
            }
        }
        private Visibility _formworkVisibility;
        public Visibility FormworkVisibility
        {
            get { return _formworkVisibility; }
            set
            {
                _formworkVisibility = value;
                this.RaisePropertyChanged("FormworkVisibility");
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
        private Visibility _stockInNoVisibility;
        public Visibility StockInNoVisibility
        {
            get { return _stockInNoVisibility; }
            set
            {
                _stockInNoVisibility = value;
                this.RaisePropertyChanged("StockInNoVisibility");
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
        private Visibility _duMotVisibility;
        public Visibility DuMoVisibility
        {
            get { return _duMotVisibility; }
            set
            {
                _duMotVisibility = value;
                this.RaisePropertyChanged("DuMoVisibility");
            }
        }
        private Visibility _duMoNoVisibility;
        public Visibility DuMoNoVisibility
        {
            get { return _duMoNoVisibility; }
            set
            {
                _duMoNoVisibility = value;
                this.RaisePropertyChanged("DuMoNoVisibility");
            }
        }
        private Visibility _importVisibility;
        public Visibility ImportVisibility
        {
            get { return _importVisibility; }
            set
            {
                _importVisibility = value;
                this.RaisePropertyChanged("ImportVisibility");
            }
        }
        private Visibility _glassIDVisibility;
        public Visibility GlassIDVisibility
        {
            get { return _glassIDVisibility; }
            set
            {
                _glassIDVisibility = value;
                this.RaisePropertyChanged("GlassIDVisibility");
            }
        }
        private Visibility _glassIDMidifyVisibility;
        public Visibility GlassIDModifyVisibility
        {
            get { return _glassIDMidifyVisibility; }
            set
            {
                _glassIDMidifyVisibility = value;
                this.RaisePropertyChanged("GlassIDModifyVisibility");
            }
        }
        private Visibility _qualityInfoVisibility;
        public Visibility QualityInfoVisibility
        {
            get { return _qualityInfoVisibility; }
            set
            {
                _qualityInfoVisibility = value;
                this.RaisePropertyChanged("QualityInfoVisibility");
            }
        }
        #endregion
        private DelegateCommand _cmdGotoAccountView;
        public DelegateCommand CmdGotoAccountView
        {
            get
            {
                if (_cmdGotoAccountView == null)
                {
                    _cmdGotoAccountView = new DelegateCommand(() =>
                    {
                        //MessageBox.Show(new LED_DialogParameters()
                        //{
                        //    Content = "wocao"
                        //});

                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AccountView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoAccountView;
            }
            set { _cmdGotoAccountView = value; }
        }
        private DelegateCommand _cmdGotoFormworkMainView;
        public DelegateCommand CmdGotoFormworkMainView
        {
            get
            {
                if (_cmdGotoFormworkMainView == null)
                {
                    _cmdGotoFormworkMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.FormworkMainView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoFormworkMainView;
            }
            set { _cmdGotoFormworkMainView = value; }
        }
        private DelegateCommand _cmdGotoStockInMainView;
        public DelegateCommand CmdGotoStockInMainView
        {
            get
            {
                if (_cmdGotoStockInMainView == null)
                {
                    _cmdGotoStockInMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.StockInMainView,
                            Entity = true,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoStockInMainView;
            }
            set { _cmdGotoFormworkMainView = value; }
        }
        private DelegateCommand _cmdGotoStockInNoMainView;
        public DelegateCommand CmdGotoStockInNoMainView
        {
            get
            {
                if (_cmdGotoStockInNoMainView == null)
                {
                    _cmdGotoStockInNoMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.StockInMainView,
                            Entity = false,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoStockInNoMainView;
            }
        }

        private DelegateCommand _cmdGotoStockOutMainView;
        public DelegateCommand CmdGotoStockOutMainView
        {
            get
            {
                if (_cmdGotoStockOutMainView == null)
                {
                    _cmdGotoStockOutMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.StockOutMainView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoStockOutMainView;
            }
            set { _cmdGotoFormworkMainView = value; }
        }
        private DelegateCommand _cmdGotoRoleMainView;
        public DelegateCommand CmdGotoRoleMainView
        {
            get
            {
                if (_cmdGotoRoleMainView == null)
                {
                    _cmdGotoRoleMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.RoleMainView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoRoleMainView;
            }
            set { _cmdGotoFormworkMainView = value; }
        }

        private DelegateCommand _cmdGotoProcess_JianBaoView;
        public DelegateCommand CmdGotoProcess_JianBaoView
        {
            get
            {
                if (_cmdGotoProcess_JianBaoView == null)
                {
                    _cmdGotoProcess_JianBaoView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.Process_JianBaoView,
                            Target = "Sell",
                        });
                    });

                }
                return _cmdGotoProcess_JianBaoView;
            }
            set
            {
                _cmdGotoProcess_JianBaoView = value;
            }
        }

        private DelegateCommand _cmdGotoProcess_PaoGuangView;
        public DelegateCommand CmdGotoProcess_PaoGuangView
        {
            get
            {
                if (_cmdGotoProcess_PaoGuangView == null)
                {
                    _cmdGotoProcess_PaoGuangView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.Process_PaoGuangView,
                            Target = "Sell",
                        });
                    });

                }
                return _cmdGotoProcess_PaoGuangView;
            }
            set
            {
                _cmdGotoProcess_PaoGuangView = value;
            }
        }
        private DelegateCommand _cmdGotoProcess_DuMoView;
        public DelegateCommand CmdGotoProcess_DuMoView
        {
            get
            {
                if (_cmdGotoProcess_DuMoView == null)
                {
                    _cmdGotoProcess_DuMoView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.Process_DuMoView,
                            Target = "Sell",
                        });
                    });

                }
                return _cmdGotoProcess_DuMoView;
            }
            set
            {
                _cmdGotoProcess_DuMoView = value;
            }
        }
        private DelegateCommand _cmdGotoProcess_FanGongView;
        public DelegateCommand CmdGotoProcess_FanGongView
        {
            get
            {
                if (_cmdGotoProcess_FanGongView == null)
                {
                    _cmdGotoProcess_FanGongView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.Process_FanGongView,
                            Target = "Sell",
                        });
                    });

                }
                return _cmdGotoProcess_FanGongView;
            }
        }
        private DelegateCommand _cmdGotoImportStockLotView;
        public DelegateCommand CmdGotoImportStockLotView
        {
            get
            {
                if (_cmdGotoImportStockLotView == null)
                {
                    _cmdGotoImportStockLotView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ImportStockLotView,
                            Target = "Sell",
                        });
                    });

                }
                return _cmdGotoImportStockLotView;
            }
        }
        private DelegateCommand _cmdGotoQualityInfoMainView;
        public DelegateCommand CmdGotoQualityInfoMainView
        {
            get
            {
                if (_cmdGotoQualityInfoMainView == null)
                {
                    _cmdGotoQualityInfoMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.QualityInfoMainView,
                            Target = "Sell",
                        });
                    });

                }
                return _cmdGotoQualityInfoMainView;
            }
        }
        private DelegateCommand _cmdGotoStockLotMainView;
        public DelegateCommand CmdGotoStockLotMainView
        {
            get
            {
                if (_cmdGotoStockLotMainView == null)
                {
                    _cmdGotoStockLotMainView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.StockLotMainView,
                            cmdName=CmdName.New,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoStockLotMainView;
            }
        }
        private DelegateCommand _cmdGotoModifyGlassIDView;
        public DelegateCommand CmdGotoModifyGlassIDView
        {
            get
            {
                if (_cmdGotoModifyGlassIDView == null)
                {
                    _cmdGotoModifyGlassIDView = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.ModifyGlassIDView,
                            Target = "Sell",
                        });
                    });
                }
                return _cmdGotoModifyGlassIDView;
            }
        }
        private DelegateCommand _cmdTest;

        public DelegateCommand CmdTest
        {
            get
            {
                if (_cmdTest == null)
                {
                    _cmdTest = new DelegateCommand(() =>
                    {
                        LED_MessageBoxResult result = Common.MessageBox.Show(new LED_DialogParameters()
                          {
                              Content = "弹出测试",
                          });
                        Common.MessageBox.Show(new LED_DialogParameters()
                        {
                            Content = result.ToString(),
                        });
                    });
                }
                return _cmdTest;
            }
            set { _cmdTest = value; }
        }
    }
}
