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
        /// 账号登录
        /// </summary>
        /// <param name="strNumber">编号</param>
        /// <param name="strPwd">密码</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns></returns>
        [OperationContract]
        Account Login(string strNumber, string strPwd, ref string ErrMsg);
        [OperationContract]
        Account Login_Ex(string strMACAddress, string strNumber, string strPwd, ref string ErrMsg);
        [OperationContract]
        IList<Account> GetAccountList(string checkCode, int AccountID, IDictionary<string, string> queryList, ref string ErrMsg);
        [OperationContract]
        bool AddAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg);
        [OperationContract]
        bool ModifyAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg);
        [OperationContract]
        bool DelAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg);
        [OperationContract]
        bool ChangePwd(string checkCode, int AccountID, string OldPwd, string NewPwd, ref string ErrMsg);
        [OperationContract]
        void ExitCurrentAccount(string checkCode, int AccountID);


    }
}
