using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

/**
 *   命名空间:   SCG.SINOStock.WCFService
 *   文件名:     SINOStockServiceProxy
 *   说明:       WCF客户端代理类
 *   创建时间:   2013/12/29 21:34:48
 *   作者:       liende
 */
namespace SCG.SINOStock.WCFService
{
    public partial class SINOStockServiceProxy : ISINOStockServiceProxy
    {
        private readonly SINOStockServiceClient _client;
        public SINOStockServiceProxy()
        {
            _client = new SINOStockServiceClient();
        }

        #region Active Call Count

        private int _activeCallCount;
        public int ActiveCallCount
        {
            get { return _activeCallCount; }
        }

        private void DecrementCallCount()
        {
            Interlocked.Decrement(ref _activeCallCount);
            OnPropertyChanged("ActiveCallCount");
        }

        private void IncrementCallCount()
        {
            Interlocked.Increment(ref _activeCallCount);
            OnPropertyChanged("ActiveCallCount");
        }

        #endregion

        #region INotifyPropertyChanged Interface implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion







        public Account[] GetAccountList(string checkCode, int AccountID, Dictionary<string, string> queryList, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }





        public void DoWork()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDoWork(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public void EndDoWork(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public Account GetAccountById()
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetAccountById(AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public Account EndGetAccountById(IAsyncResult result)
        {
            throw new NotImplementedException();
        }









        public StockLotOut GetStockLotOutEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref FormWork formWork, ref int OutCount, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotOutEntityByLotNo_Out(string checkCode, int AccountID, string LotNo, ref FormWork formWork, ref int OutCount, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockLotOut EndGetStockLotOutEntityByLotNo_Out(ref FormWork formWork, ref int OutCount, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockLotOut GetStockLotOutEntityByLotNo_Out_NO(string checkCode, int AccountID, string LotNo, int Qty, string ProModel, ref FormWork formWork, ref int OutCount, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotOutEntityByLotNo_Out_NO(string checkCode, int AccountID, string LotNo, int Qty, string ProModel, ref FormWork formWork, ref int OutCount, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockLotOut EndGetStockLotOutEntityByLotNo_Out_NO(ref FormWork formWork, ref int OutCount, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool EndStockLotOut(string checkCode, int AccountID, int StockLotID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginEndStockLotOut(string checkCode, int AccountID, int StockLotID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndEndStockLotOut(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool DeleteStockLotOut(string checkCode, int AccountID, int StockLotOutID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDeleteStockLotOut(string checkCode, int AccountID, int StockLotOutID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndDeleteStockLotOut(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool CheckLotNo(string checkCode, int AccountID, string LotNo, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginCheckLotNo(string checkCode, int AccountID, string LotNo, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndCheckLotNo(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public string[] GetNoStockOutGlass(string checkCode, int AccountID, string LotNo, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetNoStockOutGlass(string checkCode, int AccountID, string LotNo, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public string[] EndGetNoStockOutGlass(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockLotOut[] GetStockLotOutList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockLotOutList(string checkCode, int AccountID, Dictionary<string, string> queryList, int PageCount, int PageIndex, ref int listCount, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockLotOut[] EndGetStockLotOutList(ref int listCount, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }





        public StockOutDetail[] GetStockOutDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockOutDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockOutDetail[] EndGetStockOutDetailList(ref int CountQty, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool AddStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, bool IsCheck, ref int QtyCount, ref StockOutDetail entityID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, bool IsCheck, ref int QtyCount, ref StockOutDetail entityID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndAddStockOutDetail(ref int QtyCount, ref StockOutDetail entityID, ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockOutDetail GetStockoutDetailByGlassID(string checkCode, int AccountID, string strGlassID, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetStockoutDetailByGlassID(string checkCode, int AccountID, string strGlassID, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockOutDetail EndGetStockoutDetailByGlassID(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool ModifyStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginModifyStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndModifyStockOutDetail(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool DeleteStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginDeleteStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndDeleteStockOutDetail(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool AddStockProDic(StockProDic entity, ref string ErrMsg)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAddStockProDic(StockProDic entity, ref string ErrMsg, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public bool EndAddStockProDic(ref string ErrMsg, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public StockProDic GetProDicByProAndLotID(int LotID, string ProModel)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginGetProDicByProAndLotID(int LotID, string ProModel, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public StockProDic EndGetProDicByProAndLotID(IAsyncResult result)
        {
            throw new NotImplementedException();
        }




















































        #region ISINOStockService 成员


      

        #endregion

        #region ISINOStockService 成员


    

        #endregion

        #region ISINOStockService 成员



        #endregion

        #region ISINOStockService 成员


      
        #endregion

        #region ISINOStockService 成员


      

        #endregion
    }
}
