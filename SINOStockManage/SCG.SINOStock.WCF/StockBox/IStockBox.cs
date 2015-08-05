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

        [OperationContract]
        bool AddStockBox(string checkCode, int AccountID, StockBox entity, List<int> sdoIDList, ref bool IsPrintTray, ref string ErrMsg);
        [OperationContract]
        StockBox GetStockBoxToBarCode(string checkCode, int AccountID, string BarCode, ref string ErrMsg);
        [OperationContract]
        string GetMaxBarCode(string checkCode, int AccountID,int LotID, ref string ErrMsg);
        [OperationContract]
        List<StockBox> GetBoxListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg);

        [OperationContract]
        StockBox GetMaxStockBox(string checkCode, int AccountID, int LotID, ref string ErrMsg);
        [OperationContract]
        bool ModifyStockBox(string checkCode, int AccountID, StockBox entity, List<int> sdoIDList, List<int> stockLotIDs, ref bool IsPrintTray, ref string ErrMsg);

        [OperationContract]
        string GetMaxBarCode_Ex(string checkCode, int AccountID, ref string ErrMsg);

        [OperationContract]
        StockBox GetMaxStockBox_Ex(string checkCode, int AccountID, ref string ErrMsg);

        [OperationContract]
        StockBox ChangeBoxBarCode(string strBarCode, ref string ErrMsg);

        
        /// <summary>
        /// 补打时：修改外箱标签
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="entity"></param>
        /// <param name="sdoIDList"></param>
        /// <param name="stockLotIDs"></param>
        /// <param name="IsPrintTray"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        bool ModifyBoxBarCode(string checkCode, int AccountID, string OldBarCode, string NewBarCode, ref string ErrMsg);

        /// <summary>
        /// 查出当前登录用户最近一个未打印的箱号实体
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        [OperationContract]
        StockBox GetStocBoxkEntityByIsUnPrint(string checkCode, int AccountID, ref string ErrMsg);
    }
}
