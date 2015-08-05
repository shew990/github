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
        bool AddStockProDic(StockProDic entity, ref string ErrMsg);

        [OperationContract]
        StockProDic GetProDicByProAndLotID(int LotID, string ProModel);

    }
}
