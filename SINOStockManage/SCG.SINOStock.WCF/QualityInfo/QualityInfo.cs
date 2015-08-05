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
        public IList<QualityInfo> GetQualityInfoList(string checkCode, int AccountID, IDictionary<string, string> queryList, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                IQueryable<QualityInfo> tmplist = entities.QualityInfoes;
                foreach (var item in queryList)
                {
                    switch (item.Key)
                    {
                        case "Name":
                            tmplist = tmplist.Where(p => p.Name.Contains(item.Value));
                            break;
                        case "Type":
                            tmplist = tmplist.Where(p => p.InfoType == item.Value);
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

        public bool AddQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "品质信息为空！";
                return false;
            }

            if (string.IsNullOrWhiteSpace(entity.Name))
            {
                ErrMsg = "品质信息为空";
                return false;
            }

            //if (entities.QualityInfoes.Any(p => p.Name == entity.Name))
            //{
            //    ErrMsg = "当前品质名已在该工序下存在";
            //    return false;
            //}

            // entity.CreateDt = DateTime.Now;
            try
            {
                entities.QualityInfoes.Add(entity);
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

        public bool ModifyQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "品质信息为空";
                return false;
            }
            if (!entities.QualityInfoes.Any(p => p.ID == entity.ID))
            {
                ErrMsg = "该品质信息不存在或已删除";
                return false;
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrMsg = "品质信息不能为空";
                return false;
            }
            //   entity.ModifyDt = DateTime.Now;
            try
            {
                var entry = entities.Entry<QualityInfo>(entity);
                if (entry.State == EntityState.Detached)
                {
                    entities.Set<QualityInfo>().Attach(entity);
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

        public bool DelQualityInfo(string checkCode, int AccountID, QualityInfo entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "品质信息为空";
                return false;
            }
            QualityInfo _tmp = entities.QualityInfoes.FirstOrDefault(p => p.ID == entity.ID);
            if (_tmp == null)
            {
                ErrMsg = "该品质信息不存在或已删除";
                return false;
            }

            try
            {
                entities.QualityInfoes.Remove(_tmp);
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