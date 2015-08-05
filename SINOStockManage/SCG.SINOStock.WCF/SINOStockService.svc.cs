using SCG.SINOStock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SCG.SINOStock.WCF
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SINOStockService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SINOStockService.svc 或 SINOStockService.svc.cs，然后开始调试。
    public partial class SINOStockService : ISINOStockService
    {
        protected SINOStockDBEntities entities;
        public SINOStockService()
        {
            entities = new SINOStockDBEntities();

        }
        public void DoWork()
        {
        }
        public Account GetAccountById()
        {
            return null;
        }
        private bool CheckingCheckCode(string checkCode, int AccountID, ref string ErrMsg)
        {
            try
            {
                Account entity = entities.Accounts.FirstOrDefault(p => p.ID == AccountID);

                if (entity.CheckCode != checkCode)
                {
                    ErrMsg = "您太久没有操作（登录超时），帐号已在另一电脑上登录！";
                    return false;
                }

                entity.LoginDt = DateTime.Now;
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
