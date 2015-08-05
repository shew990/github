using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using SCG.SINOStock.Entities;

namespace SCG.SINOStock.WCF
{
    public partial interface ISINOStockService
    {
        /// <summary>
        /// 根据LotNO获取实体
        /// </summary>
        /// <param name="LotNo"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        StockLot GetStockLotEntityByLotNo(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg);
        [OperationContract]
        StockLot GetStockLotEntityByLotNo_Ex(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int HOLDQty, ref string ErrMsg);
        [OperationContract]
        List<StockLot> GetStockLotEntityListByLotNo(string checkCode, int AccountID, List<int> LotID, int iStatus, bool isShowAllDetail, ref string ErrMsg);
        /// <summary>
        /// 添加实体 成功返回true并带出ID
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="StockLotID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddStockLot(string checkCode, int AccountID, StockLot entity, ref int StockLotID, ref string ErrMsg);

        /// <summary>
        /// 结束StockLot  结束之后该LotNo不能进行入库
        /// </summary>
        /// <param name="StockLotID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        bool EndStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg);

        /// <summary>
        /// 删除StockLot  只有admin才有权操作。
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="StockLotID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg);


        /// <summary>
        /// 根据LotNO获取实体（出库专用），加这个方法，主要是为了在查询LotNo时带出该LotNo下面的出库总数
        /// </summary>
        /// <param name="LotNo"></param>
        /// <param name="OutCount">已出库数量</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        StockLot GetStockLotEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref int OutCount, ref string ErrMsg);
        [OperationContract]
        List<StockLot> GetStockLotList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg);
        [OperationContract]
        List<StockLot> GetStockLotList_Two(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg);

        /// <summary>
        /// /将该LOTNO下状态为HOLD的明细全部转移到新的LOTNO下，新的LOTNO号为就LOTNO号+“*”
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="StockLotID">需要转移的LOTNOID</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>返回是否成功，true：成功。false：失败</returns>
        [OperationContract]
        bool HOLDAllToNewStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg);

        /// <summary>
        /// 获取LOTNO集合中实际入库数and出库数
        /// </summary>
        /// <param name="StockLotIDs">LOTNO ID集合</param>
        /// <returns>结果集</returns>
        [OperationContract]
        List<StockOutQtyHelper> GetStockOutQtys(List<int> StockLotIDs);

        [OperationContract]
        bool GetStockLotEntityByLotNoTotal(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg);
    }
}
