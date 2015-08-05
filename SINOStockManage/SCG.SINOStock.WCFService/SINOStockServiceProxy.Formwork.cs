using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     SINOStockServiceProxy
 *   说明:       WCF客户端 模板代理类
 *   创建时间:   2014/2/2 11:38:31
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public partial class SINOStockServiceProxy
    {
        public FormWork[] GetFormWorkList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetFormWorkList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetFormWorkList(checkCode, AccountID, queryList, ref ErrMsg, pCallback, asyncState);
        }

        public FormWork[] EndGetFormWorkList(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetFormWorkList(ref ErrMsg, result);
        }

        public bool AddFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginAddFormWork(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndAddFormWork(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndAddFormWork(ref ErrMsg, result);
        }

        public bool ModifyFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginModifyFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginModifyFormWork(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndModifyFormWork(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndModifyFormWork(ref ErrMsg, result);
        }

        public bool DelFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginDelFormWork(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndDelFormWork(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndDelFormWork(ref ErrMsg, result);
        }

        public string[] GetProductStrList()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetProductStrList(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public string[] EndGetProductStrList(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public FormWork GetFormWorkByProModel(string checkCode, int AccountID, string proModel, ref string ErrMsg)
        {
            return _client.GetFormWorkByProModel(checkCode, AccountID, proModel, ref ErrMsg);
        }

        public IAsyncResult BeginGetFormWorkByProModel(string checkCode, int AccountID, string proModel, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetFormWorkByProModel(checkCode, AccountID, proModel, ref ErrMsg, pCallback, asyncState);
        }

        public FormWork EndGetFormWorkByProModel(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetFormWorkByProModel(ref ErrMsg, result);
        }
    }
}
