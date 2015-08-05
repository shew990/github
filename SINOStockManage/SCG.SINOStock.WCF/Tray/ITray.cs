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
        bool AddTray(string checkCode, int AccountID, Tray entity, List<int> StockLotIDs, bool isQiangDa, ref int Qty, ref List<string> Boxs, ref string ErrMsg);

        [OperationContract]
        Tray GetTrayByBarCode(string checkCode, int AccountID, string BarCode, ref int Qty, ref string ProModel, ref string ErrMsg);

        [OperationContract]
        string GetTrayMaxBarCode(string checkCode, int AccountID, int LogID, ref string ErrMsg);

        [OperationContract]
        List<Tray> GetTrayListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg);

        [OperationContract]
        Tray GetMaxTray(string checkCode, int AccountID, int LotID, ref string ErrMsg);
        [OperationContract]
        bool ModifyTray(string checkCode, int AccountID, Tray entity, int StockLotID, ref int Qty, ref string ErrMsg);

        [OperationContract]
        bool ChangeTryBarCode(string strBarCode, ref string ErrMsg);
    }

}
