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
        public IList<Role> GetRoleList(string checkCode, int RoleID, IDictionary<string, string> queryList, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, RoleID, ref ErrMsg);
                IQueryable<Role> tmplist = entities.Roles;
                foreach (var item in queryList)
                {
                    switch (item.Key)
                    {
                        case "Name":
                            tmplist = tmplist.Where(p => p.RuleName.Contains(item.Value));
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

        public bool AddRole(string checkCode, int RoleID, Role entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, RoleID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "角色信息为空！";
                return false;
            }

            if (string.IsNullOrWhiteSpace(entity.RuleName))
            {
                ErrMsg = "角色不能为空";
                return false;
            }
            if (entities.Roles.Any(p => p.RuleName == entity.RuleName))
            {
                ErrMsg = "当前角色名已存在";
                return false;
            }

            entity.CreateDt = DateTime.Now;
            try
            {
                entities.Roles.Add(entity);
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

        public bool ModifyRole(string checkCode, int RoleID, Role entity, ref string ErrMsg)
        {
            //if (!CheckingCheckCode(checkCode, RoleID, ref ErrMsg))
            //    return false;
            if (entity == null)
            {
                ErrMsg = "角色信息为空";
                return false;
            }
            if (!entities.Roles.Any(p => p.ID == entity.ID))
            {
                ErrMsg = "该角色不存在或已删除";
                return false;
            }
            if (string.IsNullOrEmpty(entity.RuleName))
            {
                ErrMsg = "角色名不能为空";
                return false;
            }
            entity.ModifyDt = DateTime.Now;
            try
            {
                var entry = entities.Entry<Role>(entity);
                if (entry.State == EntityState.Detached)
                {
                    entities.Set<Role>().Attach(entity);
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

        public bool DelRole(string checkCode, int RoleID, Role entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, RoleID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "角色信息为空";
                return false;
            }
            Role _tmp = entities.Roles.Include("Accounts").FirstOrDefault(p => p.ID == entity.ID);
            if (_tmp == null)
            {
                ErrMsg = "该角色不存在或已删除";
                return false;
            }
            if (_tmp.Accounts != null && _tmp.Accounts.Count() > 0)
            {
                ErrMsg = "已有用户分配了该角色，无法删除";
                return false;
            }
            try
            {
                entities.Roles.Remove(_tmp);
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
    }
}