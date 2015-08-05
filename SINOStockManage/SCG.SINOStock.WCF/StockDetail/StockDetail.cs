using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SCG.SINOStock.Entities;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StockLotID"></param>
        /// <param name="AccountID">根据操作人查询列表，如果AccountID为0，则查询全部</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public IList<StockDetail> GetStockDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg)
        {
            try
            {

                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                //DbSet<StockDetail> sets = entities.Set<StockDetail>();
                //var list= sets.AsQueryable().Where(p=>p.StockLotID==StockLotID).ToList();
                // var list = entities.StockDetails.Where(p => p.StockLotID == StockLotID).ToList();
                //var list = from n in entities.StockDetails where n.StockLotID == StockLotID select n;
                var list = entities.StockDetails.Include("StockLot").Where(p => p.StockLotID == StockLotID);
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
        /// <summary>
        /// 添加GlassID，入库时调用！！！其他流程请勿调用
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="QtyCount">添加成功之后，带出该LotNO下实际已入库数量</param>
        /// <param name="IsCheck">是否对比关键字</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool AddStockDetail(string checkCode, int AccountID, StockDetail entity, bool IsCheck, ref int QtyCount, ref string ErrMsg)
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
                //StockDetail tmpEntity=entities


                StockLot sl = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.ID == entity.StockLotID);

                if (sl == null)
                {
                    ErrMsg = "LotNo不存在或已删除";
                    return false;
                }
                //2014年2月23日 01:46:44  二期更改
                if (sl.IsImport)//当状态为未入库（导入模版)。
                {
                    StockDetail tmpEntity = sl.StockDetails.FirstOrDefault(p => p.GlassID == entity.GlassID);
                    tmpEntity.Status = entity.Status;
                    tmpEntity.StockInDT = entity.StockInDT;
                    tmpEntity.IsHOLD = entity.IsHOLD;
                    tmpEntity.StockInInfo = entity.StockInInfo;
                    tmpEntity.AccountID = entity.AccountID;
                    tmpEntity.AccountName = entity.AccountName;

                    return ModifyStockDetail(checkCode, AccountID, tmpEntity, ref ErrMsg);
                }

                if (entities.StockDetails.Any(p => p.GlassID == entity.GlassID))
                {
                    ErrMsg = "GlassID 不能重复";
                    return false;
                }

                if (sl.Status > 0)
                {
                    ErrMsg = "LotNo已结束，不能扫描GlassID";
                }
                if (sl.StockDetails != null && sl.PCSQty <= sl.StockDetails.Count())
                {
                    ErrMsg = "当前剩余入库数量为0";
                    return false;
                }
                FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == sl.ProModel);
                if (fw == null)
                {
                    ErrMsg = "该型号的模板不存在或已被删除，无法入库";
                    return false;
                }
                if (IsCheck)
                {
                    string[] strArray = fw.IDKeyWords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string tmpStr = strArray[strArray.Length - 1];
                    if (entity.GlassID.Length < int.Parse(tmpStr.Substring(0, tmpStr.Length - 1)))
                    {
                        ErrMsg = "与模板的关键字不匹配，不能入库";
                        return false;
                    }
                    for (int i = 0; i < entity.GlassID.Length; i++)
                    {
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            if ((i + 1) == int.Parse(strArray[j].Substring(0, strArray[j].Length - 1)))
                            {
                                if (entity.GlassID[i].ToString() != (strArray[j].Substring(strArray[j].Length - 1)))
                                {
                                    ErrMsg = "与模板的关键字不匹配，不能入库";
                                    return false;
                                }
                            }
                        }
                    }
                }

                entity.CreateDt = DateTime.Now;
                entities.StockDetails.Add(entity);
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "添加失败";
                    return false;
                }
                QtyCount = sl.StockDetails.Count();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool ModifyStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg)
        {
            //if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
            //    return false;
            if (entity == null)
            {
                ErrMsg = "Glass信息为空";
                return false;
            }
            if (entity.Status == 8 && entity.FanGongNum > 5)
            {
                ErrMsg = "返工次数已达到最大上限（5），无法再进行返工";
                return false;
            }
            if (entities.StockDetails.Any(p => p.GlassID == entity.GlassID&&p.ID!=entity.ID))
            {
                ErrMsg = "GlassID已存在，不能修改成该GlassID";
                return false;
            }
            try
            {
                var entry = entities.Entry<StockDetail>(entity);
                if (entry.State == EntityState.Detached)
                {
                    entities.Set<StockDetail>().Attach(entity);
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

        public bool DeleteStockDetail(string checkCode, int AccountID, StockDetail entity, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "Glass ID信息为空";
                return false;
            }
            StockDetail _tmp = entities.StockDetails.FirstOrDefault(p => p.ID == entity.ID);
            if (_tmp == null)
            {
                ErrMsg = "该Glass ID不存在或已删除";
                return false;
            }

            try
            {
                entities.StockDetails.Remove(_tmp);
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

        public bool UpdateStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (string.IsNullOrWhiteSpace(strGlassID))
            {
                ErrMsg = "请输入GlassID";
                return false;
            }
            try
            {

                StockDetail entity = entities.StockDetails.FirstOrDefault(p => p.GlassID == strGlassID);
                if (entity == null)
                {
                    ErrMsg = "不存在该GlassID";
                    return false;
                }
                //if (entity.Status < 1)
                //{
                //    ErrMsg = "该GlassID还未做入库";
                //    return false;
                //}
                //if (entity.Status == iStatus)
                //{
                //    ErrMsg = "该GlassID在该道工序中已扫描";
                //    return false;
                //}
                //if (entity.Status > iStatus)
                //{
                //    string strStatus = string.Empty;
                //    switch (entity.Status)
                //    {
                //        default:
                //            break;
                //    }
                //    ErrMsg = "该GlassID在该道工序中已操作，当前处于";
                //    return false;
                //}
                entity.Status = iStatus;
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

        /// <summary>
        /// 判断Detail状态 是否可以操作，并返回正确的GlassID
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="strGlassID"></param>
        /// <param name="strLotID"></param>
        /// <param name="iStatus"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockDetail CheckStockDetailStatus(string checkCode, int AccountID, string strGlassID, int strLotID, int iStatus, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            if (string.IsNullOrWhiteSpace(strGlassID))
            {
                ErrMsg = "请输入GlassID";
                return null;
            }
            try
            {

                StockDetail entity = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.GlassID == strGlassID);
                if (entity == null)
                {
                    ErrMsg = "不存在该GlassID";
                    return null;
                }
                if (entity.Status == -1)
                {
                    ErrMsg = "该GlassID已退货";
                    return null;
                }
                if (entity.IsHOLD)
                {
                    ErrMsg = "该GlassID属于HOLD状态，不能对该GlassID进行操作";
                    return null;
                }
                if (entity.StockLotID != strLotID && iStatus != 5)
                {
                    ErrMsg = "该GlassID属于<" + entity.StockLot.LotNo + ">中，不能在该LOTNo下操作";
                    return null;
                }

                #region 判断工序是否可以进行
                string strStatus = GetStrDetailStatus(entity.Status);
                string strTargetStatus = GetStrDetailStatus(iStatus);
                if (entity.Status >= iStatus && entity.Status != 8)//当GlassID状态大于需要修改的状态，并不为返工
                {
                    ErrMsg = "当前工序已操作(重复读取），该GlassID已处状态：" + strStatus;
                    return null;
                }

                switch (iStatus)
                {
                    case 2://进行减薄操作
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.Status == 8)
                        {
                            ErrMsg = "GlassID已重复扫描，当前状态为返工中";
                            return null;
                        }
                        break;
                    case 3://抛光

                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.Status != 2 && entity.Status != 8)
                        {
                            ErrMsg = "该GlassID尚未正常减薄";
                            return null;
                        }
                        //2014年4月2日 09:45:37修改  产品工序有减薄和抛光，从来料工段跳过减薄工段直接到抛光工段，提示为“此产品不需要抛光”。这种提示不明确，易误导。应提示“此产品还未正常减薄”
                        if (!entity.IsPaoGuang)
                        {
                            ErrMsg = "该GLassID不需要进行抛光";
                            return null;
                        }
                        break;
                    case 8:
                        if (entity.FanGongNum >= 5)
                        {
                            ErrMsg = "该GlassID返工次数已达到五次，无法再进行返工";
                            break;
                        }
                        if (!entity.IsPaoGuang)
                        {
                            ErrMsg = "该GLassID不需要进行抛光";
                            return null;
                        }
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.Status != 2 && entity.Status != 8)
                        {
                            ErrMsg = "该GlassID尚未进行减薄";
                            return null;
                        }
                        break;
                    case 4://进行镀膜操作
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.StockLot.IsJianBao && entity.Status < 2)
                        {
                            ErrMsg = "该GlassID尚未减薄";
                            return null;
                        }
                        if (entity.IsPaoGuang && entity.Status < 3)
                        {
                            ErrMsg = "该GlassID尚未抛光";
                            return null;
                        }
                        if (entity.Status == 8)
                        {
                            ErrMsg = "该GlassID状态处于抛光返工";
                            return null;
                        }
                        //if (entity.StockLot.IsJianBao && entity.Status != 2)//当该GlassID需要减薄操作，却为操作时
                        //{
                        //    ErrMsg = "当前GlassID需要进行减薄,请先减薄后在进行镀膜";
                        //    return null;
                        //}
                        break;
                    case 5://进行出库
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.Status != 4)//当不为镀膜后的状态时
                        {
                            if (entity.StockLot.IsJianBao && entity.Status < 2)
                            {
                                ErrMsg = "当前GlassID需要进行减薄,请先减薄后再进行出库";
                                return null;
                            }
                            if (entity.Status == 8)
                            {
                                ErrMsg = "该GlassID状态处于抛光返工";
                                return null;
                            }
                            if (entity.StockLot.IsDuMo)
                            {
                                ErrMsg = "当前GlassID需要进行镀膜,请先镀膜后再进行出库";
                                return null;
                            }
                        }
                        break;

                    default:
                        break;
                }
                #endregion
                return entity;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 判断Detail状态，（出库多批次专用）
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="strGlassID"></param>
        /// <param name="strLotID"></param>
        /// <param name="iStatus"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockDetail CheckStockDetailStatus_Out(string checkCode, int AccountID, string strGlassID, List<int> strLotIDs, int iStatus, ref string ErrMsg)
        {
            if (string.IsNullOrWhiteSpace(strGlassID))
            {
                ErrMsg = "请输入GlassID";
                return null;
            }
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                StockDetail entity = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.GlassID == strGlassID);
                if (entity == null)
                {
                    ErrMsg = "不存在该GlassID";
                    return null;
                }
                if (entity.Status == -1)
                {
                    ErrMsg = "该GlassID已退货";
                    return null;
                }
                if (entity.IsHOLD)
                {
                    ErrMsg = "该GlassID属于HOLD状态，不能对该GlassID进行操作";
                    return null;
                }
                //if (!strLotIDs.Any(p => p == entity.StockLotID))
                //{
                //    ErrMsg = "";
                //}
                if ((!strLotIDs.Any(p => p == entity.StockLotID)) && iStatus != 5)
                {
                    ErrMsg = "该GlassID属于<" + entity.StockLot.LotNo + ">中，不能在当前LOTNo多批次下出库";
                    return null;
                }

                #region 判断工序是否可以进行
                string strStatus = GetStrDetailStatus(entity.Status);
                string strTargetStatus = GetStrDetailStatus(iStatus);
                if (entity.Status >= iStatus && entity.Status != 8)//当GlassID状态大于需要修改的状态，并不为返工
                {
                    ErrMsg = "当前工序已操作(重复读取)，该GlassID已处状态：" + strStatus;
                    return null;
                }

                switch (iStatus)
                {
                    case 2://进行减薄操作
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        break;
                    case 3://抛光
                    case 8:
                        if (!entity.IsPaoGuang)
                        {
                            ErrMsg = "该GLassID不需要进行抛光";
                            return null;
                        }
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.Status != 2 && entity.Status != 8)
                        {
                            ErrMsg = "该GlassID尚未进行减薄";
                            return null;
                        }
                        break;
                    case 4://进行镀膜操作
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.StockLot.IsJianBao && entity.Status < 2)
                        {
                            ErrMsg = "该GlassID尚未减薄";
                            return null;
                        }
                        if (entity.IsPaoGuang && entity.Status < 3)
                        {
                            ErrMsg = "该GlassID尚未抛光";
                            return null;
                        }
                        if (entity.Status == 8)
                        {
                            ErrMsg = "该GlassID状态处于抛光返工";
                            return null;
                        }
                        //if (entity.StockLot.IsJianBao && entity.Status != 2)//当该GlassID需要减薄操作，却为操作时
                        //{
                        //    ErrMsg = "当前GlassID需要进行减薄,请先减薄后在进行镀膜";
                        //    return null;
                        //}
                        break;
                    case 5://进行出库
                        if (entity.Status < 1)
                        {
                            ErrMsg = "该GlassID尚未入库";
                            return null;
                        }
                        if (entity.Status != 4)//当不为镀膜后的状态时
                        {
                            if (entity.StockLot.IsJianBao && entity.Status < 2)
                            {
                                ErrMsg = "当前GlassID需要进行减薄,请先减薄后再进行出库";
                                return null;
                            }
                            if (entity.Status == 8)
                            {
                                ErrMsg = "该GlassID状态处于抛光返工";
                                return null;
                            }
                            if (entity.StockLot.IsDuMo)
                            {
                                ErrMsg = "当前GlassID需要进行镀膜,请先镀膜后再进行出库";
                                return null;
                            }
                        }
                        break;

                    default:
                        break;
                }
                #endregion
                return entity;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }


        private string GetStrDetailStatus(int iStatus)
        {
            string strResult = string.Empty;
            switch (iStatus)
            {
                case 0:
                    strResult = "未入库";
                    break;
                case 1:
                    strResult = "已入库";
                    break;
                case 2:
                    strResult = "减薄后";
                    break;
                case 3:
                    strResult = "抛光后";
                    break;
                case 4:
                    strResult = "镀膜后";
                    break;
                case 5:
                    strResult = "已出库";
                    break;
                case 8:
                    strResult = "返工中";
                    break;
                case -1:

                    strResult = "退货";
                    break;
                default:
                    break;
            }
            return strResult;
        }

        public StockDetail GetStockDetailByGlassID(string checkCode, int AccountID, string GlassID, ref string ErrMsg)
        {
            CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
            StockDetail entity = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.GlassID == GlassID);
            return entity;
        }

        public bool CheckStockDetail_In(string checkCode, int AccountID, string strGlassID, int strLotID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (string.IsNullOrWhiteSpace(strGlassID))
            {
                ErrMsg = "请输入GlassID";
                return false;
            }
            try
            {
                StockLot sl = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.ID == strLotID);
                if (sl == null)
                {
                    ErrMsg = "LOTNO不存在或已删除";
                    return false;
                }

                StockDetail entity = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.GlassID == strGlassID);
                if (sl.IsImport)//如果为导入模版
                {
                    if (entity == null)
                    {
                        ErrMsg = "LOTNO中，不存在该GlassID";
                        return false;
                    }
                }
                else
                {
                    if (sl.StockDetails != null && sl.StockDetails.Count() >= sl.PCSQty)
                    {
                        ErrMsg = "入库数量已满";
                        return false;
                    }



                    string strProModel = sl.ProModel;
                    FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == strProModel);
                    if (fw != null)
                    {
                        if (strGlassID.Length > fw.IDNumber)
                        {
                            ErrMsg = "GLASSID位数不能大于所属型号的ID位数";
                            return false;
                        }
                    }
                }
                if (entity != null)
                {
                    if (entity.Status >= 1 || entity.Status == -1)
                    {
                        ErrMsg = "GlassID已重复扫描，当前状态为" + GetStrDetailStatus(entity.Status);
                        return false;
                    }
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
        /// 2014年4月2日 13:34:24 无对比入库和有对比入库验证方面有区别 主要是模版方面
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="strGlassID"></param>
        /// <param name="strLotID"></param>
        /// <param name="isCheck">是否需要验证（ture:有对比;false:无对比</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool CheckStockDetail_In_Ex(string checkCode, int AccountID, string strGlassID, int strLotID, bool isCheck, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (string.IsNullOrWhiteSpace(strGlassID))
            {
                ErrMsg = "请输入GlassID";
                return false;
            }
            try
            {
                StockLot sl = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.ID == strLotID);
                if (sl == null)
                {
                    ErrMsg = "LOTNO不存在或已删除";
                    return false;
                }
                if (sl.Status == 1)
                {
                    ErrMsg = "该LotNO入库已结束，不能对该LotNO进行入库扫描";
                    return false;
                }
                StockDetail entity = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.GlassID == strGlassID);
                if (entity != null && entity.StockLotID != strLotID)//&& iStatus != 5
                {
                    ErrMsg = "该GlassID属于<" + entity.StockLot.LotNo + ">中，不能在该LOTNo下操作";
                    return false;
                }
                if (sl.IsImport)//如果为导入模版
                {
                    if (entity == null)
                    {
                        ErrMsg = "LOTNO中，不存在该GlassID";
                        return false;
                    }
                }
                else
                {
                    if (sl.StockDetails != null && sl.StockDetails.Count() >= sl.PCSQty)
                    {
                        ErrMsg = "入库数量已满";
                        return false;
                    }



                    string strProModel = sl.ProModel;
                    FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == strProModel);
                    if (fw != null && isCheck)
                    {
                        if (strGlassID.Length != fw.IDNumber)
                        {
                            ErrMsg = "GLASSID位数不等于所属型号的ID位数.(" + fw.IDNumber + ")";
                            return false;
                        }


                        string[] strArray = fw.IDKeyWords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string tmpStr = strArray[strArray.Length - 1];
                        if (strGlassID.Length < int.Parse(tmpStr.Substring(0, tmpStr.Length - 1)))
                        {
                            ErrMsg = "与模板的关键字不匹配，不能入库";
                            return false;
                        }
                        for (int i = 0; i < strGlassID.Length; i++)
                        {
                            for (int j = 0; j < strArray.Length; j++)
                            {
                                if ((i + 1) == int.Parse(strArray[j].Substring(0, strArray[j].Length - 1)))
                                {
                                    if (strGlassID[i].ToString() != (strArray[j].Substring(strArray[j].Length - 1)))
                                    {
                                        ErrMsg = "与模板的关键字不匹配，不能入库";
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (entity != null)
                {
                    if (entity.Status >= 1 || entity.Status == -1)
                    {
                        ErrMsg = "GlassID已重复扫描，当前状态为" + GetStrDetailStatus(entity.Status);
                        return false;
                    }
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
        /// 退货功能（删除GlassID数据，并在主表累加退货数）  2014年5月4日 11:12:49
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="StockDetailID">GlassID ID</param>
        /// <param name="ErrMsg">返回错误信息</param>
        /// <returns></returns>
        public bool DelStockDetailAndTuihuoCount(string checkCode, int AccountID, int StockDetailID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (StockDetailID <= 0)
            {
                ErrMsg = "Glass ID信息为空";
                return false;
            }
            StockDetail _tmp = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.ID == StockDetailID);
            if (_tmp == null)
            {
                ErrMsg = "该Glass ID不存在或已删除";
                return false;
            }

            try
            {
                _tmp.StockLot.TuiHuoCount += 1;//退货数+1
                entities.StockDetails.Remove(_tmp);

                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "退货失败";
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