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
        IList<FormWork> GetFormWorkList(string checkCode, int AccountID, IDictionary<string, string> queryList, ref string ErrMsg);
        [OperationContract]
        bool AddFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg);
        [OperationContract]
        bool ModifyFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg);
        [OperationContract]
        bool DelFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg);
        [OperationContract]
        List<string> GetProductStrList();

        [OperationContract]
        FormWork GetFormWorkByProModel(string checkCode, int AccountID, string proModel, ref string ErrMsg);
    }
}
