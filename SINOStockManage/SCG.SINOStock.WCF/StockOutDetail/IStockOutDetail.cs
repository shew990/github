using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using SCG.SINOStock.Entities;

namespace SCG.SINOStock.WCF
{
   
    public partial interface ISINOStockService
    {
        /// <summary>
        /// 根据LotNot和当前登录者获取出库详细列表，如果AccountID为0，则查出该StockLotNo下的全部订单明细，否则查出操作人ID为AccountID的订单明细
        /// </summary>
        /// <param name="StockLotID">LotNo</param>
        /// <param name="AccountID">当前登录者</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>返回是否成功，true 成功，false 失败，如果失败则ErrMsg会带出失败信息</returns>
        [OperationContract]
        IList<StockOutDetail> GetStockOutDetailList(string checkCode, int cAccountID, int StockLotID, int AccountID, ref int CountQty, ref string ErrMsg);
        /// <summary>
        /// 添加出库详细，
        /// </summary>
        /// <param name="entity">需要添加的出库详细</param>
        /// <param name="IsCheck">是否验证关键字</param>
        /// <param name="QtyCount">该LotNO下实际添加的出库数</param>
        /// <param name="ErrMsg">错误信息</param>
        /// <returns>返回是否成功，true 成功，false 失败，如果成功，QtyCount会带出已出库数，失败则ErrMsg会带出失败信息</returns>
        [OperationContract]
        bool AddStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, bool IsCheck, ref int QtyCount, ref StockOutDetail entityID, ref string ErrMsg);

        [OperationContract]
        StockOutDetail GetStockoutDetailByGlassID(string checkCode, int AccountID, string strGlassID, ref string ErrMsg);

        [OperationContract]
        bool ModifyStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg);

        [OperationContract]
        bool DeleteStockOutDetail(string checkCode, int AccountID, StockOutDetail entity, ref string ErrMsg);
    }
}
