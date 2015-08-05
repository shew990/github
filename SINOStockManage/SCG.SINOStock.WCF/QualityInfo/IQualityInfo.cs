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
        IList<QualityInfo> GetQualityInfoList(string checkCode, int AccountID, IDictionary<string, string> queryList, ref string ErrMsg);
        [OperationContract]
        bool AddQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg);
        [OperationContract]
        bool ModifyQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg);
        [OperationContract]
        bool DelQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg);
    }
}
