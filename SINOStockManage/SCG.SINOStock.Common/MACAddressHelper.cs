using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.Common
 *   文件名:     MACAddressHelper
 *   说明:       
 *   创建时间:   2014/3/18 0:14:50
 *   作者:       liende
 */
namespace SCG.SINOStock.Common
{
    public class MACAddressHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetMACAddress()
        {
            string MoAddress = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                    MoAddress = mo["MacAddress"].ToString();
                mo.Dispose();
            }
            return MoAddress;
        }
    }
}
