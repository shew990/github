using SCG.SINOStock.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkCode"></param>
        /// <param name="AccountID"></param>
        /// <param name="entity"></param>
        /// <param name="StockLotIDs"></param>
        /// <param name="isQiangDa">是否强打 true强打  false 正常</param>
        /// <param name="Qty"></param>
        /// <param name="Boxs"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public bool AddTray(string checkCode, int AccountID, Tray entity, List<int> StockLotIDs, bool isQiangDa, ref int Qty, ref List<string> Boxs, ref string ErrMsg)
        {
            try
            {
                if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                    return false;
                if (entity == null)
                {
                    ErrMsg = "实体为空";
                    return false;
                }
                if (string.IsNullOrEmpty(entity.BarCode))
                {
                    ErrMsg = "请填写托号";
                    return false;
                }

                Tray tray = entities.Trays.FirstOrDefault(p => p.BarCode == entity.BarCode);
                if (tray == null)
                {
                    tray = new Tray();
                    tray.BarCode = entity.BarCode;
                    tray.CreateDt = DateTime.Now;
                    entities.Trays.Add(tray);
                }
                // entity.CreateDt = DateTime.Now;
                //   entities.Trays.Add(entity);


                List<StockLot> sls = new List<StockLot>();
                foreach (int item in StockLotIDs)
                {
                    StockLot lot = entities.StockLots.Include("StockDetails").Include("StockDetails.StockBox").Include("StockDetails.StockBox.Tray").FirstOrDefault(p => p.ID == item);
                    if (lot != null)
                        sls.Add(lot);
                }



                // StockLot sl = entities.StockLots.Include("StockBoxes").Include("StockBoxes.StockOutDetails").FirstOrDefault(p => p.ID == StockLotID);
                StockLot tmplot = sls.First();
                FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == tmplot.ProModel);


                //todo:Box与LOT关系取消
                var slDetail = sls.SelectMany(p => p.StockDetails);
                //   var tmpbox = slDetail.Where(p => p.StockBoxID != null);
                var boxs = slDetail.Where(p => p.StockBoxID != null).Select(p => p.StockBox).Where(p => p.TrayID == null).Distinct().OrderByDescending(p => p.CreateDt).Take(fw.BoxQty);
                //  var boxList = sl.StockBoxes.Where(p => p.TrayID == null).Take(fw.BoxQty);
                Boxs = boxs.Select(p => p.BarCode).ToList();
                if (boxs.Count() < fw.BoxQty && (!isQiangDa))
                {
                    ErrMsg = "本次打印已在其他电脑上完成，点击确定继续";
                    return false;
                }
                foreach (StockBox item in boxs)
                {
                    item.TrayID = tray.ID;
                    Qty += item.StockDetails.Count();
                }
                entities.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public Tray GetTrayByBarCode(string checkCode, int AccountID, string BarCode, ref int Qty, ref string ProModel, ref string ErrMsg)
        {
            if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                return null;
            if (string.IsNullOrEmpty(BarCode))
            {
                ErrMsg = "请输入托号";
                return null;
            }
            try
            {
                Tray entity = entities.Trays.Include("StockBoxes").Include("StockBoxes.StockDetails").Include("StockBoxes.StockDetails.StockLot").FirstOrDefault(p => p.BarCode == BarCode);
                if (entity == null)
                {
                    ErrMsg = "该托号不存在";
                    return null;
                }
                if (entity.StockBoxes == null || entity.StockBoxes.Count() <= 0)
                {
                    ErrMsg = "所选托号不包含外箱标签，请重新操作补打托号";
                    return null;
                }
                if (entity.StockBoxes != null && entity.StockBoxes.Count() > 0)
                {

                    ProModel = entity.StockBoxes.First().StockDetails.First().StockLot.ProModel;
                    Qty = entity.StockBoxes.Sum(p => p.StockDetails.Count());
                }
                return entity;
                //var boxList = sl.StockBoxes.Where(p => p.TrayID == entity.ID);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        private string GetTrayMaxBarCode(string checkCode, int AccountID, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                string strDt = DateTime.Now.ToString("yyyyMM");
                var list = entities.Trays.Select(p => p.BarCode).Where(p => p.Contains(strDt));
                if (list == null || list.Count() <= 0)
                {
                    return strDt + "0001";
                }
                List<int> strList = new List<int>();
                foreach (string item in list)
                {
                    string tmp = item.Substring(6, item.Length - 6);
                    int itmp;
                    if (int.TryParse(tmp, out itmp))
                    {
                        strList.Add(itmp + 1);
                    }
                }
                string result = strDt + strList.Max().ToString().PadLeft(4, '0');



                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return string.Empty;
            }
        }
        public string GetTrayMaxBarCode(string checkCode, int AccountID, int LogID, ref string ErrMsg)
        {
            try
            {
                //CheckingCheckCode(checkCode, AccountID, ref ErrMsg);

                //string strDt = DateTime.Now.ToString("yyyyMM");
                ////  StockLotOut entity = entities.StockLotOuts.Include("StockBox").Include("StockBox.Tray").FirstOrDefault(p => p.ID == LogID);
                //var boxs = entities.StockBoxes.Include("Tray");//todo:Box与LOT关系取消.Where(p => p.StockLotOutID == LogID);
                //var tmpBoxs = boxs.Where(p => p.TrayID != null && p.Tray.BarCode.Contains(strDt));

                //string result = string.Empty;
                //if (boxs == null || tmpBoxs.Count() <= 0)//如果该LotID下没有编号
                //{

                //    var list = entities.Trays.Select(p => p.BarCode).Where(p => p.Contains(strDt));
                //    if (list == null || list.Count() <= 0)
                //    {
                //        string strtmpp = strDt + "0001";
                //        Tray tray1 = new Tray();
                //        tray1.BarCode = strtmpp;
                //        tray1.CreateDt = DateTime.Now;
                //        entities.Trays.Add(tray1);
                //        entities.SaveChanges();

                //        return strtmpp;
                //    }
                //    List<int> strList = new List<int>();
                //    foreach (string item in list)
                //    {
                //        string tmp = item.Substring(6, item.Length - 6);
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
                //    //Tray tb=entities

                //    StockBox sb = tmpBoxs.OrderByDescending(p => p.Tray.CreateDt).FirstOrDefault();
                //    int sbLength = sb.Tray.BarCode.Length;
                //    if (sbLength >= 4)
                //    {
                //        int i = 1;
                //        string strTmpResult = sb.Tray.BarCode.Substring(0, sbLength - 3);
                //        string strTmp = sb.Tray.BarCode.Substring(sbLength - 3, 3);
                //        if (int.TryParse(strTmp, out i))
                //        {
                //            i = i + 1;


                //            //不大改  将就一下
                //            string tmp = strTmpResult + i.ToString().PadLeft(3, '0');
                //            while (entities.Trays.Any(p => p.BarCode == tmp))
                //            {
                //                i = i + 1;
                //                tmp = strTmpResult + i.ToString().PadLeft(3, '0');
                //            }
                //        }
                //        result = strTmpResult + i.ToString().PadLeft(3, '0');
                //    }
                //}

                string result = string.Empty;
                Tray sb = entities.Trays.OrderByDescending(p => p.ID).FirstOrDefault();

                if (sb == null)
                {
                    return DateTime.Now.ToString("yyyyMM") + "0001";

                }
                string strBarCode = sb.BarCode;
                if (strBarCode.Length > 6)//如果长度大于8，则判断是否为时间格式
                {
                    string tmpDtSub = strBarCode.Substring(0, 4) + "-" + strBarCode.Substring(4, 2);
                    DateTime tmpdtresult;
                    if (DateTime.TryParse(tmpDtSub, out tmpdtresult))//如果可以转换成时间格式
                    {
                        string tmpDt = strBarCode.Substring(6, strBarCode.Length - 6);
                        int itmpDt;
                        if (int.TryParse(tmpDt, out itmpDt))
                        {
                            itmpDt += 1;

                            string dtNow = DateTime.Now.ToString("yyyy-MM");
                            if (dtNow == tmpDtSub)
                                return strBarCode.Substring(0, 6) + itmpDt.ToString().PadLeft(4, '0');
                            else
                            {
                                string tmpdtNow = DateTime.Now.ToString("yyyyMM");
                                string strlast = string.Empty;
                                var tmplist = entities.Trays.Where(p => p.BarCode.Contains(tmpdtNow));
                                if (tmplist != null && tmplist.Count() > 0)
                                {
                                    strBarCode = tmplist.OrderByDescending(p => p.ID).FirstOrDefault().BarCode;
                                    tmpDt = strBarCode.Substring(6, strBarCode.Length - 6);
                                    if (int.TryParse(tmpDt, out itmpDt))
                                    {
                                        itmpDt += 1;
                                    }
                                    else
                                        itmpDt = 1;
                                    return strBarCode.Substring(0, 6) + itmpDt.ToString().PadLeft(4, '0');
                                }
                                return DateTime.Now.ToString("yyyyMM") + "0001";
                            }
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
                if (iIntLeng >= 5)
                {
                    iIndex = strBarCode.Length - 5;
                    iIntLeng = 5;
                }
                string tmp = strBarCode.Substring(iIndex + 1);
                int itmp = int.Parse(tmp);

                itmp += 1;
                result = strBarCode.Substring(0, iIndex + 1) + itmp.ToString().PadLeft(iIntLeng - 1, '0');



                Tray tray = new Tray();
                tray.BarCode = result;
                tray.CreateDt = DateTime.Now;
                entities.Trays.Add(tray);
                entities.SaveChanges();

                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return string.Empty;
            }
        }
        public bool ChangeTryBarCode(string strBarCode, ref string ErrMsg)
        {
            try
            {
                //  string strBarCode = GetMaxBarCode_Ex(checkCode, AccountID, ref ErrMsg);
                if (entities.Trays.Any(p => p.BarCode == strBarCode))
                {
                    ErrMsg = "输入的托号ID已存在";
                    return false;
                }
                Tray tray = new Tray();
                tray.BarCode = strBarCode;
                tray.CreateDt = DateTime.Now;
                entities.Trays.Add(tray);
                entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public List<Tray> GetTrayListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg)
        {
            DateTime startDt = StartDt.Date;
            DateTime endDt = EndDt.Date;
            return entities.Trays.Where(p => p.CreateDt >= startDt && p.CreateDt <= endDt).ToList();
        }

        public Tray GetMaxTray(string checkCode, int AccountID, int LotID, ref string ErrMsg)
        {
            try
            {
                CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
                string strBarCode = GetMaxBarCode(checkCode, AccountID, LotID, ref ErrMsg);
                Tray sb = new Tray();
                sb.BarCode = strBarCode;
                sb.CreateDt = DateTime.Now;
                //   sb.isPrint = true;
                //sb.StockLotOutID = LotID;

                entities.Trays.Add(sb);
                entities.SaveChanges();
                return sb;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public bool ModifyTray(string checkCode, int AccountID, Tray entity, int StockLotID, ref int Qty, ref string ErrMsg)
        {
            try
            {
                if (!CheckingCheckCode(checkCode, AccountID, ref ErrMsg))
                    return false;
                if (entity == null)
                {
                    ErrMsg = "实体为空";
                    return false;
                }
                if (string.IsNullOrEmpty(entity.BarCode))
                {
                    ErrMsg = "请填写托号";
                    return false;
                }
                if (entities.Trays.Where(p => p.ID != entity.ID).Any(p => p.BarCode == entity.BarCode))
                {
                    ErrMsg = "该托号已被使用，请重新输入";
                    return false;
                }
                //entity.CreateDt = DateTime.Now;
                //entities.Trays.Add(entity);
                Tray t = entities.Trays.FirstOrDefault(p => p.ID == entity.ID);
                t.BarCode = entity.BarCode;


                StockLot sl = entities.StockLots.Include("StockBoxes").Include("StockBoxes.StockOutDetails").FirstOrDefault(p => p.ID == StockLotID);
                FormWork fw = entities.FormWorks.FirstOrDefault(p => p.ProductModel == sl.ProModel);
                //todo:Box与LOT关系取消
                //var boxList = sl.StockBoxes.Where(p => p.TrayID == null).Take(fw.BoxQty);
                //foreach (StockBox item in boxList)
                //{
                //    item.TrayID = entity.ID;
                //    Qty += item.StockDetails.Count();
                //}
                entities.SaveChanges();
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