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
        public Role[] GetRoleList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg)
        {
            return _client.GetRoleList(checkCode, AccountID, queryList, ref ErrMsg);
           // throw new NotImplementedException();
        }
        public IAsyncResult BeginGetRoleList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetRoleList(checkCode, AccountID, queryList, ref ErrMsg, pCallback, asyncState);
        }

        public Role[] EndGetRoleList(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetRoleList(ref ErrMsg, result);
        }

        public bool AddRole(string checkCode, int AccountID, Role entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddRole(string checkCode, int AccountID, Role entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginAddRole(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndAddRole(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndAddRole(ref ErrMsg, result);
        }

        public bool ModifyRole(string checkCode, int AccountID, Role entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginModifyRole(string checkCode, int AccountID, Role entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginModifyRole(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndModifyRole(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndModifyRole(ref ErrMsg, result);
        }

        public bool DelRole(string checkCode, int AccountID, Role entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelRole(string checkCode, int AccountID, Role entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginDelRole(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndDelRole(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndDelRole(ref ErrMsg, result);
        }
    }
}
