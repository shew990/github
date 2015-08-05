using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.Common
 *   文件名:     TestHelper
 *   说明:       
 *   创建时间:   2014/9/10 23:25:14
 *   作者:       liende
 */
namespace SCG.SINOStock.Common
{
    public class TestHelper
    {
        /// <summary>
        /// 将字符串写入TXT(追加)
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strData"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static bool WriteDataLine(string strPath, string strData, ref string ErrMsg)
        {
            StreamWriter sw = File.AppendText(strPath);
            try
            {
                sw.WriteLine(strData);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
            return true;
        }
    }
}
