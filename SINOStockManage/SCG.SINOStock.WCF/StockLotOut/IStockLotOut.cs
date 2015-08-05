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
        /// 根据LotNO获取实体（出库专用），加这个方法，主要是为了在查询LotNo时带出该LotNo下面的出库总数
        /// </summary>
        /// <param name="LotNo">LotNo</param>
        /// <param name="OutCount">已出库数量</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        StockLotOut GetStockLotOutEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref FormWork formWork, ref int OutCount, ref string ErrMsg);

        [OperationContract]
        StockLotOut GetStockLotOutEntityByLotNo_Out_NO(string checkCode, int AccountID, string LotNo,int Qty,string ProModel, ref FormWork formWork, ref int OutCount, ref string ErrMsg);

        [OperationContract]
        bool EndStockLotOut(string checkCode, int AccountID, int StockLotID, ref string ErrMsg);

        /// 删除StockLot  只有admin才有权操作。
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="StockLotID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteStockLotOut(string checkCode, int AccountID, int StockLotOutID, ref string ErrMsg);

        [OperationContract]
        bool CheckLotNo(string checkCode, int AccountID, string LotNo, ref string ErrMsg);

        /// <summary>
        /// 获取LOTNO下已入库但未出库的GLASSID 列表（仅对比）
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="LotNo"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetNoStockOutGlass(string checkCode, int AccountID, string LotNo, ref string ErrMsg);


        [OperationContract]
        List<StockLotOut> GetStockLotOutList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg);
    }
}
