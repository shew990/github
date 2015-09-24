using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using SCG.SINOStock.Entities;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        /// <summary>
        /// 添加箱号
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sdoIDList"></param>
        /// <param name="IsPrintTray">是否需要打印托号</param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool AddStockBox(string checkCode, int AccountID, StockBox entity, List<int> sdoIDList, ref bool IsPrintTray, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "实体不能为空";
                return false;
            }
            if (entities.StockBoxes.Any(p => p.BarCode == entity.BarCode))
            {
                ErrMsg = "Box ID 不能重复";
                return false;
            }
            entity.CreateDt = DateTime.Now;
            try
            {
                entities.StockBoxes.Add(entity);
                // entities.SaveChanges();
                //todo:多次SavaChanges();


                foreach (int item in sdoIDList)
                {
                    var tmpDetail = entities.StockDetails.FirstOrDefault(p => p.ID == item);
                    if (tmpDetail != null)
                        tmpDetail.StockBoxID = entity.ID;
                }
                entities.SaveChanges();
                //todo:是否需要打印托号
                //StockLot sl = entities.StockLots.Include("StockBoxes").FirstOrDefault(p => p.ID == entity.StockLotOutID);
                // sl.StockBoxes
                //   FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == sl.ProModel);


                //if ((sl.StockBoxes.Count() % fw.BoxQty) == 0)
                //    IsPrintTray = true;
                //else
                //    IsPrintTray = false;
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public bool ModifyStockBox(string checkCode, int AccountID, StockBox entity, List<int> sdoIDList, List<int> stockLotIDs, ref bool IsPrintTray, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;
            if (entity == null)
            {
                ErrMsg = "实体不能为空";
                return false;
            }
            if (entities.StockBoxes.Where(p => p.ID != entity.ID).Any(p => p.BarCode == entity.BarCode))
            {
                ErrMsg = "Box ID 不能重复";
                return false;
            }
            //entity.CreateDt = DateTime.Now;
            try
            {
                //#region 标记为修改状态
                //var entry = entities.Entry<StockBox>(entity);
                //if (entry.State == EntityState.Detached)
                //{
                //    entities.Set<StockBox>().Attach(entity);
                //    entry.State = EntityState.Modified;
                //}
                //entities.SaveChanges();
                //#endregion 
                StockBox sb = entities.StockBoxes.FirstOrDefault(p => p.ID == entity.ID);
                sb.BarCode = entity.BarCode;
                sb.isPrint = true;

                foreach (int item in sdoIDList)
                {
                    var tmpDetail = entities.StockDetails.FirstOrDefault(p => p.ID == item);
                    if (tmpDetail != null)
                        tmpDetail.StockBoxID = entity.ID;
                }
                entities.SaveChanges();



                //   IQueryable<StockLot> sls = new Queryable<StockLot>();
                List<StockLot> sls = new List<StockLot>();
                foreach (int item in stockLotIDs)
                {
                    StockLot lot = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockDetails.StockBox.Tray").FirstOrDefault(p => p.ID == item);
                    if (lot != null)
                        sls.Add(lot);
                }
                // var sls = entities.StockLots.Where(p => stockLotIDs.Any(a => a == p.ID));



                //StockLot sl = entities.StockLots.Include("StockBoxes").FirstOrDefault(p => p.ID == entity.StockLotOutID);
                // sl.StockBoxes
                StockLot tmplot = sls.First();
                FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == tmplot.ProModel);

                var slDetail = sls.SelectMany(p => p.StockDetails);
                var boxs = slDetail.Select(p => p.StockBox).Where(p => p != null);
                //(boxs.Distinct().Count() % fw.BoxQty) == 0  2014年5月7日 10:57:27修改判断条件
                if (boxs.Distinct().Where(p => p.Tray == null).Count() >= fw.BoxQty)
                    IsPrintTray = true;
                else
                    IsPrintTray = false;
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public StockBox GetStockBoxToBarCode(string checkCode, int AccountID, string BarCode, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            if (string.IsNullOrEmpty(BarCode))
            {
                ErrMsg = "请填写BOX ID";
                return null;
            }
            try
            {
                StockBox entity = entities.StockBoxes.Include("StockDetails").Include("StockDetails.StockLot").FirstOrDefault(p => p.BarCode == BarCode);
                if (entity == null)
                {
                    ErrMsg = "不存在该BOX ID";
                    return null;
                }
                return entity;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        private string GetMaxBarCode_废弃(string checkCode, int AccountID, ref string ErrMsg)
        {
            CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
            string strDt = DateTime.Now.ToString("yyyyMMdd");
            var list = entities.StockBoxes.Select(p => p.BarCode).Where(p => p.Contains(strDt));
            if (list == null || list.Count() <= 0)
            {
                return strDt + "0001";
            }
            List<int> strList = new List<int>();
            foreach (string item in list)
            {
                string tmp = item.Substring(8, item.Length - 8);
                int itmp;
                if (int.TryParse(tmp, out itmp))
                {
                    strList.Add(itmp + 1);
                }
            }
            string result = strDt + strList.Max().ToString().PadLeft(4, '0');
            return result;
        }
        public string GetMaxBarCode(string checkCode, int AccountID, int LotID, ref string ErrMsg)
        {
            return "";
            //StockLot entity = entities.StockLots.Include("StockBoxes").FirstOrDefault(p => p.ID == LotID);

            //string result = string.Empty;
            //if (entity == null || entity.StockBoxes == null || entity.StockBoxes.Count() <= 0)//如果该LotID下没有编号
            //{
            //    string strDt = DateTime.Now.ToString("yyyyMMdd");
            //    var list = entities.StockBoxes.Select(p => p.BarCode).Where(p => p.Contains(strDt));
            //    if (list == null || list.Count() <= 0)
            //    {
            //        return strDt + "0001";
            //    }
            //    List<int> strList = new List<int>();
            //    foreach (string item in list)
            //    {
            //        string tmp = item.Substring(8, item.Length - 8);
            //        int itmp;
            //        if (int.TryParse(tmp, out itmp))
            //        {
            //            strList.Add(itmp + 1);
            //        }
            //    }
            //    result = strDt + strList.Max().ToString().PadLeft(4, '0');
            //}
            //else
            //{
            //    StockBox sb = entity.StockBoxes.OrderByDescending(p => p.CreateDt).FirstOrDefault();
            //    int sbLength = sb.BarCode.Length;
            //    if (sbLength >= 4)
            //    {
            //        int i = 1;
            //        string strTmpResult = sb.BarCode.Substring(0, sbLength - 3);
            //        string strTmp = sb.BarCode.Substring(sbLength - 3, 3);
            //        if (int.TryParse(strTmp, out i))
            //        {
            //            i = i + 1;
            //        }
            //        result = strTmpResult + i.ToString().PadLeft(3, '0');
            //    }
            //}
            //return result;
        }

        /// <summary>
        /// 二期多批次 无法根据LOTID去自动编号
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public string GetMaxBarCode(string checkCode, int AccountID, ref string ErrMsg)
        {
            CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
            string result = string.Empty;

            string strDt = DateTime.Now.ToString("yyyyMMdd");
            var list = entities.StockBoxes.Select(p => p.BarCode).Where(p => p.Contains(strDt));
            if (list == null || list.Count() <= 0)
            {
                return strDt + "0001";
            }
            List<int> strList = new List<int>();
            foreach (string item in list)
            {
                string tmp = item.Substring(8, item.Length - 8);
                int itmp;
                if (int.TryParse(tmp, out itmp))
                {
                    strList.Add(itmp + 1);
                }
            }
            result = strDt + strList.Max().ToString().PadLeft(4, '0');
            return result;
        }

        /// <summary>
        /// 二期多批次 无法根据LOTID去自动编号
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public string GetMaxBarCode_Ex(string checkCode, int AccountID, ref string ErrMsg)
        {
            string result = string.Empty;
            StockBox sb = entities.StockBoxes.OrderByDescending(p => p.ID).FirstOrDefault();
            if (sb == null)
            {
                return DateTime.Now.ToString("yyyyMMdd") + "0001";

            }
            string strBarCode = sb.BarCode;
            if (strBarCode.Length > 8)//如果长度大于8，则判断是否为时间格式
            {
                string tmpDtSub = strBarCode.Substring(0, 4) + "-" + strBarCode.Substring(4, 2) + "-" + strBarCode.Substring(6, 2);
                DateTime tmpdtresult;
                if (DateTime.TryParse(tmpDtSub, out tmpdtresult))//如果可以转换成时间格式
                {
                    string tmpDt = strBarCode.Substring(8, strBarCode.Length - 8);
                    int itmpDt;
                    if (int.TryParse(tmpDt, out itmpDt))
                    {
                        itmpDt += 1;

                        string dtNow = DateTime.Now.ToString("yyyy-MM-dd");
                        if (dtNow == tmpDtSub)
                            return strBarCode.Substring(0, 8) + itmpDt.ToString().PadLeft(4, '0');
                        else
                            return DateTime.Now.ToString("yyyyMMdd") + "0001";
                    }


                }

            }


            Regex rex = new Regex(@"^\d+$");

            int iIndex = 0;
            for (int i = strBarCode.Length - 1; i >= 0; i--)
            {
                string strItem = strBarCode[i].ToString();
                if (!rex.IsMatch(strItem))//如果不为
                {
                    iIndex = i;
                    break;
                }
            }

            int iIntLeng = strBarCode.Length - iIndex;
            if (iIntLeng >= 6)
            {
                iIndex = strBarCode.Length - 6;
                iIntLeng = 6;
            }
            string tmp = strBarCode.Substring(iIndex + 1);
            int itmp = int.Parse(tmp);

            itmp += 1;
            result = strBarCode.Substring(0, iIndex + 1) + itmp.ToString().PadLeft(iIntLeng - 1, '0');
            return result;
        }


        #region 获取自动编号，在获取的时候就向Box表中插入数据，然后返回Box对象

        public StockBox GetMaxStockBox(string checkCode, int AccountID, int LotID, ref string ErrMsg)
        {
            try
            {
                string strBarCode = GetMaxBarCode(checkCode, AccountID, ref ErrMsg);
                StockBox sb = new StockBox();
                sb.BarCode = strBarCode;
                sb.CreateDt = DateTime.Now;
                sb.isPrint = true;


                entities.StockBoxes.Add(sb);
                entities.SaveChanges();
                return sb;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 2014年7月22日 16:16:43
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="LotID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockBox GetMaxStockBox_Ex(string checkCode, int AccountID, ref string ErrMsg)
        {
            try
            {

                string strBarCode = GetMaxBarCode_Ex(checkCode, AccountID, ref ErrMsg);
                StockBox sb = new StockBox();
                sb.BarCode = strBarCode;
                sb.CreateDt = DateTime.Now;
                sb.isPrint = false;
                StockBox s = entities.StockBoxes.FirstOrDefault(p => p.BarCode == strBarCode);
                if (s == null)
                    sb.CreateAccountID = AccountID;

                entities.StockBoxes.Add(sb);
                entities.SaveChanges();
                return sb;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }


        public StockBox ChangeBoxBarCode(string strBarCode, ref string ErrMsg)
        {
            try
            {
                //  string strBarCode = GetMaxBarCode_Ex(checkCode, AccountID, ref ErrMsg);
                if (entities.StockBoxes.Any(p => p.BarCode == strBarCode))
                {
                    ErrMsg = "输入的BOXID已存在";
                    return null;
                }
                StockBox sb = new StockBox();
                sb.BarCode = strBarCode;
                sb.CreateDt = DateTime.Now;
                sb.isPrint = true;


                entities.StockBoxes.Add(sb);
                entities.SaveChanges();
                return sb;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public StockBox ChangeBoxBarCode_Pro(string strNewBarCode, string strOldBarCode, int AccountID, ref string ErrMsg)
        {
            try
            {
                //  string strBarCode = GetMaxBarCode_Ex(checkCode, AccountID, ref ErrMsg);
                if (entities.StockBoxes.Any(p => p.BarCode == strNewBarCode))
                {
                    ErrMsg = "输入的BOXID已存在";
                    return null;
                }

                StockBox sb = entities.StockBoxes.FirstOrDefault(p => p.BarCode == strOldBarCode);
                sb.BarCode = strNewBarCode;
                sb.IsModify = true;
                sb.isPrint = true;

                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "修改失败";
                    return null;
                }

                return sb;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        #endregion

        public List<StockBox> GetBoxListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg)
        {
            CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
            DateTime startDt = StartDt.Date;
            DateTime endDt = EndDt.Date;
            return entities.StockBoxes.Where(p => p.CreateDt >= startDt && p.CreateDt <= endDt).ToList();
        }




        /// <summary>
        /// 补打时：修改外箱标签
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="entity"></param>
        /// <param name="sdoIDList"></param>
        /// <param name="stockLotIDs"></param>
        /// <param name="IsPrintTray"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool ModifyBoxBarCode(string checkCode, int AccountID, string OldBarCode, string NewBarCode, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return false;

            if (entities.StockBoxes.Any(p => p.BarCode == NewBarCode))
            {
                ErrMsg = "Box ID 已存在，请重新输入";
                return false;
            }

            try
            {

                StockBox sb = entities.StockBoxes.FirstOrDefault(p => p.BarCode == OldBarCode);
                sb.BarCode = NewBarCode;
                sb.IsModify = true;

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
        /// 查出当前登录用户最近一个未打印的箱号实体
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public StockBox GetStocBoxkEntityByIsUnPrint(string checkCode, int AccountID, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;

            try
            {
                return entities.StockBoxes.Include("StockDetails").Include("StockDetails.StockLot").FirstOrDefault(p => p.isPrint == false && p.CreateAccountID == AccountID);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
    }
}