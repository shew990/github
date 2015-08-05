using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     SINOStockServiceProxy
 *   说明:       
 *   创建时间:   2014/2/26 17:04:08
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public partial class SINOStockServiceProxy
    {
        public bool AddTray(string checkCode, int AccountID, Tray entity, int[] StockLotIDs, bool isQiangDa, ref int Qty, ref string[] Boxs, ref string ErrMsg)
        {
            try
            {
                return _client.AddTray(checkCode, AccountID, entity, StockLotIDs, isQiangDa, ref Qty, ref  Boxs, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public IAsyncResult BeginAddTray(string checkCode, int AccountID, Tray entity, int[] StockLotIDs, bool isQiangDa, ref int Qty, ref string[] Boxs, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndAddTray(ref int Qty, ref string[] Boxs, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public Tray GetTrayByBarCode(string checkCode, int AccountID, string BarCode, ref int Qty, ref string ProModel, ref string ErrMsg)
        {
            try
            {
                return _client.GetTrayByBarCode(checkCode, AccountID, BarCode, ref Qty, ref ProModel, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginGetTrayByBarCode(string checkCode, int AccountID, string BarCode, ref int Qty, ref string ProModel, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public Tray EndGetTrayByBarCode(ref int Qty, ref string ProModel, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public string GetTrayMaxBarCode(string checkCode, int AccountID, int LogID, ref string ErrMsg)
        {
            try
            {
                return _client.GetTrayMaxBarCode(checkCode, AccountID, LogID, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = string.Empty;
                return string.Empty;
            }
        }

        public IAsyncResult BeginGetTrayMaxBarCode(string checkCode, int AccountID, int LogID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public string EndGetTrayMaxBarCode(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public Tray[] GetTrayListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg)
        {
            try
            {
                return _client.GetTrayListByDt(checkCode, AccountID, StartDt, EndDt, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }

        public IAsyncResult BeginGetTrayListByDt(string checkCode, int AccountID, DateTime StartDt, DateTime EndDt, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public Tray[] EndGetTrayListByDt(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public Tray GetMaxTray(string checkCode, int AccountID, int LotID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetMaxTray(string checkCode, int AccountID, int LotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public Tray EndGetMaxTray(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool ModifyTray(string checkCode, int AccountID, Tray entity, int StockLotID, ref int Qty, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginModifyTray(string checkCode, int AccountID, Tray entity, int StockLotID, ref int Qty, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndModifyTray(ref int Qty, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }
        public bool ChangeTryBarCode(string strBarCode, ref string ErrMsg)
        {
            try
            {
                return _client.ChangeTryBarCode(strBarCode, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public IAsyncResult BeginChangeTryBarCode(string strBarCode, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndChangeTryBarCode(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }


    }
}
