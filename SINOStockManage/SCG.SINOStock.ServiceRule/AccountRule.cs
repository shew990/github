using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     AccountRule
 *   说明:       
 *   创建时间:   2014/1/24 17:33:49
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class AccountRule : RuleBase
    {
        public event EventHandler<ResultArgs<Account>> LoginCompleted;
        public event EventHandler<ResultsArgs<Account>> GetAccountsCompleted;
        private ObservableCollection<Account> _accountCollection;
        public ObservableCollection<Account> AccountCollection
        {
            get { return _accountCollection; }
            set
            {
                if (!ReferenceEquals(_accountCollection, value))
                {
                    _accountCollection = value;
                    OnPropertyChanged("AccountCollection");
                }
            }
        }


        /// <summary>
        /// 登录（异步）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="password"></param>
        public void LoginAsyns(string code, string password)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginLogin_Ex(Common.MACAddressHelper.GetMACAddress(), code, password, ref ErrMsg, result =>
                {
                    ThreadHelper.BeginInvokeOnUIThread(() =>
                    {
                        try
                        {
                            Account loginInfo = Proxy.EndLogin_Ex(ref ErrMsg, result);
                            if (loginInfo != null)
                            {
                                LoginCompleted(this, new ResultArgs<Account>(loginInfo, null, false, result.AsyncState));
                            }
                            else
                            {
                                LoginCompleted(this, new ResultArgs<Account>(null, new Exception(ErrMsg), true, result.AsyncState));
                            }
                        }
                        catch (Exception ex)
                        {
                            if (LoginCompleted != null && (_lastError == null || AllowMultipleErrors))
                            {
                                LoginCompleted(this, new ResultArgs<Account>(null, ex, true, result.AsyncState));
                            }
                            _lastError = ex;
                        }
                    });
                }, null);
        }

        /// <summary>
        /// 通知服务器，当前用户退出登录，不需要返回值
        /// </summary>
        public void ExitCurrentAccountAsyns()
        {
            if (CurrentAccount != null)
                Proxy.BeginExitCurrentAccount(CurrentAccount.CheckCode, CurrentAccount.ID, null, null);
        }

        public void GetAccountsAsyns(Dictionary<string, string> queryList)
        {
            string ErrMsg = string.Empty;
            //   Dictionary<string, string> queryList = new Dictionary<string, string>();
            //if (!string.IsNullOrWhiteSpace(StrLoginName))
            //    queryList.Add("Name", StrLoginName.Trim());
            //if (!string.IsNullOrWhiteSpace(StrLoginNumber))
            Proxy.BeginGetAccountList(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {

                    try
                    {
                        AccountCollection = new ObservableCollection<Account>(Proxy.EndGetAccountList(ref ErrMsg, result));
                        if (GetAccountsCompleted != null)
                        {
                            GetAccountsCompleted(this, new ResultsArgs<Account>(AccountCollection, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetAccountsCompleted != null)
                        {
                            GetAccountsCompleted(this, new ResultsArgs<Account>(null, ex, true, result.AsyncState));
                        }
                        _lastError = ex;
                    }
                });

            }, null);
        }

        public event EventHandler<ResultArgs<bool>> AddAccountCompleted;
        public void AddAccountAsyns(Account entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginAddAccount(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
                {
                    ThreadHelper.BeginInvokeOnUIThread(() =>
                 {
                     try
                     {
                         bool bResult = Proxy.EndAddAccount(ref ErrMsg, result);
                         if (bResult)
                         {
                             AddAccountCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                         }
                         else
                         {
                             AddAccountCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                         }
                     }
                     catch (Exception ex)
                     {
                         if (AddAccountCompleted != null)
                         {
                             AddAccountCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                         }
                     }
                 });
                }, null);
        }
        public event EventHandler<ResultArgs<bool>> ModifyAccountCompleted;

        public void ModifyAccountAsyns(Account entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginModifyAccount(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndModifyAccount(ref ErrMsg, result);
                        if (bResult)
                        {
                            ModifyAccountCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            ModifyAccountCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ModifyAccountCompleted != null)
                        {
                            ModifyAccountCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> DelAccountCompleted;

        public void DelAccountAsyns(Account entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginDelAccount(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndDelAccount(ref ErrMsg, result);
                        if (bResult)
                        {
                            DelAccountCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            DelAccountCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DelAccountCompleted != null)
                        {
                            DelAccountCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public bool ChangePwd(string OldPwd, string NewPwd, ref string ErrMsg)
        {
            return Proxy.ChangePwd(CurrentAccount.CheckCode, CurrentAccount.ID, OldPwd, NewPwd, ref ErrMsg);
        }
    }
}
