using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     SINOStockServiceProxy
 *   说明:       WCF客户端 账户管理代理类
 *   创建时间:   2014/1/23 17:49:57
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public partial class SINOStockServiceProxy
    {

        public IAsyncResult BeginGetAccountList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetAccountList(checkCode, AccountID, queryList, ref ErrMsg, pCallback, asyncState);
        }

        public Account[] EndGetAccountList(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetAccountList(ref ErrMsg, result);
        }

        /// <summary>
        /// 账户登录
        /// </summary>
        /// <param name="strNumber"></param>
        /// <param name="strPwd"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public Account Login(string strNumber, string strPwd, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///账户登录
        /// </summary>
        /// <param name="strNumber"></param>
        /// <param name="strPwd"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public IAsyncResult BeginLogin(string strNumber, string strPwd, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginLogin(strNumber, strPwd, ref ErrMsg, pCallback, asyncState);
        }
        public Account EndLogin(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndLogin(ref ErrMsg, result);
        }

        #region 登录限制改变
        public Account Login_Ex(string strMACAddress, string strNumber, string strPwd, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginLogin_Ex(string strMACAddress, string strNumber, string strPwd, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginLogin_Ex(strMACAddress, strNumber, strPwd, ref ErrMsg, pCallback, asyncState);
        }

        public Account EndLogin_Ex(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndLogin_Ex(ref ErrMsg, result);
        }

        public void ExitCurrentAccount(string checkCode, int AccountID)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginExitCurrentAccount(string checkCode, int AccountID, AsyncCallback callback, object asyncState)
        {
            //IncrementCallCount();
            //var pCallback = new AsyncCallback((ar) =>
            //{
            //    DecrementCallCount();
            //    callback(ar);
            //});
            return _client.BeginExitCurrentAccount(checkCode, AccountID, callback, asyncState);
        }

        public void EndExitCurrentAccount(IAsyncResult result)
        {
            _client.EndExitCurrentAccount(result);
        }
        #endregion



        public bool AddAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginAddAccount(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndAddAccount(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndAddAccount(ref ErrMsg, result);
        }

        public bool ModifyAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginModifyAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginModifyAccount(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndModifyAccount(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndModifyAccount(ref ErrMsg, result);
        }

        public bool DelAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginDelAccount(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndDelAccount(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndDelAccount(ref ErrMsg, result);
        }
        public bool ChangePwd(string checkCode, int AccountID, string OldPwd, string NewPwd, ref string ErrMsg)
        {
            try
            {
                return _client.ChangePwd(checkCode, AccountID, OldPwd, NewPwd, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public IAsyncResult BeginChangePwd(string checkCode, int AccountID, string OldPwd, string NewPwd, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndChangePwd(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}
