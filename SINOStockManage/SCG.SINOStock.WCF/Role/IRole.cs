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
        IList<Role> GetRoleList(string checkCode, int RoleID, IDictionary<string, string> queryList, ref string ErrMsg);
        [OperationContract]
        bool AddRole(string checkCode, int RoleID, Role entity, ref string ErrMsg);
        [OperationContract]
        bool ModifyRole(string checkCode, int RoleID, Role entity, ref string ErrMsg);
        [OperationContract]
        bool DelRole(string checkCode, int RoleID, Role entity, ref string ErrMsg);
    }
}
