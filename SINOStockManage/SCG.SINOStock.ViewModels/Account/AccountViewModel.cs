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
 *   命名空间:   SCG.SINOStock.ViewModels.Account
 *   文件名:     AccountViewModel
 *   说明:       用户管理MainViewModel类
 *   创建时间:   2013/12/12 18:26:05
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private AccountRule _rule;
        private RoleRule _roleRule;
        public AccountViewModel()
        {
            _rule = new AccountRule();
            _roleRule = new RoleRule();
            _rule.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals("IsBusy"))
                {
                    IsBusy = _rule.IsBusy;
                }
            };
            _rule.GetAccountsCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    AccountList = new ObservableCollection<Account>(e.Results);
                }
            };
            _rule.DelAccountCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Common.MessageBox.Show(e.Error.Message);
                }
                else
                {
                    Common.MessageBox.Show("删除成功");
                    Dictionary<string, string> queryList = new Dictionary<string, string>();
                    if (!string.IsNullOrWhiteSpace(StrLoginName))
                        queryList.Add("Name", StrLoginName.Trim());
                    if (!string.IsNullOrWhiteSpace(_strLoginNumber))
                        queryList.Add("Number", StrLoginName.Trim());
                    _rule.GetAccountsAsyns(queryList);
                }
            };
            this._eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        StrLoginName = string.Empty;
                        StrLoginNumber = string.Empty;
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (!loginAcount.LoginNumber.Equals("admin"))
                        {
                            AddVisibility = Visibility.Collapsed;
                            ModifyVisibility = Visibility.Collapsed;
                            DeleteVisibility = Visibility.Collapsed;

                            string roleDetail = loginAcount.Role.RoleDetail;
                            if (roleDetail.Contains(_roleRule.str账户管理 + _roleRule.str添加))
                                AddVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str账户管理 + _roleRule.str修改))
                                ModifyVisibility = Visibility.Visible;
                            if (roleDetail.Contains(_roleRule.str账户管理 + _roleRule.str删除))
                                DeleteVisibility = Visibility.Visible;
                        }
                        else
                        {
                            AddVisibility = Visibility.Visible;
                            ModifyVisibility = Visibility.Visible;
                            DeleteVisibility = Visibility.Visible;
                        }
                        break;
                    case CmdName.Edit:
                        break;
                    case CmdName.Manager:

                        break;
                    default:
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "AccountViewModel");

        }
        #region 界面绑定属性
        private Visibility _addVisibility;
        public Visibility AddVisibility
        {
            get { return _addVisibility; }
            set
            {
                _addVisibility = value;
                this.RaisePropertyChanged("AddVisibility");
            }
        }
        private Visibility _modifyVisibility;
        public Visibility ModifyVisibility
        {
            get { return _modifyVisibility; }
            set
            {
                _modifyVisibility = value;
                this.RaisePropertyChanged("ModifyVisibility");
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
        private string _strLoginNumber;

        public string StrLoginNumber
        {
            get { return _strLoginNumber; }
            set
            {
                _strLoginNumber = value;
                this.RaisePropertyChanged("StrLoginNumber");
            }
        }
        private string _strLoginName;

        public string StrLoginName
        {
            get { return _strLoginName; }
            set
            {
                _strLoginName = value;
                this.RaisePropertyChanged("StrLoginName");
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

        private ObservableCollection<Account> _accountList;
        public ObservableCollection<Account> AccountList
        {
            get { return _accountList; }
            set
            {
                _accountList = value;
                this.RaisePropertyChanged("AccountList");
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
                        Dictionary<string, string> queryList = new Dictionary<string, string>();
                        if (!string.IsNullOrWhiteSpace(StrLoginName))
                            queryList.Add("Name", StrLoginName.Trim());
                        if (!string.IsNullOrWhiteSpace(StrLoginNumber))
                            queryList.Add("Number", StrLoginNumber.Trim());
                        _rule.GetAccountsAsyns(queryList);
                    });
                }
                return _cmdPageLoad;
            }
            set { _cmdPageLoad = value; }
        }

        private DelegateCommand _cmdAdd;
        public DelegateCommand CmdAdd
        {
            get
            {
                if (_cmdAdd == null)
                {
                    _cmdAdd = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AccountDetailView,
                            Target = "Sell",
                            Entity = null,
                        });
                        //   GetAccountsAsyns();
                    });
                }
                return _cmdAdd;
            }
            set { _cmdAdd = value; }
        }
        private DelegateCommand _cmdModify;
        public DelegateCommand CmdModify
        {
            get
            {
                if (_cmdModify == null)
                {
                    _cmdModify = new DelegateCommand(() =>
                    {
                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdViewName = CmdViewName.AccountDetailView,
                            Target = "Sell",
                            Entity = CurrentAccount,
                        });
                    });
                }
                return _cmdModify;
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
                        if (CurrentAccount.LoginNumber.Equals("admin"))
                        {
                            Common.MessageBox.Show("admin账户不允许删除");
                            return;
                        }
                        Account loginAcount = Common.ServiceDataLocator.GetInstance<Account>();
                        if (CurrentAccount.ID == loginAcount.ID)
                        {
                            Common.MessageBox.Show("不能删除自己");
                            return;
                        }
                        if (Common.MessageBox.Show("确认删除该账户?", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            _rule.DelAccountAsyns(CurrentAccount);
                        }
                    });
                }
                return _cmdDelete;
            }
        }
        #endregion
        /// <summary>
        /// 获取用户集合
        /// </summary>
        public void GetAccountsAsyns111()
        {
            string ErrMsg = string.Empty;
            Dictionary<string, string> queryList = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(StrLoginName))
                queryList.Add("Name", StrLoginName.Trim());
            if (!string.IsNullOrWhiteSpace(StrLoginNumber))
                queryList.Add("Number", StrLoginNumber.Trim());
            Proxy.BeginGetAccountList("111", 1, queryList, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        try
                        {
                            List<Account> lst = Proxy.EndGetAccountList(ref ErrMsg, result).ToList();
                            AccountList = new ObservableCollection<Account>(lst);
                        }
                        catch (Exception ex)
                        {
                            int i = 1;
                        }
                        //if (GetAccountsCompleted != null)
                        //{
                        //    GetAccountsCompleted(this, new ResultsArgs<Account>(AccountCollection, null, false, result.AsyncState));
                        //}
                    }
                    catch (Exception ex)
                    {
                        //if (GetAccountsCompleted != null && _lastError == null)
                        //{
                        //    GetAccountsCompleted(this, new ResultsArgs<Account>(null, ex, true, result.AsyncState));
                        //}
                        _lastError = ex;
                    }
                });

            }, null);

        }
    }
}
