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
        public QualityInfo[] GetQualityInfoList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg)
        {
            return _client.GetQualityInfoList(checkCode, AccountID, queryList, ref ErrMsg);
            // throw new NotImplementedException();
        }
        public IAsyncResult BeginGetQualityInfoList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetQualityInfoList(checkCode, AccountID, queryList, ref ErrMsg, pCallback, asyncState);
        }

        public QualityInfo[] EndGetQualityInfoList(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetQualityInfoList(ref ErrMsg, result);
        }

        public bool AddQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginAddQualityInfo(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndAddQualityInfo(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndAddQualityInfo(ref ErrMsg, result);
        }

        public bool ModifyQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginModifyQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginModifyQualityInfo(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndModifyQualityInfo(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndModifyQualityInfo(ref ErrMsg, result);
        }

        public bool DelQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDelQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginDelQualityInfo(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndDelQualityInfo(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndDelQualityInfo(ref ErrMsg, result);
        }
    }
}
