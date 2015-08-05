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

/**
 *   命名空间:   SCG.SINOStock.ViewModels.Account
 *   文件名:     AccountDetailViewModel
 *   说明:       
 *   创建时间:   2014/2/1 15:01:52
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class AccountDetailViewModel : ViewModelBase
    {
        private AccountRule _rule = null;
        private RoleRule _roleRule = null;
        public AccountDetailViewModel()
        {

            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
                {
                    string ErrMsg = string.Empty;
                    RoleCollection = new ObservableCollection<Role>(_roleRule.GetRoleList(new Dictionary<string, string>(), ref ErrMsg));
                    CurrentAccount = param.Entity as Account;
                    if (CurrentAccount == null)
                        CurrentAccount = new Account();
                    else
                    {

                        CurrentRole = RoleCollection.FirstOrDefault(p => p.ID == CurrentAccount.RoleID);
                    }
                }, ThreadOption.UIThread, true, p => p.Target == "AccountDetailViewModel");
            //    CurrentAccount = new Account();

            _rule = new AccountRule();
            _rule.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("IsBusy"))
                {
                    IsBusy = _rule.IsBusy;
                }
            };
            _rule.AddAccountCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("添加成功");
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AccountView,
                            Target = "Sell",
                        });
                    }
                };
            _rule.ModifyAccountCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        Common.MessageBox.Show("修改成功");
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AccountView,
                            Target = "Sell",
                        });
                    }
                };
            _roleRule = new RoleRule();
            _roleRule.GetRolesCompleted += (s, e) =>
                {
                    if (e.Cancelled)
                    {
                        Common.MessageBox.Show(e.Error.Message);
                    }
                    else
                    {
                        RoleCollection = e.Results as ObservableCollection<Role>;
                    }
                };

        }
        #region 界面绑定属性
        private ObservableCollection<Role> _roleCollection;
        public ObservableCollection<Role> RoleCollection
        {
            get { return _roleCollection; }
            set
            {
                _roleCollection = value;
                this.RaisePropertyChanged("RoleCollection");
            }
        }
        private Role _currentRole;
        public Role CurrentRole
        {
            get { return _currentRole; }
            set
            {
                _currentRole = value;
                this.RaisePropertyChanged("CurrentRole");
            }
        }
        private Account _currentAccount;
        public Account CurrentAccount
        {
            get { return _currentAccount; }
            set
            {
                _currentAccount = value;
                this.RaisePropertyChanged("CurrentAccount");
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
                            cmdViewName = CmdViewName.AccountView,
                            Target = "Sell",
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdGotoList;
            }
            set { _cmdGotoList = value; }
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
                        //  _roleRule.GetRolesAsyns();
                        // GetAccountsAsyns();

                    });
                }
                return _cmdPageLoad;
            }
        }

        private DelegateCommand _cmdSave;
        public DelegateCommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new DelegateCommand(() =>
                    {
                        if (CurrentAccount != null)
                        {
                            if (string.IsNullOrWhiteSpace(CurrentAccount.LoginNumber))
                            {
                                Common.MessageBox.Show("请输入编号");
                                return;
                            }
                            if (string.IsNullOrWhiteSpace(CurrentAccount.Name))
                            {
                                Common.MessageBox.Show("请输入姓名");
                                return;
                            }
                            if (CurrentRole == null)
                            {
                                Common.MessageBox.Show("请选择所属角色");
                                return;
                            }
                            CurrentAccount.RoleID = CurrentRole.ID;
                            if (CurrentAccount.ID <= 0)
                                _rule.AddAccountAsyns(CurrentAccount);
                            else
                            {
                                _rule.ModifyAccountAsyns(CurrentAccount);
                            }
                        }
                    });
                }
                return _cmdSave;
            }
        }
        #endregion
    }
}
