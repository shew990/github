using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     StockBoxRule
 *   说明:       
 *   创建时间:   2014/2/26 17:01:03
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class StockBoxRule : RuleBase
    {

        public StockBox GetMaxStockBox(int LotID, ref string ErrMsg)
        {
            return Proxy.GetMaxStockBox(CurrentAccount.CheckCode, CurrentAccount.ID, LotID, ref ErrMsg);

        }


        public bool ModifyStockBox(StockBox entity, int[] sdoIDList, int[] stockLotIDs, ref bool IsPrintTray, ref string ErrMsg)
        {

            return Proxy.ModifyStockBox(CurrentAccount.CheckCode, CurrentAccount.ID, entity, sdoIDList, stockLotIDs, ref IsPrintTray, ref ErrMsg);

        }

        public StockBox[] GetBoxListByDt(DateTime StartDt, DateTime EndDt, ref string ErrMsg)
        {
            return Proxy.GetBoxListByDt(CurrentAccount.CheckCode, CurrentAccount.ID, StartDt, EndDt, ref ErrMsg);
        }
        public StockBox GetStockBoxToBarCode(string BarCode, ref string ErrMsg)
        {
            return Proxy.GetStockBoxToBarCode(CurrentAccount.CheckCode, CurrentAccount.ID, BarCode, ref ErrMsg);
        }
        public StockBox GetMaxStockBox_Ex(ref string ErrMsg)
        {
            return Proxy.GetMaxStockBox_Ex(CurrentAccount.CheckCode, CurrentAccount.ID, ref ErrMsg);
        }
        public StockBox ChangeBoxBarCode(string BarCode, ref string ErrMsg)
        {
            return Proxy.ChangeBoxBarCode(BarCode, ref ErrMsg);
        }

        public bool ModifyBoxBarCode( string OldBarCode, string NewBarCode, ref string ErrMsg)
        {
            return Proxy.ModifyBoxBarCode(CurrentAccount.CheckCode, CurrentAccount.ID, OldBarCode, NewBarCode, ref ErrMsg);
        }



        public StockBox GetStocBoxkEntityByIsUnPrint( ref string ErrMsg)
        {
            return Proxy.GetStocBoxkEntityByIsUnPrint(CurrentAccount.CheckCode, CurrentAccount.ID, ref ErrMsg);
        }
    }
}
