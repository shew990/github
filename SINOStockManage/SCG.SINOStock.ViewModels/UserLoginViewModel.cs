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
 *   命名空间:   SCG.SINOStock.ViewModels
 *   文件名:     UserLoginViewModel
 *   说明:       
 *   创建时间:   2013/12/13 12:53:33
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class UserLoginViewModel : ViewModelBase
    {
        private AccountRule _rule;
        public UserLoginViewModel()
        {
            _rule = new AccountRule();
            _rule.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("IsBusy"))
                {
                    IsBusy = _rule.IsBusy;
                }
            };
            _rule.LoginCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    Common.ServiceDataLocator.Register<Account>(e.Results);
                    LoginName = string.Empty;
                    LoginPwd = string.Empty;
                    //Account account = Common.ServiceDataLocator.GetInstance<Account>();
                    //  int i = 1;
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdViewName = CmdViewName.MainView,
                        Entity = null,
                        Target = "Sell",
                    });
                }
            };
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:

                        break;
                    case CmdName.Edit:
                        break;
                    case CmdName.Manager:
                        break;
                    default:
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "UserLoginViewModel");

        }
        private string _loginName;

        public string LoginName
        {
            get { return _loginName; }
            set
            {
                _loginName = value;
                this.RaisePropertyChanged("LoginName");
            }
        }
        private string _loginPwd;

        public string LoginPwd
        {
            get { return _loginPwd; }
            set
            {
                _loginPwd = value;
                this.RaisePropertyChanged("LoginPwd");
            }
        }
        public bool LoginFailed { get; set; }
        private DelegateCommand<RoutedEventArgs> _cmdLogin;
        public DelegateCommand<RoutedEventArgs> CmdLogin
        {
            get
            {
                if (_cmdLogin == null)
                {
                    _cmdLogin = new DelegateCommand<RoutedEventArgs>(e =>
                    {
                        var key = e as KeyEventArgs;
                        if (key == null || key.Key == Key.Enter)
                        {
                            if (string.IsNullOrWhiteSpace(LoginName))
                            {
                                Common.MessageBox.Show(new Common.LED_DialogParameters() { Content = "请输入用户名" });
                                LoginFailed = false; // 改变模型状态
                                this.RaisePropertyChanged("LoginFailed"); // 通知UI对状态变化作出反应
                                LoginFailed = true; // 改变模型状态
                                this.RaisePropertyChanged("LoginFailed"); // 通知UI对状态变化作出反应
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(LoginPwd))
                            {
                                Common.MessageBox.Show(new Common.LED_DialogParameters() { Content = "请输入密码" });
                                return;
                            }
                            _rule.LoginAsyns(LoginName, LoginPwd);
                            
                        }
                    });
                }
                return _cmdLogin;
            }
            set { _cmdLogin = value; }
        }
        private DelegateCommand _cmdExit;
        public DelegateCommand CmdExit
        {
            get
            {
                if (_cmdExit == null)
                {
                    _cmdExit = new DelegateCommand(() =>
                    {
                        Application.Current.Shutdown();
                    });
                }
                return _cmdExit;
            }
            set { _cmdExit = value; }
        }
        private DelegateCommand _cmdTestLogin;
        public DelegateCommand CmdTestLogin
        {
            get
            {
                if (_cmdTestLogin == null)
                {
                    _cmdTestLogin = new DelegateCommand(() =>
                    {
                        _rule.LoginAsyns("admin", "admin");

                        //Common.MessageBox.Show(Common.MACAddressHelper.GetMACAddress().Replace(":", ""));

                        //_eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        //{
                        //    cmdViewName = CmdViewName.ToolScanGlassID_StockInView,
                        //  //  Entity = CurrentStockLot,
                        //    Target = "Sell",
                        //});

                        //ImportStockLotView view = new ImportStockLotView();
                        //int a = Convert.ToInt32(LoginName);
                        //int b = Convert.ToInt32(LoginPwd);
                        //int i = a & b;
                        //Common.MessageBox.Show(i.ToString());

                    });
                }
                return _cmdTestLogin;
            }

        }

    }
}
