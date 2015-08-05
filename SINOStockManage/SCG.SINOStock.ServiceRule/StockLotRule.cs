using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     StockLotRule
 *   说明:       
 *   创建时间:   2014/2/10 22:57:08
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class StockLotRule : RuleBase
    {

        private int _stockID;

        public int StockID
        {
            get { return _stockID; }
            set
            {
                _stockID = value;
                OnPropertyChanged("StockID");
            }
        }

        public event EventHandler<ResultArgs<bool>> AddStockLotCompleted;
        public void AddStockLotAsyns(StockLot entity)
        {
            string ErrMsg = string.Empty;
            int tmpStockID = 0;
            Proxy.BeginAddStockLot(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref tmpStockID, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndAddStockLot(ref tmpStockID, ref ErrMsg, result);
                        if (bResult)
                        {
                            StockID = tmpStockID;
                            AddStockLotCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            AddStockLotCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (AddStockLotCompleted != null)
                        {
                            AddStockLotCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> DeleteStockLotCompleted;
        public void DeleteStockLotAsyns(int StockLotID)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginDeleteStockLot(CurrentAccount.CheckCode, CurrentAccount.ID, StockLotID, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndDeleteStockLot(ref ErrMsg, result);
                        if (bResult)
                        {
                            DeleteStockLotCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            DeleteStockLotCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DeleteStockLotCompleted != null)
                        {
                            DeleteStockLotCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }
        /// <summary>
        /// 批量从Excel中导入GlassID到数据库成功事件
        /// </summary>
        public event EventHandler<ResultArgs<bool>> AddStockLotAndDetailsCompleted;
        /// <summary>
        /// 批量从Excel中导入GlassID到数据库（异步）
        /// </summary>
        /// <param name="fileName">Excel完整路径</param>
        /// <param name="ProductModel">产品型号</param>
        /// <param name="LotNo"></param>
        public void AddStockLotAndDetailsAsyns(string fileName, StockLot entity)
        {
            string ErrMsg = string.Empty;
            int tmpStockID = 0;
            if (entity.IsImport)//如果为导入模版
            {
                ImportStockLot import = new ImportStockLot();
                List<StockDetail> detailList = import.ImportToXls(fileName, ref ErrMsg);
                if (detailList == null || detailList.Count() <= 0)
                {
                    AddStockLotAndDetailsCompleted(this, new ResultArgs<bool>(false, new Exception(ErrMsg), true, null));
                    return;
                }
                entity.StockDetails = detailList.ToArray();
                entity.PCSQty = detailList.Count();
            }
            //StockLot entity = new StockLot();
            //entity.Status = 8;
            //entity.LotNo = LotNo;
            //entity.PCSQty = detailList.Count();
            //entity.ProModel = ProductModel;
            //foreach (StockDetail item in detailList)
            //{

            // }
            entity.CreateAccountID = CurrentAccount.ID;
            entity.CreateAccountName = CurrentAccount.Name;
            Proxy.BeginAddStockLot(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref tmpStockID, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndAddStockLot(ref tmpStockID, ref ErrMsg, result);
                        if (bResult)
                        {
                            StockID = tmpStockID;
                            AddStockLotAndDetailsCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            AddStockLotAndDetailsCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (AddStockLotAndDetailsCompleted != null)
                        {
                            AddStockLotAndDetailsCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public int StockInQty;
        public int OperaterQty;
        public int FanGongQty;
        public int HOLDQty;
        public event EventHandler<ResultArgs<StockLot>> GetStockLotEntityByLotNoCompleted;
        public void GetStockLotEntityByLotNoAsyns(string strLotNo, int iStatus, bool isShowAllDetail)
        {
            string ErrMsg = string.Empty;
            StockInQty = 0;
            OperaterQty = 0;
            FanGongQty = 0;
            HOLDQty = 0;
            Proxy.BeginGetStockLotEntityByLotNo(CurrentAccount.CheckCode, CurrentAccount.ID, strLotNo, iStatus, isShowAllDetail, ref StockInQty, ref OperaterQty, ref FanGongQty, ref HOLDQty, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        StockLot entity = Proxy.EndGetStockLotEntityByLotNo(ref StockInQty, ref OperaterQty, ref FanGongQty, ref HOLDQty, ref ErrMsg, result);
                        if (GetStockLotEntityByLotNoCompleted != null)
                        {


                            if (entity == null)
                                GetStockLotEntityByLotNoCompleted(this, new ResultArgs<StockLot>(entity, new Exception(ErrMsg), true, result.AsyncState));
                            else
                                GetStockLotEntityByLotNoCompleted(this, new ResultArgs<StockLot>(entity, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetStockLotEntityByLotNoCompleted != null)
                        {
                            GetStockLotEntityByLotNoCompleted(this, new ResultArgs<StockLot>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public int IStockInQty { get; set; }//已入库数
        public int HOLDQty_Ex;
        public event EventHandler<ResultArgs<StockLot>> GetStockLotEntityByLotNo_ExCompleted;
        public void GetStockLotEntityByLotNo_ExAsyns(string strLotNo, int iStatus, bool isShowAllDetail)
        {
            string ErrMsg = string.Empty;
            int iQty = 0;
            HOLDQty_Ex = 0;
            Proxy.BeginGetStockLotEntityByLotNo_Ex(CurrentAccount.CheckCode, CurrentAccount.ID, strLotNo, iStatus, isShowAllDetail, ref iQty, ref HOLDQty_Ex, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        StockLot entity = Proxy.EndGetStockLotEntityByLotNo_Ex(ref iQty, ref HOLDQty_Ex, ref ErrMsg, result);
                        IStockInQty = iQty;
                        if (GetStockLotEntityByLotNo_ExCompleted != null)
                        {
                            if (entity == null)
                                GetStockLotEntityByLotNo_ExCompleted(this, new ResultArgs<StockLot>(entity, new Exception(ErrMsg), true, result.AsyncState));
                            else
                                GetStockLotEntityByLotNo_ExCompleted(this, new ResultArgs<StockLot>(entity, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetStockLotEntityByLotNo_ExCompleted != null)
                        {
                            GetStockLotEntityByLotNo_ExCompleted(this, new ResultArgs<StockLot>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }


        public event EventHandler<ResultsArgs<StockLot>> GetStockLotEntityListByLotNoCompleted;
        //2014年3月9日 23:37:09 刷新专用（出库）
        public void GetStockLotEntityListByLotNoAsyns(List<int> lotIDs, int iStatus, bool isShowAllDetail)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginGetStockLotEntityListByLotNo(CurrentAccount.CheckCode, CurrentAccount.ID, lotIDs.ToArray(), iStatus, isShowAllDetail, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        if (GetStockLotEntityListByLotNoCompleted != null)
                        {
                            GetStockLotEntityListByLotNoCompleted(this, new ResultsArgs<StockLot>(Proxy.EndGetStockLotEntityListByLotNo(ref ErrMsg, result), new Exception(ErrMsg), false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetStockLotEntityListByLotNoCompleted != null)
                        {
                            GetStockLotEntityListByLotNoCompleted(this, new ResultsArgs<StockLot>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }




        public event EventHandler<ResultsArgs<StockLot>> GetStockLotList_TwoCompleted;
        public void GetStockLotList_TwoAsyns(Dictionary<string, string> queryList)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginGetStockLotList_Two(CurrentAccount.CheckCode, CurrentAccount.ID, queryList, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        StockLot[] lst = Proxy.EndGetStockLotList_Two(ref ErrMsg, result);
                        if (GetStockLotList_TwoCompleted != null)
                        {
                            if (lst == null)
                                GetStockLotList_TwoCompleted(this, new ResultsArgs<StockLot>(lst, new Exception(ErrMsg), true, result.AsyncState));
                            else
                                GetStockLotList_TwoCompleted(this, new ResultsArgs<StockLot>(lst, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetStockLotList_TwoCompleted != null)
                        {
                            GetStockLotList_TwoCompleted(this, new ResultsArgs<StockLot>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<bool>> HOLDAllToNewStockLotCompleted;
        public void HOLDAllToNewStockLotAsyns(int StockLotID)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginHOLDAllToNewStockLot(CurrentAccount.CheckCode, CurrentAccount.ID, StockLotID, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndHOLDAllToNewStockLot(ref ErrMsg, result);
                        if (bResult)
                        {
                            HOLDAllToNewStockLotCompleted(this, new ResultArgs<bool>(true, null, false, result.AsyncState));
                        }
                        else
                        {
                            HOLDAllToNewStockLotCompleted(this, new ResultArgs<bool>(false, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetStockLotList_TwoCompleted != null)
                        {
                            HOLDAllToNewStockLotCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }
        /// <summary>
        /// 获取LOTNO集合中实际入库数and出库数
        /// </summary>
        /// <param name="StockLotIDs">LOTNO ID集合</param>
        /// <returns>结果集</returns>
        public StockOutQtyHelper[] GetStockOutQtys(int[] StockLotIDs)
        {
            return Proxy.GetStockOutQtys(StockLotIDs);
        }

        public bool EndStockLot(int StockLotID, ref string ErrMsg)
        {
            return Proxy.EndStockLot(CurrentAccount.CheckCode, CurrentAccount.ID, StockLotID, ref ErrMsg);
        }


        #region StockDdetail

        private int _qtyCount;

        public int QtyCount
        {
            get { return _qtyCount; }
            set
            {
                _qtyCount = value;
                OnPropertyChanged("QtyCount");
            }
        }
        /// <summary>
        /// 新增GlassID成功事件
        /// </summary>
        public event EventHandler<ResultArgs<bool>> AddStockDetailCompleted;
        /// <summary>
        /// 新增GlassID异步
        /// </summary>
        /// <param name="entity">需要新增的实体</param>
        /// <param name="IsCheck">是否需要对比关键字</param>
        public void AddStockDetailAsyns(StockDetail entity, bool IsCheck)
        {
            int tmpQtyCount = 0;
            string ErrMsg = string.Empty;
            entity.AccountID = CurrentAccount.ID;
            entity.AccountName = CurrentAccount.Name;

            Proxy.BeginAddStockDetail(CurrentAccount.CheckCode, CurrentAccount.ID, entity, IsCheck, ref tmpQtyCount, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndAddStockDetail(ref tmpQtyCount, ref ErrMsg, result);
                        if (bResult)
                        {
                            QtyCount = tmpQtyCount;
                            AddStockDetailCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            AddStockDetailCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (AddStockDetailCompleted != null)
                        {
                            AddStockDetailCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }



        private int _countQty;

        public int CountQty
        {
            get { return _countQty; }
            set
            {
                _countQty = value;
                OnPropertyChanged("QtyCount");
            }
        }
        public event EventHandler<ResultsArgs<StockDetail>> GetStockDetailListCompleted;
        public void GetStockDetailListAsyns(int StockLotID, bool IsCheckAll)
        {
            int tmpCountQty = 0;
            string ErrMsg = string.Empty;
            int accountID = 0;
            if (!IsCheckAll)
                accountID = CurrentAccount.ID;
            Proxy.BeginGetStockDetailList(CurrentAccount.CheckCode, CurrentAccount.ID, StockLotID, accountID, ref tmpCountQty, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        var ResultCollection = new ObservableCollection<StockDetail>(Proxy.EndGetStockDetailList(ref tmpCountQty, ref ErrMsg, result));
                        if (AddStockDetailCompleted != null)
                        {
                            CountQty = tmpCountQty;
                            GetStockDetailListCompleted(this, new ResultsArgs<StockDetail>(ResultCollection, null, false, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (GetStockDetailListCompleted != null)
                        {
                            GetStockDetailListCompleted(this, new ResultsArgs<StockDetail>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }


        //是否为后台在操作，true为是  false为否
        public bool IsHouTai = false;
        public event EventHandler<ResultArgs<bool>> ModifyStockDetailListCompleted;
        public void ModifyStockDetailListAsyns(StockDetail entity)
        {
            switch (entity.Status)
            {
                case 2://减薄
                    entity.JianBaoAccountID = CurrentAccount.ID;
                    entity.JianBaoAccountName = CurrentAccount.Name;
                    //    entity.DuMoDT = DateTime.Now;
                    break;
                case 3://抛光
                case 8:
                    entity.PaoGuangAccountID = CurrentAccount.ID;
                    if (!IsHouTai)
                    {
                        entity.PaoGuangAccountName += CurrentAccount.Name + ",";
                    }
                    //entity.PaoGuangDT = DateTime.Now;
                    break;
                case 4://镀膜
                    entity.DuMoAccountID = CurrentAccount.ID;
                    entity.DuMoAccountName = CurrentAccount.Name;
                   // entity.DuMoDT = DateTime.Now;
                    break;
                default:
                    break;
            }
            string ErrMsg = string.Empty;
            Proxy.BeginModifyStockDetail(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndModifyStockDetail(ref ErrMsg, result);
                        if (bResult)
                        {
                            ModifyStockDetailListCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            ModifyStockDetailListCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ModifyStockDetailListCompleted != null)
                        {
                            ModifyStockDetailListCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }





        public event EventHandler<ResultArgs<bool>> UpdateStockDetailStatusCompleted;
        public void UpdateStockDetailStatusAsyns(string strGlassID, int strLotID, int iStatus)
        {
            string ErrMsg = string.Empty;

            Proxy.BeginUpdateStockDetailStatus(CurrentAccount.CheckCode, CurrentAccount.ID, strGlassID, strLotID, iStatus, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndUpdateStockDetailStatus(ref ErrMsg, result);
                        if (bResult)
                        {
                            UpdateStockDetailStatusCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            UpdateStockDetailStatusCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (UpdateStockDetailStatusCompleted != null)
                        {
                            UpdateStockDetailStatusCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }

        public event EventHandler<ResultArgs<StockDetail>> CheckStockDetailStatusCompleted;
        public void CheckStockDetailStatusAsyns(string strGlassID, int strLotID, int iStatus)
        {
            string ErrMsg = string.Empty;

            Proxy.BeginCheckStockDetailStatus(CurrentAccount.CheckCode, CurrentAccount.ID, strGlassID, strLotID, iStatus, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        StockDetail entity = Proxy.EndCheckStockDetailStatus(ref ErrMsg, result);
                        if (entity != null)
                        {
                            CheckStockDetailStatusCompleted(this, new ResultArgs<StockDetail>(entity, null, false, result.AsyncState));
                        }
                        else
                        {
                            CheckStockDetailStatusCompleted(this, new ResultArgs<StockDetail>(null, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (CheckStockDetailStatusCompleted != null)
                        {
                            CheckStockDetailStatusCompleted(this, new ResultArgs<StockDetail>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }


        public event EventHandler<ResultArgs<StockDetail>> CheckStockDetailStatus_OutCompleted;
        public void CheckStockDetailStatus_OutAsyns(string strGlassID, List<int> strLotIDs, int iStatus)
        {
            string ErrMsg = string.Empty;

            Proxy.BeginCheckStockDetailStatus_Out(CurrentAccount.CheckCode, CurrentAccount.ID, strGlassID, strLotIDs.ToArray(), iStatus, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        StockDetail entity = Proxy.EndCheckStockDetailStatus_Out(ref ErrMsg, result);
                        if (entity != null)
                        {
                            CheckStockDetailStatus_OutCompleted(this, new ResultArgs<StockDetail>(entity, null, false, result.AsyncState));
                        }
                        else
                        {
                            CheckStockDetailStatus_OutCompleted(this, new ResultArgs<StockDetail>(null, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (CheckStockDetailStatus_OutCompleted != null)
                        {
                            CheckStockDetailStatus_OutCompleted(this, new ResultArgs<StockDetail>(null, ex, true, result.AsyncState));
                        }
                    }
                });
            }, null);
        }



        public StockDetail GetStockDetailByGlassID(string GlassID, ref string ErrMsg)
        {
            return Proxy.GetStockDetailByGlassID(CurrentAccount.CheckCode, CurrentAccount.ID, GlassID, ref ErrMsg);
        }
        public bool CheckStockDetail_In(string strGlassID, int strLotID, bool isCheck, ref string ErrMsg)
        {
            return Proxy.CheckStockDetail_In_Ex(CurrentAccount.CheckCode, CurrentAccount.ID, strGlassID, strLotID, isCheck, ref ErrMsg);
        }
        public bool DeleteStockDetail(StockDetail entity, ref string ErrMsg)
        {
            return Proxy.DeleteStockDetail(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg);
        }
        /// <summary>
        /// 退货功能（删除GlassID数据，并在主表累加退货数）  2014年5月4日 11:12:49
        /// </summary>
        /// <param name="StockDetailID">GlassID ID</param>
        /// <param name="ErrMsg">返回错误信息</param>
        /// <returns></returns>
        public bool DelStockDetailAndTuihuoCount(int StockDetailID, ref string ErrMsg)
        {
            return Proxy.DelStockDetailAndTuihuoCount(CurrentAccount.CheckCode, CurrentAccount.ID, StockDetailID, ref ErrMsg);
        }




        public bool GetStockLotEntityByLotNoTotal( string LotNo, int iStatus, bool isShowAllDetail, ref int Qty, ref int OperaterQty, ref int FanGongQty, ref int HOLDQty, ref string ErrMsg)
        {
            return Proxy.GetStockLotEntityByLotNoTotal(CurrentAccount.CheckCode, CurrentAccount.ID, LotNo, iStatus, isShowAllDetail, ref  Qty, ref  OperaterQty, ref  FanGongQty, ref  HOLDQty, ref  ErrMsg);
        }
        #endregion

        #region 2015年3月26日14:58:36  新增功能：HOLD状态修改
        public event EventHandler<ResultArgs<bool>> ChangeStockDetail_HOLDCompleted;
        /// <summary>
        /// 修改HOLD状态
        /// </summary>
        /// <param name="entity"></param>
        public void ChangeStockDetail_HOLDAsyns(StockDetail entity)
        {
            string ErrMsg = string.Empty;
            Proxy.BeginModifyStockDetail(CurrentAccount.CheckCode, CurrentAccount.ID, entity, ref ErrMsg, result =>
            {
                ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    try
                    {
                        bool bResult = Proxy.EndModifyStockDetail(ref ErrMsg, result);
                        if (bResult)
                        {
                            ChangeStockDetail_HOLDCompleted(this, new ResultArgs<bool>(bResult, null, false, result.AsyncState));
                        }
                        else
                        {
                            ChangeStockDetail_HOLDCompleted(this, new ResultArgs<bool>(bResult, new Exception(ErrMsg), true, result.AsyncState));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ChangeStockDetail_HOLDCompleted != null)
                        {
                            ChangeStockDetail_HOLDCompleted(this, new ResultArgs<bool>(false, ex, true, result.AsyncState));
                        }
                    }
                });
            },entity.IsHOLD);
        }

        #endregion

    }
}
