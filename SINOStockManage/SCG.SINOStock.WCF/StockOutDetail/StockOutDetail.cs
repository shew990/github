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
        public IList<StockOutDetail> GetStockOutDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg)
        {
            try
            {
                var list = entities.StockOutDetails.Include("StockLotOut").Include("StockBox").Include("StockBox.Tray").Where(p => p.StockLotOutID == StockLotID);
                CountQty = list.Count();
                if (AccountID > 0)
                    list = list.Where(p => p.AccountID == AccountID);
                return list.ToList();
                //return list;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public bool AddStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, bool IsCheck, ref int QtyCount, ref StockOutDetail entityID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
           if (entity == null)
            {
                ErrMsg = "数据为空";
                return false;
            }   
            
            try
            {
                if (entities.StockOutDetails.Any(p => p.GLassID == entity.GLassID))
                {
                    ErrMsg = "GlassID 不能重复";
                    return false;
                }

                StockLotOut sl = entities.StockLotOuts.FirstOrDefault(p => p.ID == entity.StockLotOutID);
               // StockLot sl = entities.StockLots.FirstOrDefault(p => p.ID == entity.StockLotOutID);
               
                if (sl == null)
                {
                    ErrMsg = "LotNo不存在或已删除";
                    return false;
                }
                if (sl.Status>=1)
                {
                    ErrMsg = "该LotNo出库已结束，不能扫描GlassID进行出库";
                    return false;
                }
                //if(sl.Status>=3)
                //{
                //    ErrMsg="该LotNo出库已结束，不能扫描GlassID进行出库";
                //    return false;
                //}
                int iOutCount = entities.StockOutDetails.Count(p => p.StockLotOutID == sl.ID);
                if (  sl.PCSQty <= iOutCount)
                {
                    ErrMsg = "当前剩余出库数量为0";
                    return false;
                }
                FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == sl.ProModel);
                if (fw == null)
                {
                    ErrMsg = "该型号的模板不存在或已被删除，无法出库";
                    return false;
                }
                if (IsCheck)
                {
                    #region 2013年8月8日 22:23:19 验证是否已出库
                    StockLot stockLot = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.LotNo == sl.LotNo);
                    if (stockLot == null)
                    {
                        ErrMsg = "该LotNo的入库数据不存在或已被删除，无法进行出库扫描";
                        return false;
                    }
                    if (!stockLot.StockDetails.Any(p => p.GlassID == entity.GLassID))
                    {
                        ErrMsg = "当前LotNo下不存在该GlassID的入库记录，无法出库";
                        return false;
                    }
                    #endregion
                    string[] strArray = fw.IDKeyWords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string tmpStr = strArray[strArray.Length - 1];
                    if (entity.GLassID.Length < int.Parse(tmpStr.Substring(0, tmpStr.Length - 1)))
                    {
                        ErrMsg = "与模板的关键字不匹配，不能出库";
                        return false;
                    }
                    for (int i = 0; i < entity.GLassID.Length; i++)
                    {
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            if ((i + 1) == int.Parse(strArray[j].Substring(0, strArray[j].Length - 1)))
                            {
                                if (entity.GLassID[i].ToString() != (strArray[j].Substring(strArray[j].Length - 1)))
                                {
                                    ErrMsg = "与模板的关键字不匹配，不能出库";
                                    return false;
                                }
                            }
                        }
                    }
                }

                entity.CreateDt = DateTime.Now;
                entities.StockOutDetails.Add(entity);
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "添加失败";
                    return false;
                }
                QtyCount = iOutCount + 1;
                entityID = entity;
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public StockOutDetail GetStockoutDetailByGlassID(string checkCode, int AccountID, string strGlassID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            StockOutDetail entity = entities.StockOutDetails.Include("StockBox").FirstOrDefault(p => p.GLassID == strGlassID);
            if (entity == null)
            {
                ErrMsg = "该GlassID不存在";
                return null;
            }
            return entity;
        }

        public bool ModifyStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "实体不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.GLassID))
            {
                ErrMsg = "Glass ID不能为空！";
                return false;
            }
            if (entities.StockOutDetails.Any(p => p.GLassID == entity.GLassID))//只能用于GLASSID替换，如果修改其他 不修改GLASSID  这地方会有问题
            {
                ErrMsg = "新的Glass ID已存在";
                return false;
            }
            try
            {
                var entry = entities.Entry<StockOutDetail>(entity);
                if (entry.State == EntityState.Detached)
                {
                    entities.Set<StockOutDetail>().Attach(entity);
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

        public bool DeleteStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "Glass ID信息为空";
                return false;
            }
            StockOutDetail _tmp = entities.StockOutDetails.FirstOrDefault(p => p.ID == entity.ID);
            if (_tmp == null)
            {
                ErrMsg = "该Glass ID不存在或已删除";
                return false;
            }

            try
            {
                entities.StockOutDetails.Remove(_tmp);
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