using SCG.SINOStock.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCG.SINOStock.WCF
{
    public partial class SINOStockService
    {
        public bool AddStockProDic(StockProDic entity, ref string ErrMsg)
        {

            if (entity == null)
            {
                ErrMsg = "数据为空";
                return false;
            }
            if (string.IsNullOrEmpty(entity.ProModel))
            {
                ErrMsg = "请选择产品型号";
                return false;
            }
            if (entity.PCSQty <= 0)
            {
                ErrMsg = "请填写数量";
                return false;
            }
            entity.CreateDt = DateTime.Now;
            try
            {
                entities.StockProDics.Add(entity);
                if (entities.SaveChanges() <= 0)
                {
                    ErrMsg = "添加失败";
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
        public StockProDic GetProDicByProAndLotID(int LotID, string ProModel)
        {
            //CheckingCheckCode(checkCode, AccountID, ref ErrMsg);
            return entities.StockProDics.FirstOrDefault(p => p.StockLotID == LotID && p.ProModel == ProModel);
        }
    }
}