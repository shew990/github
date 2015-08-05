using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SCG.SINOStock.Entities;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        /// <summary>
        /// 获取模板列表
        /// </summary>
        /// <param name="queryList">查询字典</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns></returns>
        public IList<FormWork> GetFormWorkList(string checkCode, int AccountID, IDictionary<string, string> queryList, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                IQueryable<FormWork> tmplist = entities.FormWorks;
                foreach (var item in queryList)
                {
                    switch (item.Key)
                    {
                        case "Product":
                            tmplist = tmplist.Where(p => p.ProductModel.Contains(item.Value));
                            break;
                        default:
                            break;
                    }
                }
                return tmplist.ToList();
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public bool AddFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "模板信息为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.ProductModel))
            {
                ErrMsg = "产品型号不能为空";
                return false;
            }
            if (entity.BoxPCSQty <= 0)
            {
                ErrMsg = "请填写每箱数量";
                return false;
            }
            if (entity.BoxQty <= 0)
            {
                ErrMsg = "请填写每托箱数";
                return false;
            }
            if (entity.IDNumber <= 0)
            {
                ErrMsg = "请填写ID位数";
                return false;
            }
            if (string.IsNullOrEmpty(entity.IDKeyWords))
            {
                ErrMsg = "关键字不能为空，请根据ID位数限制输入关键字";
                return false;
            }

            entity.CreateDt = DateTime.Now;
            try
            {
                if (entities.FormWorks.Any(p => p.ProductModel == entity.ProductModel))
                {
                    ErrMsg = "型号不能重复";
                    return false;
                }
                entities.FormWorks.Add(entity);
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

        public bool ModifyFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "模板信息为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.ProductModel))
            {
                ErrMsg = "产品型号不能为空";
                return false;
            }
            if (entity.BoxPCSQty <= 0)
            {
                ErrMsg = "请填写每箱数量";
                return false;
            }
            if (entity.BoxQty <= 0)
            {
                ErrMsg = "请填写每托箱数";
                return false;
            }
            if (entity.IDNumber <= 0)
            {
                ErrMsg = "请填写ID位数";
                return false;
            }
            if (string.IsNullOrEmpty(entity.IDKeyWords))
            {
                ErrMsg = "关键字不能为空，请根据ID位数限制输入关键字";
                return false;
            }
            entity.ModifyDt = DateTime.Now;
            try
            {
                var entry = entities.Entry<FormWork>(entity);
                if (entry.State == EntityState.Detached)
                {
                    entities.Set<FormWork>().Attach(entity);
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

        public bool DelFormWork(string checkCode, int AccountID, FormWork entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "模板信息为空";
                return false;
            }
            FormWork _tmp = entities.FormWorks.FirstOrDefault(p => p.ID == entity.ID);
            if (_tmp == null)
            {
                ErrMsg = "该模板不存在或已删除";
                return false;
            }

            try
            {
                entities.FormWorks.Remove(_tmp);
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
        /// <summary>
        /// 返回模板中所有型号，去重。
        /// </summary>
        /// <returns></returns>
        public List<string> GetProductStrList()
        {
   
            return entities.FormWorks.Select(p => p.ProductModel).Distinct().ToList();
        }

        public FormWork GetFormWorkByProModel(string checkCode, int AccountID, string proModel, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                FormWork formWork = entities.FormWorks.FirstOrDefault(p => p.ProductModel == proModel);
                if (formWork == null)
                    ErrMsg = "当前型号已被删除";
                return formWork;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
    }
}