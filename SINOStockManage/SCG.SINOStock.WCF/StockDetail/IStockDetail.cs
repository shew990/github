using SCG.SINOStock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;


namespace SCG.SINOStock.WCF
{
    public partial interface ISINOStockService
    {
        [OperationContract]
        IList<StockDetail> GetStockDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg);
        [OperationContract]
        bool AddStockDetail(string checkCode, int AccountID, StockDetail entity, bool IsCheck, ref int QtyCount, ref string ErrMsg);

        [OperationContract]
        bool ModifyStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg);

        [OperationContract]
        bool DeleteStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg);

        [OperationContract]
        bool UpdateStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg);

        [OperationContract]
        StockDetail CheckStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg);

        [OperationContract]
        StockDetail CheckStockDetailStatus_Out(string checkCode, int AccountID, string strGlassID, List<int> strLotIDs, int iStatus, ref string ErrMsg);


        [OperationContract]
        StockDetail GetStockDetailByGlassID(string checkCode, int AccountID, string GlassID, ref string ErrMsg);

        [OperationContract]
        bool CheckStockDetail_In(string checkCode, int AccountID, string strGlassID, int strLotID, ref string ErrMsg);

        [OperationContract]
        bool CheckStockDetail_In_Ex(string checkCode, int AccountID, string strGlassID, int strLotID, bool isCheck, ref string ErrMsg);

        [OperationContract]
        bool DelStockDetailAndTuihuoCount(string checkCode, int AccountID, int StockDetailID, ref string ErrMsg);
    }
}
