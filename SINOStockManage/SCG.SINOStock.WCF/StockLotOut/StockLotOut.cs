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
        /// <param name="LotNo"></param>
        /// <param name="OutCount"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockLotOut GetStockLotOutEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref FormWork formWork, ref int OutCount, ref string ErrMsg)
        {
            try
            {
               
                StockLotOut slo = entities.StockLotOuts.FirstOrDefault(p => p.LotNo == LotNo);
                if (slo == null)
                {
                    StockLot sl = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.LotNo == LotNo);
                   // sl.StockDetails
                    if (sl == null)
                    {
                        ErrMsg = "不存在该LotNo";
                        return null;
                    }
                    if (sl.Status <= 0)
                    {
                        ErrMsg = "该LotNo入库还未完成，不能进行出库";
                        return null;
                    }
                    slo = new StockLotOut();
                    slo.LotNo = LotNo;
                    slo.PCSQty = sl.StockDetails.Count();
                    slo.ProModel = sl.ProModel;
                    slo.CreateDt = DateTime.Now;
                    entities.StockLotOuts.Add(slo);
                    entities.SaveChanges();
                    OutCount = 0;
                }
                else
                {
                    OutCount = entities.StockOutDetails.Count(p => p.StockLotOutID == slo.ID);
                }
                formWork = entities.FormWorks.FirstOrDefault(p => p.ProductModel == slo.ProModel);
                return slo;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        public StockLotOut GetStockLotOutEntityByLotNo_Out_NO(string checkCode, int AccountID, string LotNo, int Qty, string ProModel, ref FormWork formWork, ref int OutCount, ref string ErrMsg)
        {
            try
            {

                StockLotOut slo = entities.StockLotOuts.FirstOrDefault(p => p.LotNo == LotNo);
                if (slo == null)
                {
                    slo = new StockLotOut();
                    slo.LotNo = LotNo;
                    slo.PCSQty = Qty;
                    slo.ProModel =ProModel;
                    slo.CreateDt = DateTime.Now;
                    entities.StockLotOuts.Add(slo);
                    entities.SaveChanges();
                    OutCount = 0;
                }
                else
                {
                    OutCount = entities.StockOutDetails.Count(p => p.StockLotOutID == slo.ID);
                }
                formWork = entities.FormWorks.FirstOrDefault(p => p.ProductModel == slo.ProModel);
                return slo;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        public bool CheckLotNo(string checkCode, int AccountID, string LotNo, ref string ErrMsg)
        {
            try
            {

                StockLotOut slo = entities.StockLotOuts.FirstOrDefault(p => p.LotNo == LotNo);
                StockLot sl = entities.StockLots.FirstOrDefault(p => p.LotNo == LotNo);
                if (slo == null && sl == null)
                {
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

        public bool EndStockLotOut(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            //if (AccountID != 1)
            //{
            //    ErrMsg = "只有admin账户才能进行结束操作";
            //    return false;
            //}
            StockLotOut entity = entities.StockLotOuts.FirstOrDefault(p => p.ID == StockLotID);
            if (entity == null)
            {
                ErrMsg = "该LotNo不存在";
                return false;
            }
            if (entity.Status > 0)
            {
                ErrMsg = "该LotNo已结束，请勿重复操作";
                return false;
            }
            entity.Status = 1;
            try
            {
                entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteStockLotOut(string checkCode, int AccountID, int StockLotOutID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (AccountID != 1)
            {
                ErrMsg = "没有权限进行该操作，只有Admin账户才能进行删除操作";
                return false;
            }
            try
            {
                StockLotOut entity = entities.StockLotOuts.FirstOrDefault(p => p.ID == StockLotOutID);
                if (entity == null)
                {
                    ErrMsg = "该LotNo不存在或已删除";
                    return false;
                }

                //删除出库明细
                var DetailList = entities.StockOutDetails.Where(p => p.StockLotOutID == entity.ID);
                foreach (var item in DetailList)
                {
                    entities.StockOutDetails.Remove(item);
                }
                //删除箱号
                //todo:Box与LOT关系取消
                //var Boxlist = entities.StockBoxes.Where(p => p.StockLotOutID == entity.ID);
                //foreach (var item in Boxlist)
                //{
                //    entities.StockBoxes.Remove(item);
                //}

                //   entity.StockDetails.Clear();
                entities.StockLotOuts.Remove(entity);
                entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

       /// <summary>
       /// 出库结束该LotNO之前，取出已入库尚未出库的GLOASS数据
       /// </summary>
       /// <param name="checkCode"></param>
       /// <param name="AccountID"></param>
       /// <param name="LotNo"></param>
       /// <param name="ErrMsg"></param>
       /// <returns></returns>
        public List<string> GetNoStockOutGlass(string checkCode, int AccountID, string LotNo, ref string ErrMsg)
        {
            var stockLot = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.LotNo == LotNo);
            var stockOutLot = entities.StockLotOuts.Include("StockOutDetails").FirstOrDefault(p => p.LotNo == LotNo);
            List<string> list = new List<string>();
            if (stockLot != null && stockOutLot != null)//当两者都不为空的情况下 才进行比对
            {
                var details = stockLot.StockDetails;
                var outdetails = stockOutLot.StockOutDetails;
                foreach (var item in details)
                {
                    if (!outdetails.Any(p => p.GLassID == item.GlassID))
                    {
                        list.Add(item.GlassID);
                    }
                }
            }
            return list;
        }


        public List<StockLotOut> GetStockLotOutList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg)
        {
            try
            {
                IQueryable<StockLotOut> list = entities.StockLotOuts;
                foreach (var item in queryList)
                {
                    switch (item.Key)
                    {
                        case "StartDt":
                            DateTime dtStart = DateTime.Parse(item.Value).Date;
                            list = list.Where(p => p.CreateDt >= dtStart);
                            break;
                        case "EndDt":
                            DateTime dtEnd = DateTime.Parse(item.Value).Date;
                            list = list.Where(p => p.CreateDt <= dtEnd);
                            break;
                        default:
                            break;
                    }
                }
                listCount = list.Count();
                return list.OrderByDescending(p => p.CreateDt).Skip(PageCount * PageIndex).Take(PageCount).ToList();
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
    }
}