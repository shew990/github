using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     ISINOStockServiceProxy
 *   说明:       
 *   创建时间:   2014/2/10 22:28:46
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public partial class SINOStockServiceProxy
    {
        public bool AddStockLot(string checkCode, int AccountID, StockLot entity, ref int StockLotID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddStockLot(string checkCode, int AccountID, StockLot entity, ref int StockLotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginAddStockLot(checkCode, AccountID, entity, ref StockLotID, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndAddStockLot(ref int StockLotID, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndAddStockLot(ref StockLotID, ref ErrMsg, result);
        }

        public bool EndStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            return _client.EndStockLot(checkCode, AccountID, StockLotID, ref ErrMsg);
        }

        public IAsyncResult BeginEndStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginEndStockLot(checkCode, AccountID, StockLotID, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndEndStockLot(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndEndStockLot(ref ErrMsg, result);
        }

        public bool DeleteStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDeleteStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginDeleteStockLot(checkCode, AccountID, StockLotID, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndDeleteStockLot(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndDeleteStockLot(ref ErrMsg, result);
        }

        public StockLot GetStockLotEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref int OutCount, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref int OutCount, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockLotEntityByLotNo_Out(checkCode, AccountID, LotNo, ref OutCount, ref ErrMsg, pCallback, asyncState);
        }

        public StockLot EndGetStockLotEntityByLotNo_Out(ref int OutCount, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockLotEntityByLotNo_Out(ref OutCount, ref ErrMsg, result);
        }

        public StockLot[] GetStockLotList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockLotList(checkCode, AccountID, queryList, PageCount, PageIndex, ref listCount, ref ErrMsg, pCallback, asyncState);
        }

        public StockLot[] EndGetStockLotList(ref int listCount, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockLotList(ref listCount, ref ErrMsg, result);
        }


        public StockLot GetStockLotEntityByLotNo(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        ///  根据LOTNo得到StockLot实体
        /// </summary>
        /// <returns></returns>
        public IAsyncResult BeginGetStockLotEntityByLotNo(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockLotEntityByLotNo(checkCode, AccountID, LotNo, iStatus, isShowAllDetail, ref Qty, ref OperaterQty, ref FanGongQty, ref HOLDQty, ref ErrMsg, pCallback, asyncState);
        }


        public StockLot EndGetStockLotEntityByLotNo(ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockLotEntityByLotNo(ref Qty, ref OperaterQty, ref FanGongQty, ref HOLDQty, ref ErrMsg, result);
        }

        public StockLot GetStockLotEntityByLotNo_Ex(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int HOLDQty, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotEntityByLotNo_Ex(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int HOLDQty, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockLotEntityByLotNo_Ex(checkCode, AccountID, LotNo, iStatus, isShowAllDetail, ref Qty, ref HOLDQty, ref ErrMsg, pCallback, asyncState);
        }

        public StockLot EndGetStockLotEntityByLotNo_Ex(ref int Qty, ref int HOLDQty, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockLotEntityByLotNo_Ex(ref Qty, ref HOLDQty, ref ErrMsg, result);
        }

        public StockLot[] GetStockLotEntityListByLotNo(string checkCode, int AccountID, int[] LotID, int iStatus, bool isShowAllDetail, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotEntityListByLotNo(string checkCode, int AccountID, int[] LotID, int iStatus, bool isShowAllDetail, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockLotEntityListByLotNo(checkCode, AccountID, LotID, iStatus, isShowAllDetail, ref ErrMsg, pCallback, asyncState);
        }

        public StockLot[] EndGetStockLotEntityListByLotNo(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockLotEntityListByLotNo(ref ErrMsg, result);
        }









        public StockLot[] GetStockLotList_Two(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotList_Two(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockLotList_Two(checkCode, AccountID, queryList, ref ErrMsg, pCallback, asyncState);
        }

        public StockLot[] EndGetStockLotList_Two(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockLotList_Two(ref ErrMsg, result);
        }

        public bool HOLDAllToNewStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将该LOTNO下状态为HOLD的明细全部转移到新的LOTNO下，新的LOTNO号为就LOTNO号+“*”
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="StockLotID">需要转移的LOTNOID<</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <param name="callback">回调函数</param>
        /// <param name="asyncState">异步状态</param>
        /// <returns></returns>
        public IAsyncResult BeginHOLDAllToNewStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginHOLDAllToNewStockLot(checkCode, AccountID, StockLotID, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndHOLDAllToNewStockLot(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndHOLDAllToNewStockLot(ref ErrMsg, result);
        }


        /// <summary>
        /// 获取LOTNO集合中实际入库数and出库数
        /// </summary>
        /// <param name="StockLotIDs">LOTNO ID集合</param>
        /// <returns>结果集</returns>
        public StockOutQtyHelper[] GetStockOutQtys(int[] StockLotIDs)
        {
            return _client.GetStockOutQtys(StockLotIDs);
        }

        public IAsyncResult BeginGetStockOutQtys(int[] StockLotIDs, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockOutQtyHelper[] EndGetStockOutQtys(IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        #region StockDetail


        public StockDetail[] GetStockDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginGetStockDetailList(checkCode, AccountID, StockLotID, AccountID, ref CountQty, ref ErrMsg, pCallback, asyncState);
        }

        public StockDetail[] EndGetStockDetailList(ref int CountQty, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndGetStockDetailList(ref CountQty, ref ErrMsg, result);
        }

        /// <summary>
        /// 添加GlassID
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="entity">新增的实体</param>
        /// <param name="IsCheck">是否需要对比关键字</param>
        /// <param name="QtyCount">添加成功之后，带出该LotNO下实际已入库数量</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool AddStockDetail(string checkCode, int AccountID, StockDetail entity, bool IsCheck, ref int QtyCount, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }


        public IAsyncResult BeginAddStockDetail(string checkCode, int AccountID, StockDetail entity, bool IsCheck, ref int QtyCount, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginAddStockDetail(checkCode, AccountID, entity, IsCheck, ref QtyCount, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndAddStockDetail(ref int QtyCount, ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndAddStockDetail(ref QtyCount, ref ErrMsg, result);
        }

        public bool ModifyStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }
        public IAsyncResult BeginModifyStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginModifyStockDetail(checkCode, AccountID, entity, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndModifyStockDetail(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndModifyStockDetail(ref ErrMsg, result);
        }


        public bool UpdateStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 修改GlassID状态
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="strGlassID"></param>
        /// <param name="iStatus"></param>
        /// <param name="ErrMsg"></param>
        /// <param name="callback"></param>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        public IAsyncResult BeginUpdateStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginUpdateStockDetailStatus(checkCode, AccountID, strGlassID, strLotID, iStatus, ref ErrMsg, pCallback, asyncState);
        }

        public bool EndUpdateStockDetailStatus(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndUpdateStockDetailStatus(ref ErrMsg, result);
        }

        public StockDetail CheckStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginCheckStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginCheckStockDetailStatus(checkCode, AccountID, strGlassID, strLotID, iStatus, ref ErrMsg, pCallback, asyncState);
        }

        public StockDetail EndCheckStockDetailStatus(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndCheckStockDetailStatus(ref ErrMsg, result);
        }

        public StockDetail CheckStockDetailStatus_Out(string checkCode, int AccountID, string strGlassID, int[] strLotIDs, int iStatus, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginCheckStockDetailStatus_Out(string checkCode, int AccountID, string strGlassID, int[] strLotIDs, int iStatus, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            IncrementCallCount();
            var pCallback = new AsyncCallback((ar) =>
            {
                DecrementCallCount();
                callback(ar);
            });
            return _client.BeginCheckStockDetailStatus_Out(checkCode, AccountID, strGlassID, strLotIDs.ToArray(), iStatus, ref ErrMsg, pCallback, asyncState);
        }

        public StockDetail EndCheckStockDetailStatus_Out(ref string ErrMsg, IAsyncResult result)
        {
            return _client.EndCheckStockDetailStatus_Out(ref ErrMsg, result);
        }


        public StockDetail GetStockDetailByGlassID(string checkCode, int AccountID, string GlassID, ref string ErrMsg)
        {
            return _client.GetStockDetailByGlassID(checkCode, AccountID, GlassID, ref ErrMsg);
        }

        public IAsyncResult BeginGetStockDetailByGlassID(string checkCode, int AccountID, string GlassID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockDetail EndGetStockDetailByGlassID(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public bool CheckStockDetail_In(string checkCode, int AccountID, string strGlassID, int strLotID, ref string ErrMsg)
        {
            return _client.CheckStockDetail_In(checkCode, AccountID, strGlassID, strLotID, ref ErrMsg);
        }

        public IAsyncResult BeginCheckStockDetail_In(string checkCode, int AccountID, string strGlassID, int strLotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndCheckStockDetail_In(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public bool CheckStockDetail_In_Ex(string checkCode, int AccountID, string strGlassID, int strLotID, bool isCheck, ref string ErrMsg)
        {
            return _client.CheckStockDetail_In_Ex(checkCode, AccountID, strGlassID, strLotID, isCheck, ref ErrMsg);
        }

        public IAsyncResult BeginCheckStockDetail_In_Ex(string checkCode, int AccountID, string strGlassID, int strLotID, bool isCheck, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndCheckStockDetail_In_Ex(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool DeleteStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg)
        {
            return _client.DeleteStockDetail(checkCode, AccountID, entity, ref ErrMsg);
        }

        public IAsyncResult BeginDeleteStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndDeleteStockDetail(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        #endregion



        public bool DelStockDetailAndTuihuoCount(string checkCode, int AccountID, int StockDetailID, ref string ErrMsg)
        {
            return _client.DelStockDetailAndTuihuoCount(checkCode, AccountID, StockDetailID, ref ErrMsg);
        }

        public IAsyncResult BeginDelStockDetailAndTuihuoCount(string checkCode, int AccountID, int StockDetailID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndDelStockDetailAndTuihuoCount(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }




        public bool GetStockLotEntityByLotNoTotal(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg)
        {

            return _client.GetStockLotEntityByLotNoTotal(checkCode, AccountID, LotNo, iStatus, isShowAllDetail, ref  Qty, ref  OperaterQty, ref  FanGongQty, ref  HOLDQty, ref  ErrMsg);
        }

        public IAsyncResult BeginGetStockLotEntityByLotNoTotal(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndGetStockLotEntityByLotNoTotal(ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}
