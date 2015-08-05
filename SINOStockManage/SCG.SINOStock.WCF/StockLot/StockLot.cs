using SCG.SINOStock.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="LotNo"></param>
        /// <param name="iStatus"></param>
        /// <param name="isShowAllDetail"></param>
        /// <param name="Qty">带出已入库数</param>
        /// <param name="OperaterQty">带出实际操作数</param>
        /// <param name="FanGongQty">返工数</param>
        /// <param name="HOLDQty">HOLD数</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockLot GetStockLotEntityByLotNo(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            var entity = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockDetails.StockBox.Tray").FirstOrDefault(p => p.LotNo == LotNo);
            //var entity = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockBox.Tray").FirstOrDefault(p => p.LotNo == LotNo);
            
            if (iStatus == 99)//查询GlassID专用
            {
                return entity;
            }
            if (entity == null)
            {
                ErrMsg = "当前不存在该LotNo";
                return null;
            }

            Qty = entity.StockDetails.Count(p => p.Status != 0);
            HOLDQty = entity.StockDetails.Count(p => p.IsHOLD);
            switch (iStatus)
            {
                case 2://减薄 
                    if (!entity.IsJianBao)
                    {
                        ErrMsg = "当前LotNo没有减薄工序";
                        return null;
                    }
                    break;
                case 4://镀膜
                    if (!entity.IsDuMo)
                    {
                        ErrMsg = "当前LotNo没有镀膜工序";
                        return null;
                    }
                    break;
                default:
                    break;
            }
            if (iStatus == 3)//如果为抛光
            {
                OperaterQty = entity.StockDetails.Where(p => p.Status >= iStatus && p.Status != 8).Count();
                FanGongQty = entity.StockDetails.Where(p => p.Status == 8).Count();
                entity.StockDetails = entity.StockDetails.Where(p => p.Status == iStatus || p.Status == 8).ToList();
            }
            else
            {
                //   if (iStatus == 2)//如果为减薄
                OperaterQty = entity.StockDetails.Where(p => p.Status >= iStatus).Count();

                entity.StockDetails = entity.StockDetails.Where(p => p.Status == iStatus).ToList();
            }
            if (!isShowAllDetail)
            {
                switch (iStatus)
                {
                    case 1://入库
                        entity.StockDetails = entity.StockDetails.Where(p => p.AccountID == AccountID).ToList();
                        break;
                    case 2://减薄
                        entity.StockDetails = entity.StockDetails.Where(p => p.JianBaoAccountID == AccountID).OrderByDescending(p => p.JianBaoDT).ToList();
                        break;
                    case 3://抛光
                        entity.StockDetails = entity.StockDetails.Where(p => p.PaoGuangAccountID == AccountID).OrderByDescending(p => p.PaoGuangDT).ToList();
                        break;
                    case 4://镀膜
                        entity.StockDetails = entity.StockDetails.Where(p => p.DuMoAccountID == AccountID).OrderByDescending(p => p.DuMoDT).ToList();
                        break;
                    default:
                        break;
                }

            }
            return entity;
        }
        /// <summary>
        /// 带出已入库数
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="LotNo"></param>
        /// <param name="iStatus"></param>
        /// <param name="isShowAllDetail"></param>
        /// <param name="Qty"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockLot GetStockLotEntityByLotNo_Ex(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int HOLDQty, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            var entity = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockDetails.StockBox.Tray").FirstOrDefault(p => p.LotNo == LotNo);
            if (iStatus == 99)//查询GlassID专用
            {
                return entity;
            }
            if (entity == null)
            {
                ErrMsg = "当前不存在该LotNo";
                return null;
            }
            Qty = entity.StockDetails.Count(p => p.Status != 0);
            HOLDQty = entity.StockDetails.Count(p => p.IsHOLD);
            switch (iStatus)
            {
                case 2://减薄 
                    if (!entity.IsJianBao)
                    {
                        ErrMsg = "当前LotNo没有减薄工序";
                        return null;
                    }
                    break;
                case 4://镀膜
                    if (!entity.IsDuMo)
                    {
                        ErrMsg = "当前LotNo没有镀膜工序";
                        return null;
                    }
                    break;
                default:
                    break;
            }
            if (iStatus == 3)
                entity.StockDetails = entity.StockDetails.Where(p => p.Status == iStatus || p.Status == 8).ToList();
            else
                entity.StockDetails = entity.StockDetails.Where(p => p.Status == iStatus).ToList();
            if (!isShowAllDetail)
            {
                switch (iStatus)
                {
                    case 1://入库
                        entity.StockDetails = entity.StockDetails.Where(p => p.AccountID == AccountID).ToList();
                        break;
                    case 2://减薄
                        entity.StockDetails = entity.StockDetails.Where(p => p.JianBaoAccountID == AccountID).OrderByDescending(p => p.JianBaoDT).ToList();
                        break;
                    case 3://抛光
                        entity.StockDetails = entity.StockDetails.Where(p => p.PaoGuangAccountID == AccountID).OrderByDescending(p => p.PaoGuangDT).ToList();
                        break;
                    case 4://镀膜
                        entity.StockDetails = entity.StockDetails.Where(p => p.DuMoAccountID == AccountID).OrderByDescending(p => p.DuMoDT).ToList();
                        break;
                    default:
                        break;
                }

            }
            return entity;
        }

        /// <summary>
        /// 查询StockLot集合（出库刷新专用2014年3月9日 23:28:02)
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="LotID"></param>
        /// <param name="iStatus"></param>
        /// <param name="isShowAllDetail"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public List<StockLot> GetStockLotEntityListByLotNo(string checkCode, int AccountID, List<int> LotID, int iStatus, bool isShowAllDetail, ref string ErrMsg)
        {
            CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
            List<StockLot> results = new List<StockLot>();
            foreach (int item in LotID)
            {
                var entity = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockDetails.StockBox.Tray").FirstOrDefault(p => p.ID == item);
                if (entity != null)
                {
                    if (!isShowAllDetail)
                        entity.StockDetails = entity.StockDetails.Where(p => p.DuMoAccountID == AccountID).ToList();
                    else
                    {
                        entity.StockDetails = entity.StockDetails.Where(p => p.DuMoAccountID != null).ToList();
                    }

                    results.Add(entity);
                }
            }
            return results;
        }
        public bool AddStockLot(string checkCode, int AccountID, StockLot entity, ref int StockLotID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "数据为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.LotNo))
            {
                ErrMsg = "Lot No不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.ProModel))
            {
                ErrMsg = "请选择产品型号";
                return false;
            }
            if (entity.PCSQty <= 0)
            {
                ErrMsg = "请输入数量";
                return false;
            }
            if (entities.StockLots.Any(p => p.LotNo == entity.LotNo))
            {
                ErrMsg = "LotNo已存在";
                return false;
            }
            //  entities.StockDetails
            // entity.StockDetails
            if (entity.IsImport)
            {
                foreach (var item in entity.StockDetails)
                {
                    if (entities.StockDetails.Any(p => p.GlassID.Equals(item.GlassID)))
                    {
                        ErrMsg = "GlassID不能重复";
                        return false;
                    }
                    #region 2014年6月25日 15:10:45  对比型号
                    FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == entity.ProModel);
                    if (fw == null)
                    {
                        ErrMsg = "该型号的模板不存在或已被删除，无法入库";
                        return false;
                    }

                    string[] strArray = fw.IDKeyWords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string tmpStr = strArray[strArray.Length - 1];
                    if (item.GlassID.Length < int.Parse(tmpStr.Substring(0, tmpStr.Length - 1)))
                    {
                        ErrMsg = "导入列表中有数据与模板的关键字不匹配";
                        return false;
                    }
                    for (int i = 0; i < item.GlassID.Length; i++)
                    {
                        for (int j = 0; j < strArray.Length; j++)
                        {
                            if ((i + 1) == int.Parse(strArray[j].Substring(0, strArray[j].Length - 1)))
                            {
                                if (item.GlassID[i].ToString() != (strArray[j].Substring(strArray[j].Length - 1)))
                                {
                                    ErrMsg = "导入列表中有数据与模板的关键字不匹配";
                                    return false;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            entity.CreateDt = DateTime.Now;
            try
            {
                entities.StockLots.Add(entity);
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "添加失败";
                    return false;
                }
                StockLotID = entity.ID;
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool EndStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            //if (AccountID != 1)
            //{
            //    ErrMsg = "只有admin账户才能进行结束操作";
            //    return false;
            //}
            StockLot entity = entities.StockLots.FirstOrDefault(p => p.ID == StockLotID);
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
        public bool DeleteStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;

            try
            {
                StockLot entity = entities.StockLots.FirstOrDefault(p => p.ID == StockLotID);
                if (entity == null)
                {
                    ErrMsg = "该LotNo不存在或已删除";
                    return false;
                }

                var DetailList = entities.StockDetails.Where(p => p.StockLotID == entity.ID);
                foreach (var item in DetailList)
                {
                    entities.StockDetails.Remove(item);
                }
                //   entity.StockDetails.Clear();
                entities.StockLots.Remove(entity);
                entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }

        }
        public StockLot GetStockLotEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref int OutCount, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            ErrMsg = "方法未实现";
            return null;
            //var entity = entities.StockLots.FirstOrDefault(p => p.LotNo == LotNo);
            //// var list=entities.stoc
            //if (entity != null)
            //    OutCount = entities.StockOutDetails.Count(p => p.StockLotID == entity.ID);
            //return entity;
        }
        public List<StockLot> GetStockLotList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                IQueryable<StockLot> list = entities.StockLots;
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

        public List<StockLot> GetStockLotList_Two(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                IQueryable<StockLot> list = entities.StockLots;
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
                        case "GlassID":
                            StockDetail entity = entities.StockDetails.Include("StockLot").FirstOrDefault(p => p.GlassID == item.Value);
                            if (entity == null)
                            {
                                ErrMsg = "GlassID不存在";
                                return null;
                            }
                            List<StockLot> lst = new List<StockLot>();
                            lst.Add(entity.StockLot);
                            return lst;
                        //  break;
                        default:
                            break;
                    }
                }
                return list.ToList();
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// /将该LOTNO下状态为HOLD的明细全部转移到新的LOTNO下，新的LOTNO号为就LOTNO号+“*”
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="StockLotID">需要转移的LOTNOID</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>返回是否成功，true：成功。false：失败</returns>
        public bool HOLDAllToNewStockLot(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            //  if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))//验证是否有权限操作
            //      return false;
            try
            {
                StockLot entity = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.ID == StockLotID);
                if (entity == null)
                {
                    ErrMsg = "当前LOTNO不存在或已删除";
                    return false;
                }
                if (!entity.StockDetails.Any(p => p.IsHOLD))
                {
                    ErrMsg = "当前LOTNO下没有状态为HOLD的GlassID";
                    return false;
                }
                if (!entity.StockDetails.Any(p => !p.IsHOLD))
                {
                    ErrMsg = "当前LOTNO下的GlassID全为HOLD，无需转为新的LOTNO";
                    return false;
                }
                //将旧Stocklot下的信息全部复制到新的Stocklot
                StockLot tmp = new StockLot();
                tmp.PCSQty = entity.PCSQty;
                tmp.ProModel = entity.ProModel;
                tmp.LotNo = entity.LotNo + "*";
                tmp.CreateDt = entity.CreateDt;
                tmp.Status = entity.Status;
                tmp.IsJianBao = entity.IsJianBao;
                tmp.IsDuMo = entity.IsDuMo;
                tmp.DetailInfoHOLD = entity.DetailInfoHOLD;
                tmp.ImageHOLD = entity.ImageHOLD;
                tmp.IsImport = entity.IsImport;

                //得到明细中状态为HOLD的集合
                var details = entity.StockDetails.Where(p => p.IsHOLD).ToList();
                for (int i = details.Count() - 1; i >= 0; i--)
                {
                    StockDetail tmpItem = details[i];

                    // StockDetail detail = new StockDetail();
                    #region 因为实体整体赋值会出现问题，现在每项逐个复制
                    //detail.GlassID = tmpItem.GlassID;
                    //detail.Qty = tmpItem.Qty;
                    //detail.CreateDt = tmpItem.CreateDt;
                    //detail.StockLotID = tmpItem.StockLotID;
                    //detail.AccountID = tmpItem.AccountID;
                    //detail.AccountName = tmpItem.AccountName;
                    //detail.Status = tmpItem.Status;
                    //detail.IsPaoGuang = tmpItem.IsPaoGuang;
                    //detail.StockInDT = tmpItem.StockInDT;
                    //detail.JianBaoNum = tmpItem.JianBaoNum;
                    //detail.JianBaoDT = tmpItem.JianBaoDT;
                    //detail.PaoguangType = tmpItem.PaoguangType;
                    //detail.PaoGuangMian = tmpItem.PaoGuangMian;
                    //detail.PaoGuangNum = tmpItem.PaoGuangNum;
                    //detail.PaoGuangDT = tmpItem.PaoGuangDT;
                    //detail.DuMoNum = tmpItem.DuMoNum;
                    //detail.DuMoDT = tmpItem.DuMoDT;
                    //detail.StockBoxID = tmpItem.StockBoxID;
                    //detail.StockInInfo = tmpItem.StockInInfo;
                    //detail.JianBaoInfo = tmpItem.JianBaoInfo;
                    //detail.JianBaoImgInfo = tmpItem.JianBaoImgInfo;
                    //detail.DuMoInfo = tmpItem.DuMoInfo;
                    //detail.DuMoImgInfo = tmpItem.DuMoImgInfo;
                    //detail.PaoGuangInfo = tmpItem.PaoGuangInfo;
                    //detail.PaoGuangImgInfo = tmpItem.PaoGuangImgInfo;
                    //detail.IsHOLD = tmpItem.IsHOLD;
                    //detail.IsTuiHuo = tmpItem.IsTuiHuo;
                    //detail.FanGongNum = tmpItem.FanGongNum;
                    //detail.IsFanGong = tmpItem.IsFanGong;
                    //detail.JianBaoAccountID = tmpItem.JianBaoAccountID;
                    //detail.JianBaoAccountName = tmpItem.JianBaoAccountName;
                    //detail.PaoGuangAccountID = tmpItem.PaoGuangAccountID;
                    //detail.PaoGuangAccountName = tmpItem.PaoGuangAccountName;
                    //detail.DuMoAccountID = tmpItem.DuMoAccountID;
                    //detail.DuMoAccountName = tmpItem.DuMoAccountName;
                    //detail.IsPaoGuangOverInfo = tmpItem.IsPaoGuangOverInfo;
                    //detail.StockInImgInfo = tmpItem.StockInImgInfo;
                    #endregion

                    // tmp.StockDetails.Add(detail);
                    tmp.StockDetails.Add(tmpItem);
                    //这边不用去多此一举做个remove操作，如果将该Detail转移到另一个主表下，该表会自动remove
                    //entity.StockDetails.Remove(details[i]);

                }
                entities.StockLots.Add(tmp);
                //将原有的StockLot标记为修改状态
                //var entry = entities.Entry<StockLot>(entity);
                //if (entry.State == EntityState.Detached)
                //{
                //    entities.Set<StockLot>().Attach(entity);
                //    entry.State = EntityState.Modified;
                //}

                entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
            // entities.StockLots.Add(entity);
        }


        /// <summary>
        /// 获取LOTNO集合中实际入库数and出库数
        /// </summary>
        /// <param name="StockLotIDs">LOTNO ID集合</param>
        /// <returns>结果集</returns>
        public List<StockOutQtyHelper> GetStockOutQtys(List<int> StockLotIDs)
        {
            List<StockOutQtyHelper> lst = new List<StockOutQtyHelper>();

            try
            {
                if (StockLotIDs != null && StockLotIDs.Count() > 0)
                {
                    for (int i = 0; i < StockLotIDs.Count; i++)
                    {
                        int itmp = StockLotIDs[i];
                        StockLot entity = entities.StockLots.Include("StockDetails").FirstOrDefault(p => p.ID == itmp);
                        if (entity != null)
                        {
                            StockOutQtyHelper help = new StockOutQtyHelper();
                            help.LOTNO = entity.LotNo;
                            help.Qty = entity.StockDetails.Count();
                            help.StockOutQty = entity.StockDetails.Count(p => p.Status == 4 || p.Status == 5);//状态为镀膜后和出库后
                            lst.Add(help);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return lst;
        }

        #region 2014年9月14日 14:43:25
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="LotNo"></param>
        /// <param name="iStatus"></param>
        /// <param name="isShowAllDetail"></param>
        /// <param name="Qty">带出已入库数</param>
        /// <param name="OperaterQty">带出实际操作数</param>
        /// <param name="FanGongQty">返工数</param>
        /// <param name="HOLDQty">HOLD数</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool GetStockLotEntityByLotNoTotal(string checkCode, int AccountID, string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            var entity = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockDetails.StockBox.Tray").FirstOrDefault(p => p.LotNo == LotNo);

            if (entity == null)
            {
                ErrMsg = "当前不存在该LotNo";
                return false;
            }

            Qty = entity.StockDetails.Count(p => p.Status != 0);
            HOLDQty = entity.StockDetails.Count(p => p.IsHOLD);
            switch (iStatus)
            {
                case 2://减薄 
                    if (!entity.IsJianBao)
                    {
                        ErrMsg = "当前LotNo没有减薄工序";
                        return false;
                    }
                    break;
                case 4://镀膜
                    if (!entity.IsDuMo)
                    {
                        ErrMsg = "当前LotNo没有镀膜工序";
                        return false;
                    }
                    break;
                default:
                    break;
            }
            if (iStatus == 3)//如果为抛光
            {
                OperaterQty = entity.StockDetails.Where(p => p.Status >= iStatus && p.Status != 8).Count();
                FanGongQty = entity.StockDetails.Where(p => p.Status == 8).Count();
            }
            else
            {
                //   if (iStatus == 2)//如果为减薄
                OperaterQty = entity.StockDetails.Where(p => p.Status >= iStatus).Count();

            }
            return true;
        }
        #endregion



      
    }


    /// <summary>
    /// 方便统计实际出库数
    /// </summary>
    [DataContract(IsReference = true)]
    public class StockOutQtyHelper
    {
        /// <summary>
        /// LOTNO
        /// </summary>
        [DataMember]
        public string LOTNO { get; set; }
        /// <summary>
        /// 实际入库数
        /// </summary>
        [DataMember]
        public int Qty { get; set; }
        /// <summary>
        /// 实际出库数
        /// </summary>
        [DataMember]
        public int StockOutQty { get; set; }
    }
}