using SCG.SINOStock.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNumber"></param>
        /// <param name="strPwd"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public Account Login(string strNumber, string strPwd, ref string ErrMsg)
        {
            if (string.IsNullOrEmpty(strNumber))
            {
                ErrMsg = "编号不能为空";
                return null;
            }
            if (string.IsNullOrEmpty(strPwd))
            {
                ErrMsg = "密码不能为空";
                return null;
            }
            try
            {
                Account _tmp = entities.Accounts.Include("Role").FirstOrDefault(p => p.LoginNumber == strNumber && p.LoginPwd == strPwd);
                if (_tmp == null)
                {
                    ErrMsg = "编号或密码错误";
                    return null;
                }
                _tmp.CheckCode = Guid.NewGuid().ToString();
                entities.SaveChanges();
                return _tmp;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public Account Login_Ex(string strMACAddress, string strNumber, string strPwd, ref string ErrMsg)
        {
            if (string.IsNullOrEmpty(strNumber))
            {
                ErrMsg = "编号不能为空";
                return null;
            }
            if (string.IsNullOrEmpty(strPwd))
            {
                ErrMsg = "密码不能为空";
                return null;
            }
            try
            {
                Account _tmp = entities.Accounts.Include("Role").FirstOrDefault(p => p.LoginNumber == strNumber && p.LoginPwd == strPwd);
                if (_tmp == null)
                {
                    ErrMsg = "编号或密码错误";
                    return null;
                }
                DateTime dtNow = DateTime.Now;//当前时间
                if (!string.IsNullOrWhiteSpace(_tmp.CheckCode))
                {
                    if (_tmp.CheckCode != strMACAddress && _tmp.LoginDt != null && _tmp.LoginDt.Value.AddMinutes(30) > dtNow)
                    {
                        ErrMsg = "当前账户已在另一地点登录";
                        return null;
                    }
                }
                _tmp.CheckCode = strMACAddress;
                _tmp.LoginDt = dtNow;
                entities.SaveChanges();
                return _tmp;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }




        /// <summary>
        /// 查询账户信息列表
        /// </summary>
        /// <param name="queryList">查询条件</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns></returns>
        public IList<Account> GetAccountList(string checkCode, int AccountID, IDictionary<string, string> queryList, ref string ErrMsg)
        {

            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);


                var tmplist = entities.Accounts.Include("Role").Where(p=>1==1);//.Where(p => p.ID != 100);//查询用户信息列表  排除ID唯一的数据(admin)
                foreach (var item in queryList)
                {
                    switch (item.Key)
                    {
                        case "Name":
                            tmplist = tmplist.Where(p => p.Name.Contains(item.Value));
                            break;
                        case "Number":
                            tmplist = tmplist.Where(p => p.LoginNumber.Contains(item.Value));
                            break;
                        default:
                            break;
                    }
                }
                //System.Threading.Thread.Sleep(5000);
                return tmplist.ToList();
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 添加账户信息
        /// </summary>
        /// <param name="entity">需要添加的实体</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>添加成功true，失败返回false并返回错误信息</returns>
        public bool AddAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "账户信息为空";
                return false;
            }
         
            if (string.IsNullOrEmpty(entity.LoginNumber))
            {
                ErrMsg = "编号不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrMsg = "用户姓名不能为空";
                return false;
            }
            entity.LoginPwd = "111111";
            entity.CreateDt = DateTime.Now;
            try
            {
                if (entities.Accounts.Any(p => p.LoginNumber == entity.LoginNumber))
                {
                    ErrMsg = "编号已存在";
                    return false;
                }
                entities.Accounts.Add(entity);
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "添加失败";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool ModifyAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg)
        {
            //if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
            //    return false;
            if (entity == null)
            {
                ErrMsg = "账户信息为空";
                return false;
            }
            if (!entities.Accounts.Any(p => p.ID == entity.ID))
            {
                ErrMsg = "该账户不存在或已删除";
                return false;
            }
            if (entity.LoginNumber.Equals("admin"))
            {
                ErrMsg = "admin账户不允许修改";
                return false;
            }

            if (string.IsNullOrEmpty(entity.LoginNumber))
            {
                ErrMsg = "编号不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrMsg = "用户姓名不能为空";
                return false;
            }
            entity.ModifyDt = DateTime.Now;
            entity.Role = entities.Roles.FirstOrDefault(p => p.ID == entity.RoleID);
            try
            {
                var entry = entities.Entry<Account>(entity);
                if (entry.State == EntityState.Detached)
                {
                    entities.Set<Account>().Attach(entity);
                    entry.State = EntityState.Modified;
                }
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "修改失败";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool DelAccount(string checkCode, int AccountID, Account entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "账户信息为空";
                return false;
            }
            Account _tmp = entities.Accounts.FirstOrDefault(p => p.ID == entity.ID);
            if (_tmp == null)
            {
                ErrMsg = "该账户不存在或已删除";
                return false;
            }

            try
            {
                entities.Accounts.Remove(_tmp);
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "删除失败";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }

        }

        public bool ChangePwd(string checkCode, int AccountID, string OldPwd, string NewPwd, ref string ErrMsg)
        {
            try
            {
                if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                    return false;
                Account entity = entities.Accounts.FirstOrDefault(p => p.ID == AccountID);
                if (entity == null)
                {
                    ErrMsg = "该用户不存在或已删除";
                    return false;
                }
                if (entity.LoginPwd != OldPwd)
                {
                    ErrMsg = "旧密码不正确";
                    return false;
                }
                //if (OldPwd == NewPwd)
                //{
                //    ErrMsg = "新密码与旧密码相同";
                //    return false;
                //}
                entity.LoginPwd = NewPwd;
                entities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                ErrMsg = e.Message;
                return false;
            }
        }

        public void ExitCurrentAccount(string checkCode, int AccountID)
        {

            Account _tmp = entities.Accounts.FirstOrDefault(p => p.ID == AccountID);
            if (_tmp != null && _tmp.CheckCode == checkCode)
            {
                _tmp.CheckCode = "";//清空验证码 
            }

            try
            {
                entities.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }
    }
}