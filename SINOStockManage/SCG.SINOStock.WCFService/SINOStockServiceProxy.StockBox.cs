using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     SINOStockServiceProxy
 *   说明:       
 *   创建时间:   2014/2/26 16:51:44
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public partial class SINOStockServiceProxy
    {
        public bool AddStockBox(string checkCode, int AccountID, StockBox entity, int[] sdoIDList, ref bool IsPrintTray, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddStockBox(string checkCode, int AccountID, StockBox entity, int[] sdoIDList, ref bool IsPrintTray, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndAddStockBox(ref bool IsPrintTray, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockBox GetStockBoxToBarCode(string checkCode, int AccountID, string BarCode, ref string ErrMsg)
        {
            return _client.GetStockBoxToBarCode(checkCode, AccountID, BarCode, ref ErrMsg);
        }

        public IAsyncResult BeginGetStockBoxToBarCode(string checkCode, int AccountID, string BarCode, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockBox EndGetStockBoxToBarCode(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public string GetMaxBarCode(string checkCode, int AccountID, int LotID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetMaxBarCode(string checkCode, int AccountID, int LotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public string EndGetMaxBarCode(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockBox[] GetBoxListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg)
        {
            return _client.GetBoxListByDt(checkCode, AccountID, StartDt, EndDt, ref ErrMsg);
        }

        public IAsyncResult BeginGetBoxListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockBox[] EndGetBoxListByDt(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }



        //================================================


        public StockBox GetMaxStockBox(string checkCode, int AccountID, int LotID, ref string ErrMsg)
        {
            try
            {
                return _client.GetMaxStockBox(checkCode, AccountID, LotID, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginGetMaxStockBox(string checkCode, int AccountID, int LotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockBox EndGetMaxStockBox(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool ModifyStockBox(string checkCode, int AccountID, StockBox entity, int[] sdoIDList, int[] stockLotIDs, ref bool IsPrintTray, ref string ErrMsg)
        {
            try
            {
                return _client.ModifyStockBox(checkCode, AccountID, entity, sdoIDList, stockLotIDs, ref IsPrintTray, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public IAsyncResult BeginModifyStockBox(string checkCode, int AccountID, StockBox entity, int[] sdoIDList, int[] stockLotIDs, ref bool IsPrintTray, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndModifyStockBox(ref bool IsPrintTray, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        //======================2014年7月22日 22:13:21===============================
        public string GetMaxBarCode_Ex(string checkCode, int AccountID, ref string ErrMsg)
        {
            try
            {
                return _client.GetMaxBarCode_Ex(checkCode, AccountID, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginGetMaxBarCode_Ex(string checkCode, int AccountID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public string EndGetMaxBarCode_Ex(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockBox GetMaxStockBox_Ex(string checkCode, int AccountID, ref string ErrMsg)
        {
            try
            {
                return _client.GetMaxStockBox_Ex(checkCode, AccountID, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginGetMaxStockBox_Ex(string checkCode, int AccountID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockBox EndGetMaxStockBox_Ex(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockBox ChangeBoxBarCode(string strBarCode, ref string ErrMsg)
        {
            try
            {
                return _client.ChangeBoxBarCode(strBarCode, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginChangeBoxBarCode(string strBarCode, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockBox EndChangeBoxBarCode(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }


        public bool ModifyBoxBarCode(string checkCode, int AccountID, string OldBarCode, string NewBarCode, ref string ErrMsg)
        {
            try
            {
                return _client.ModifyBoxBarCode(checkCode, AccountID, OldBarCode, NewBarCode, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public IAsyncResult BeginModifyBoxBarCode(string checkCode, int AccountID, string OldBarCode, string NewBarCode, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndModifyBoxBarCode(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }



        public StockBox GetStocBoxkEntityByIsUnPrint(string checkCode, int AccountID, ref string ErrMsg)
        {
            try
            {
                return _client.GetStocBoxkEntityByIsUnPrint(checkCode, AccountID, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginGetStocBoxkEntityByIsUnPrint(string checkCode, int AccountID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockBox EndGetStocBoxkEntityByIsUnPrint(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }
    }
}
