using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     TrayRule
 *   说明:       
 *   创建时间:   2014/2/26 17:10:54
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class TrayRule : RuleBase
    {
        public bool Add_(Tray entity, int[] StockLotIDs, bool isQiangDa, ref int Qty, ref string[] Boxs, ref string ErrMsg)
        {

            return Proxy.AddTray(CurrentAccount.CheckCode, CurrentAccount.ID, entity, StockLotIDs, isQiangDa, ref Qty, ref Boxs, ref ErrMsg);

        }
        public Tray GetTrayByBarCode(string BarCode, ref int Qty, ref string ProMode, ref string ErrMsg)
        {

            return Proxy.GetTrayByBarCode(CurrentAccount.CheckCode, CurrentAccount.ID, BarCode, ref Qty, ref ProMode, ref ErrMsg);

        }
        public string GetTrayMaxBarCode(int LotID, ref string ErrMsg)
        {

            return Proxy.GetTrayMaxBarCode(CurrentAccount.CheckCode, CurrentAccount.ID, LotID, ref ErrMsg);

        }

        public List<Tray> GetTrayListByDt(DateTime StartDt, DateTime EndDt, ref string ErrMsg)
        {

            return Proxy.GetTrayListByDt(CurrentAccount.CheckCode, CurrentAccount.ID, StartDt, EndDt, ref ErrMsg).ToList();

        }

        public bool ChangeTryBarCode(string strBarCode, ref string ErrMsg)
        {
            try
            {
                return Proxy.ChangeTryBarCode(strBarCode, ref ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
    }
}
