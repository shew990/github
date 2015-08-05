using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ViewModels
 *   文件名:     TestReadTxt
 *   说明:       
 *   创建时间:   2014/5/7 10:10:30
 *   作者:       liende
 */
namespace SCG.SINOStock.ViewModels
{
    public class TestReadTxt
    {
        #region 字符串读取操作
        /// <summary>
        /// 读取TXT，返回字符串
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static bool ReadDataToString(string strPath, out string Result)
        {
            Result = string.Empty;
            StreamReader sr = new StreamReader(strPath, Encoding.GetEncoding("GB2312"));
            try
            {
                Result = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sr.Close();
            }
            return true;
        }
        /// <summary>
        /// 将字符串写入TXT
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strData"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static bool WriteData(string strPath, string strData, ref string ErrMsg)
        {
            StreamWriter sw = new StreamWriter(strPath, false, Encoding.GetEncoding("GB2312"));
            try
            {
                sw.Write(strData);
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
            finally
            {

                sw.Close();
            }
            return true;
        }

        #endregion

        #region 拆分字符串
        /// <summary>
        /// 根据字符串拆分字符串
        /// </summary>
        /// <param name="strSource">要拆分的字符串</param>
        /// <param name="strSplit">拆分符</param>
        /// <returns></returns>
        public static string[] StringSplit(string strSource, string strSplit)
        {
            string[] strtmp = new string[1];
            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[0] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        #endregion

        #region 采用递归将字符串分割成数组
        /// <summary>
        /// 采用递归将字符串分割成数组
        /// </summary>
        /// <param name="strSource"></param>
        /// <param name="strSplit"></param>
        /// <param name="attachArray"></param>
        /// <returns></returns>
        private static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
        {
            string[] strtmp = new string[attachArray.Length + 1];
            attachArray.CopyTo(strtmp, 0);


            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        #endregion
    }
}
