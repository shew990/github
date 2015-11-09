using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     PrintHelp
 *   说明:       
 *   创建时间:   2014/2/26 18:04:51
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class PrintHelp
    {
        #region DLL
        [DllImport("Trace.dll")]
        public static extern void setup(int a, int b, int c, int d, int e, int f);

        [DllImport("Trace.dll")]
        public static extern void setbaudrate(int n);

        [DllImport("Trace.dll")]
        public static extern void SetTimeOutTicks(int ms);

        // n = 0 → LPT1 
        // n = 1 → COM1 
        // n = 2 → COM2 
        // n = 3 → COM3 
        // n = 4 → COM4 
        // n = 5 → LPT2 
        // n = 6 → USB         
        [DllImport("Trace.dll")]
        public static extern void openport(
            [MarshalAs(UnmanagedType.LPStr)]
            string n);

        [DllImport("Trace.dll")]
        public static extern void closeport();


        [DllImport("Trace.dll")]
        public static extern void sendcommand(
            [MarshalAs(UnmanagedType.LPArray)]
            byte[] command);

        [DllImport("Trace.dll")]
        public static extern void sendcommand(
            [MarshalAs(UnmanagedType.LPStr)]
            string command);

        // 1 -> Success, 0 -> Fail
        [DllImport("Trace.dll")]
        public static extern int isready();


        [DllImport("Trace.dll")]
        public static extern void intloadimage(
            [MarshalAs(UnmanagedType.LPStr)]
            string filename,
            [MarshalAs(UnmanagedType.LPStr)]
            string image_name,
            [MarshalAs(UnmanagedType.LPStr)]
            string image_type);

        [DllImport("Trace.dll")]
        public static extern void extloadimage(
            [MarshalAs(UnmanagedType.LPStr)]
            string filename,
            [MarshalAs(UnmanagedType.LPStr)]
            string image_name,
            [MarshalAs(UnmanagedType.LPStr)]
            string image_type);

        // 1 -> Success, 0 -> Fail
        [DllImport("Trace.dll")]
        public static extern int ecTextOut(int x, int y, int b,
            [MarshalAs(UnmanagedType.LPStr)]
            string c,
            [MarshalAs(UnmanagedType.LPStr)]
            string d);

        // 1 -> Success, 0 -> Fail
        [DllImport("Trace.dll")]
        public static extern int ecTextOutR(int x, int y, int b,
            [MarshalAs(UnmanagedType.LPStr)]
            string c,
            [MarshalAs(UnmanagedType.LPStr)]
            string d, int e, int f, int g);

        // 1 -> Success, 0 -> Fail
        [DllImport("Trace.dll")]
        public static extern int ecTextDownLoad(int b, string c, string d, int e, int f, int g,
            [MarshalAs(UnmanagedType.LPStr)]
            string name);

        // 1 -> Success, 0 -> Fail
        [DllImport("Trace.dll")]
        public static extern int putimage(int x, int y,
            [MarshalAs(UnmanagedType.LPStr)]
            string filename, int degree);

        // 1 -> Success, 0 -> Fail
        [DllImport("Trace.dll")]
        public static extern int downloadimage(
            [MarshalAs(UnmanagedType.LPStr)]
            string filename, int degree,
            [MarshalAs(UnmanagedType.LPStr)]
            string name);

        [DllImport("Trace.dll")]
        public static extern void SelectUsbPrinter(
            [MarshalAs(UnmanagedType.LPStr)]
            string ID);

        [DllImport("Trace.dll")]
        public static extern void SelectUsbPortNumber(int portnumber);


        [DllImport("Trace.dll")]
        public static extern void readusb(
            [MarshalAs(UnmanagedType.LPArray)]
            byte[] buff,
            ref int length);
        #endregion


        public static bool PrintStockBox(string[] strPrintArray, ref string ErrMsg)
        {
            openport("6"); //Ex:USB
            try
            {
                string str = string.Format(@"^Q100,3
                                            ^W100
                                            ^H5
                                            ^P1
                                            ^S2
                                            ^AD
                                            ^C1
                                            ^R0
                                            ~Q+0
                                            ^O0
                                            ^D0
                                            ^E12
                                            ~R200
                                            ^L
                                            Dy2-me-dd
                                            Th:m:s
                                            AZ1,32,30,2,2,0,0,型号:{0}
                                            AZ1,32,80,2,2,0,0,日期:{1}
                                            AZ1,32,130,2,2,0,0,BOX ID:{2}
                                            AZ1,32,180,2,2,0,0,GLASS ID:
                                            BQ,474,82,2,6,80,0,1,{2}
                                            AC,32,254,1,1,0,0,{3}
                                            AC,277,254,1,1,0,0,{4}
                                            AC,522,254,1,1,0,0,{5}
                                            AC,32,300,1,1,0,0,{6}
                                            AC,277,300,1,1,0,0,{7}
                                            AC,522,300,1,1,0,0,{8}
                                            AC,32,350,1,1,0,0,{9}
                                            AC,277,350,1,1,0,0,{10}
                                            AC,522,350,1,1,0,0,{11}
                                            AC,32,404,1,1,0,0,{12}
                                            AC,277,404,1,1,0,0,{13}
                                            AC,522,404,1,1,0,0,{14}
                                            AC,32,450,1,1,0,0,{15}
                                            AC,277,450,1,1,0,0,{16}
                                            AC,522,450,1,1,0,0,{17}
                                            AC,32,496,1,1,0,0,{18}
                                            AC,277,496,1,1,0,0,{19}
                                            AC,522,496,1,1,0,0,{20}
                                            AC,32,546,1,1,0,0,{21}
                                            AC,277,546,1,1,0,0,{22}
                                            AC,522,546,1,1,0,0,{23}
                                            AC,32,600,1,1,0,0,{24}
                                            AC,277,600,1,1,0,0,{25}
                                            AC,522,600,1,1,0,0,{26}
                                            AC,32,656,1,1,0,0,{27}
                                            AC,277,656,1,1,0,0,{28}
                                            AC,522,656,1,1,0,0,{29}
                                            AC,32,710,1,1,0,0,{30}
                                            AC,277,710,1,1,0,0,{31}
                                            AC,522,710,1,1,0,0,{32}
                                            E
                            ", strPrintArray[0], strPrintArray[1], strPrintArray[2], strPrintArray[3], strPrintArray[4], strPrintArray[5], strPrintArray[6], strPrintArray[7], strPrintArray[8], strPrintArray[9], strPrintArray[10], strPrintArray[11], strPrintArray[12], strPrintArray[13], strPrintArray[14], strPrintArray[15], strPrintArray[16], strPrintArray[17], strPrintArray[18], strPrintArray[19], strPrintArray[20], strPrintArray[21], strPrintArray[22], strPrintArray[23], strPrintArray[24], strPrintArray[25], strPrintArray[26], strPrintArray[27], strPrintArray[28], strPrintArray[29], strPrintArray[30], strPrintArray[31], strPrintArray[32]);

                // string[] strarray = str.Split(new char[]{);
                string[] tmpstrArray = StrSplitHelp.StringSplit(str, "\r\n");

                foreach (string item in tmpstrArray)
                {
                    byte[] by = System.Text.Encoding.Default.GetBytes(item.Trim());
                    sendcommand(by);

                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public static bool PrintStockTray(string[] strPrintArray, ref string ErrMsg)
        {
            openport("6"); //Ex:USB
            try
            {
                string str = string.Format(@"^Q100,3
                                            ^W100
                                            ^H5
                                            ^P1
                                            ^S2
                                            ^AD
                                            ^C1
                                            ^R0
                                            ~Q+0
                                            ^O0
                                            ^D0
                                            ^E12
                                            ~R200
                                            ^L
                                            Dy2-me-dd
                                            Th:m:s
                                            AZ1,45,100,3,3,0,0,型号：{0}
                                            AZ1,45,185,3,3,0,0,QTY：{1}PCS
                                            AZ1,45,270,3,3,0,0,托号ID：{2}
                                            E
                            ", strPrintArray[0], strPrintArray[1], strPrintArray[2]);

                // string[] strarray = str.Split(new char[]{);
                string[] tmpstrArray = StrSplitHelp.StringSplit(str, "\r\n");

                foreach (string item in tmpstrArray)
                {
                    byte[] by = System.Text.Encoding.Default.GetBytes(item.Trim());
                    sendcommand(by);

                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        #region 废弃   2014年3月24日 00:22:32
        //public static bool PrintStockBox_TXT(PrintItemType[] strPrintArray, ref string ErrMsg)
        //{
        //    //  string strTest = string.Empty;
        //    openport("6"); //Ex:USB
        //    try
        //    {
        //        StreamReader sr = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\BOXFormat.txt", Encoding.GetEncoding("GB2312"));
        //        string strCMD = sr.ReadToEnd();
        //        strCMD = strCMD.Replace("[型号]", strPrintArray[0].Value);
        //        strCMD = strCMD.Replace("[日期]", strPrintArray[1].Value);
        //        strCMD = strCMD.Replace("[BOXID]", strPrintArray[2].Value);
        //        strCMD = strCMD.Replace("[条码]", strPrintArray[2].Value);
        //        //  string[] tmpArray = StrSplitHelp.StringSplit(strCMD, "[GLASSID]");
        //        //  string[] strarray = str.Split(new char[]{);
        //        string[] tmpstrArray = StrSplitHelp.StringSplit(strCMD, "\r\n");

        //        int i = 3;
        //        foreach (string item in tmpstrArray)
        //        {

        //            string strTmp = item;
        //            //if (i >= strPrintArray.Length)
        //            //{
        //            //    strTmp = strTmp.Replace("[GLASSID]", " ");
        //            //    strTmp = strTmp.Replace("[LOTNO]", " ");
        //            //   // break;
        //            //}
        //            //else
        //            //{
        //            if (strTmp.Contains("[GLASSID]") || strTmp.Contains("[LOTNO]"))
        //            {
        //                if (strPrintArray[i].Type == PrintTypeEnum.LOTNO && strTmp.Contains("[LOTNO]"))
        //                {
        //                    strTmp = strTmp.Replace("[LOTNO]", strPrintArray[i].Value);
        //                    i++;
        //                }
        //                else
        //                    if (strPrintArray[i].Type == PrintTypeEnum.GlassID && strTmp.Contains("[GLASSID]"))
        //                    {
        //                        strTmp = strTmp.Replace("[GLASSID]", strPrintArray[i].Value);
        //                        i++;
        //                    }
        //            }
        //            //   }


        //            byte[] by = System.Text.Encoding.Default.GetBytes(strTmp.Trim());
        //            sendcommand(by);
        //            //  strTest += strTmp + "\r\n";
        //        }
        //        closeport();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrMsg = ex.Message;
        //        return false;
        //    }
        //}
        #endregion
        public static bool PrintStockBox_TXT(string strBox, string strProModel, PrintHelperEx[] strPrintArray, ref string ErrMsg)
        {
            //  string strTest = string.Empty;
            openport("6"); //Ex:USB
            try
            {
                StreamReader sr = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\BOXFormat.txt", Encoding.GetEncoding("GB2312"));
                string strCMD = sr.ReadToEnd();
                strCMD = strCMD.Replace("[型号]", strProModel);
                strCMD = strCMD.Replace("[日期]", DateTime.Now.ToString("yyyyMMdd"));
                strCMD = strCMD.Replace("[BOXID]", strBox);
                strCMD = strCMD.Replace("[条码]", strBox);
                //  string[] tmpArray = StrSplitHelp.StringSplit(strCMD, "[GLASSID]");
                //  string[] strarray = str.Split(new char[]{);

                //替换模版中LOTNO
                for (int k = 0; k < 10; k++)
                {
                    if (k < strPrintArray.Length)
                    {
                        strCMD = strCMD.Replace("[LOTNO" + (k + 1) + "]", strPrintArray[k].Value);
                        if (strPrintArray[k].Item.Count() > 0)
                        {
                            for (int j = 0; j < 40; j++)
                            {
                                if (j < strPrintArray[k].Item.Count())
                                {
                                    Regex r = new Regex("(\\[GLASSID" + (k + 1) + "\\]){1}");
                                    strCMD = r.Replace(strCMD, strPrintArray[k].Item[j].GlassID, 1, 0);
                                }
                                else
                                {
                                    strCMD = strCMD.Replace("[GLASSID" + (k + 1) + "]", " ");
                                    break;
                                }
                            }
                        }
                        else
                            strCMD = strCMD.Replace("[GLASSID" + (k + 1) + "]", " ");
                    }
                    else
                    {
                        strCMD = strCMD.Replace("[LOTNO" + (k + 1) + "]", " ");
                        strCMD = strCMD.Replace("[GLASSID" + (k + 1) + "]", " ");
                    }


                }


                string[] tmpstrArray = StrSplitHelp.StringSplit(strCMD, "\r\n");
                foreach (string item in tmpstrArray)
                {
                    string strTmp = item;

                    //TODO:打印到文件
                    // Common.TestHelper.WriteDataLine("D:\\testBox.txt", strTmp.Trim(), ref ErrMsg);
                    byte[] by = System.Text.Encoding.Default.GetBytes(strTmp.Trim());
                    sendcommand(by);
                    //  strTest += strTmp + "\r\n";
                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public static bool PrintStockTray_TXT(string[] strPrintArray, ref string ErrMsg)
        {
            openport("6"); //Ex:USB
            try
            {
                StreamReader sr = new StreamReader(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) + "\\TrayFormat.txt", Encoding.GetEncoding("GB2312"));
                string strCMD = sr.ReadToEnd();
                strCMD = strCMD.Replace("[型号]", strPrintArray[0]);
                strCMD = strCMD.Replace("[数量]", strPrintArray[1]);
                strCMD = strCMD.Replace("[托号]", strPrintArray[2]);
                // string[] strarray = str.Split(new char[]{);
                string[] tmpstrArray = StrSplitHelp.StringSplit(strCMD, "\r\n");
                int i = 3;
                foreach (string item in tmpstrArray)
                {
                    string strTmp = item;
                    if (strTmp.Contains("[BOXID]"))
                    {
                        strTmp = strTmp.Replace("[BOXID]", strPrintArray[i]);
                        i++;
                    }
                    //TODO:打印到文件
                    //   Common.TestHelper.WriteDataLine("D:\\testTry.txt", strTmp.Trim(), ref ErrMsg);
                    byte[] by = System.Text.Encoding.Default.GetBytes(strTmp.Trim());
                    sendcommand(by);
                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public static bool PrintStockTray_EXT(string[] strPrintTrayArray, ref string ErrMsg)
        {
            //  string strTest = string.Empty;
            openport("6"); //Ex:USB
            try
            {
                string name = string.Empty;
                string strPrintLotNO = string.Empty;
                string strLogoName = string.Empty;
                try
                {
                    name = ConfigurationManager.AppSettings["CoLtdName"].ToString();
                    strLogoName = ConfigurationManager.AppSettings["LogoName"].ToString();
                }
                catch (Exception ex)
                {
                    ErrMsg = "打印未成功：配置文件的配置不正确，请检查";
                    return false;
                }


                string strCMD = @"^Q80,3
^W100
^H5
^P1
^S2
^AD
^C1
^R8
~Q+8
^O0
^D0
^E12
~R200
^XSET,ROTATION,0
^L
Dy2-me-dd
Th:m:s
Dy2-me-dd
Th:m:s
Lo,14,125,758,128
Lo,24,583,768,586
";


                string printProModel = string.Format("AZ2,18,20,1,1,0,0,型号:{0}\r\n", strPrintTrayArray[0]);
                string printQty = string.Format("AZ2,18,54,1,1,0,0,数量:{0}PCS\r\n", strPrintTrayArray[1]);
                string printTrayID = string.Format("AZ2,4,88,1,1,0,0,PALLETID:{0}\r\n", strPrintTrayArray[2]);
                string printTrayIDBarCode = string.Format("BQ,346,59,2,6,60,0,0,{0}\r\n", strPrintTrayArray[2]);
                string printName = string.Format("AZ2,430,599,1,1,0,0,{0}\r\n", name);
                //string printImage = string.Format("Y658,26,{0}\r\n", strLogoName);
                string printImage = string.Format("Y668,36,{0}\r\n", strLogoName);

                StringBuilder sbTmp = new StringBuilder().Append(printProModel).Append(printQty).Append(printTrayID).Append(printTrayIDBarCode).Append(printName).Append(printImage);

                strCMD += sbTmp.ToString();

                string strEnd = @"E
";
                StringBuilder sb = new StringBuilder();



                int printX = 15;//初始坐标X
                int printY = 134;//初始坐标Y

                int iCurrentX = printX;
                int iCurrentY = printY;

                int indexX = 270;//每个GlassID X轴间隔
                int indexY = 21;//每个GlassID Y轴间隔

                int i = 0;
                List<string> lstTmp = strPrintTrayArray.ToList();
                if (lstTmp.Count > 3)
                {
                    lstTmp.RemoveAt(0);
                    lstTmp.RemoveAt(0);
                    lstTmp.RemoveAt(0);
                }
                strPrintTrayArray = lstTmp.ToArray();
                foreach (var item in strPrintTrayArray)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    if (i == 20)//Y轴只能打下20
                    {
                        iCurrentY = printY;
                        iCurrentX += indexX;
                        i = 0;
                    }

                    sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, item));
                    iCurrentY += indexY;
                    i++;

                }



                string[] tmpstrArray = StrSplitHelp.StringSplit(strCMD + sb.ToString() + strEnd, "\r\n");
                foreach (string item in tmpstrArray)
                {
                    string strTmp = item;

                    //TODO:打印到文件
                    Common.TestHelper.WriteDataLine("D:\\testBox.txt", strTmp.Trim(), ref ErrMsg);
                    byte[] by = System.Text.Encoding.Default.GetBytes(strTmp.Trim());
                    sendcommand(by);
                    //  strTest += strTmp + "\r\n";
                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }



        #region 修改样式标签2015年4月14日13:18:22
        public static bool PrintStockBox_EXT(string strBox, string strProModel, PrintHelperEx[] strPrintArray, ref string ErrMsg)
        {
            //  string strTest = string.Empty;
            openport("6"); //Ex:USB
            try
            {
                string name = string.Empty;
                string strPrintLotNO = string.Empty;
                string strLogoName = string.Empty;
                try
                {
                    name = ConfigurationManager.AppSettings["CoLtdName"].ToString();
                    strPrintLotNO = ConfigurationManager.AppSettings["IsPrintLOTNO"].ToString();
                    strLogoName = ConfigurationManager.AppSettings["LogoName"].ToString();
                }
                catch (Exception ex)
                {
                    ErrMsg = "打印未成功：配置文件的配置不正确，请检查";
                    return false;
                }
                bool isPrintLotNO = strPrintLotNO.ToUpper().Equals("YES");



                string strCMD = @"^Q80,3
^W100
^H5
^P1
^S2
^AD
^C1
^R8
~Q+8
^O0
^D0
^E12
~R200
^XSET,ROTATION,0
^L
Dy2-me-dd
Th:m:s
Dy2-me-dd
Th:m:s
Lo,14,125,758,128
Lo,24,583,768,586
";


                string printProModel = string.Format("AZ2,18,20,1,1,0,0,型号:{0}\r\n", strProModel);
                string printDate = string.Format("AZ2,18,54,1,1,0,0,日期:{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd"));
                string printBOXID = string.Format("AZ2,4,88,1,1,0,0,BOXID:{0}\r\n", strBox);
                //string printBOXIDBarCode = string.Format("BQ,346,59,2,6,60,0,0,{0}\r\n", strBox);
                string printBOXIDBarCode = string.Format("BQ,320,57,2,6,60,0,0,{0}\r\n", strBox);

                //string printName = string.Format("AZ2,430,599,1,1,0,0,{0}\r\n", name);
                string printName = string.Format("AZ2,15,599,1,1,0,0,{0}\r\n", name);

                //string printImage = string.Format("Y658,26,{0}\r\n", strLogoName);
                string printImage = string.Format("Y668,36,{0}\r\n", strLogoName);

                StringBuilder sbTmp = new StringBuilder().Append(printProModel).Append(printDate).Append(printBOXID).Append(printBOXIDBarCode).Append(printName).Append(printImage);

                strCMD += sbTmp.ToString();

                string strEnd = @"E
";
                StringBuilder sb = new StringBuilder();



                int printX = 15;//初始坐标X
                int printY = 134;//初始坐标Y

                int iCurrentX = printX;
                int iCurrentY = printY;

                int indexX = 270;//每个GlassID X轴间隔
                int indexY = 21;//每个GlassID Y轴间隔

                int i = 0;
                int MaxRowCount = 21;//Y轴最大行数
                bool IsModel2 = false;//是否使用4列模版
                int j = 0;//记录数（换行算两条）
                //先算记录数（换行算两条）
                foreach (var item in strPrintArray)
                {
                    if (isPrintLotNO)
                    {
                        string temp = CutStr(26, item.Value);//超过26位换行
                        if (temp.Equals(item.Value))
                        {
                            j++;
                        }
                        else//LOTNO换行
                        {
                            j += 2;
                        }
                    }
                    foreach (var detail in item.Item)
                    {
                        j++;
                    }
                }
                if (j > 63)
                {
                    indexX = 200;//每个GlassID X轴间隔
                    IsModel2 = true;
                }
                foreach (var item in strPrintArray)
                {
                    if (i == MaxRowCount)//Y轴只能打下20
                    {
                        iCurrentY = printY;
                        j++;
                        iCurrentX += indexX;
                        i = 0;
                    }

                    //sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, " "));
                    //iCurrentY += indexY;
                    //i++;

                    if (isPrintLotNO)
                    {
                        if (i == MaxRowCount)//Y轴只能打下20
                        {
                            iCurrentY = printY;
                            iCurrentX += indexX;
                            i = 0;
                        }
                        string temp = CutStr(26, item.Value);//超过26位换行
                        if (temp.Equals(item.Value))
                        {
                            sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, item.Value));
                            iCurrentY += indexY;
                            i++;
                        }
                        else//LOTNO换行
                        {
                            string str1 = temp;
                            string str2 = item.Value.Replace(temp, "");
                            sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, str1));
                            iCurrentY += indexY;
                            i++;

                            if (i == MaxRowCount)//Y轴只能打下20
                            {
                                iCurrentY = printY;
                                iCurrentX += indexX;
                                i = 0;
                            }

                            sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, str2));
                            iCurrentY += indexY;
                            i++;
                        }
                    }
                    foreach (var detail in item.Item)
                    {
                        if (i == MaxRowCount)//Y轴只能打下20
                        {
                            iCurrentY = printY;
                            iCurrentX += indexX;
                            i = 0;
                        }
                        if (!IsModel2 && CutStr(17, detail.GlassID).Equals(detail.GlassID))//glassid超过17位或记录数超过63，换更小字体
                            sb.Append(string.Format("AB,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, detail.GlassID));
                        else
                            sb.Append(string.Format("AA,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, detail.GlassID));

                        iCurrentY += indexY;
                        i++;
                    }

                }



                string[] tmpstrArray = StrSplitHelp.StringSplit(strCMD + sb.ToString() + strEnd, "\r\n");
                foreach (string item in tmpstrArray)
                {
                    string strTmp = item;

                    //TODO:打印到文件
                    //Common.TestHelper.WriteDataLine("D:\\testBox.txt", strTmp.Trim(), ref ErrMsg);
                    byte[] by = System.Text.Encoding.Default.GetBytes(strTmp.Trim());
                    sendcommand(by);
                    //  strTest += strTmp + "\r\n";
                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBox"></param>
        /// <param name="isModify">是否修改过</param>
        /// <param name="strProModel"></param>
        /// <param name="strPrintArray"></param>
        /// <param name="ErrMsg"></param>
        /// <returns></returns>
        public static bool PrintStockBoxAgain_EXT(string strBox, bool isModify, string strProModel, PrintHelperEx[] strPrintArray, ref string ErrMsg)
        {
            //  string strTest = string.Empty;
            openport("6"); //Ex:USB
            try
            {
                string name = string.Empty;
                string strPrintLotNO = string.Empty;
                string strLogoName = string.Empty;
                try
                {
                    name = ConfigurationManager.AppSettings["CoLtdName"].ToString();
                    strPrintLotNO = ConfigurationManager.AppSettings["IsPrintLOTNO"].ToString();
                    strLogoName = ConfigurationManager.AppSettings["LogoName"].ToString();
                }
                catch (Exception ex)
                {
                    ErrMsg = "打印未成功：配置文件的配置不正确，请检查";
                    return false;
                }
                bool isPrintLotNO = strPrintLotNO.ToUpper().Equals("YES");



                string strCMD = @"^Q80,3
^W100
^H5
^P1
^S2
^AD
^C1
^R8
~Q+8
^O0
^D0
^E12
~R200
^XSET,ROTATION,0
^L
Dy2-me-dd
Th:m:s
Dy2-me-dd
Th:m:s
Lo,14,125,758,128
Lo,24,583,768,586
";


                string printProModel = string.Format("AZ2,18,20,1,1,0,0,型号:{0}\r\n", strProModel);
                string printDate = string.Format("AZ2,18,54,1,1,0,0,日期:{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd"));
                string printBOXID = "";
                if (isModify)
                    printBOXID = string.Format("AZ2,4,88,1,1,0,0,BOXID:{0}\r\n", strBox + "*");//明文带"*"，条码不带
                else
                    printBOXID = string.Format("AZ2,4,88,1,1,0,0,BOXID:{0}\r\n", strBox);

                //string printBOXIDBarCode = string.Format("BQ,346,59,2,6,60,0,0,{0}\r\n", strBox);
                string printBOXIDBarCode = string.Format("BQ,320,57,2,6,60,0,0,{0}\r\n", strBox);

                string printName = string.Format("AZ2,15,599,1,1,0,0,{0}\r\n", name);
                //string printImage = string.Format("Y658,26,{0}\r\n", strLogoName);
                string printImage = string.Format("Y668,36,{0}\r\n", strLogoName);

                StringBuilder sbTmp = new StringBuilder().Append(printProModel).Append(printDate).Append(printBOXID).Append(printBOXIDBarCode).Append(printName).Append(printImage);

                strCMD += sbTmp.ToString();

                string strEnd = @"E
";
                StringBuilder sb = new StringBuilder();



                int printX = 15;//初始坐标X
                int printY = 134;//初始坐标Y

                int iCurrentX = printX;
                int iCurrentY = printY;

                int indexX = 270;//每个GlassID X轴间隔
                int indexY = 21;//每个GlassID Y轴间隔

                int i = 0;
                int MaxRowCount = 21;//Y轴最大行数
                bool IsModel2 = false;//是否使用4列的模板
                int j = 0;//记录数（换行算两条）
                //先算记录数（换行算两条）
                foreach (var item in strPrintArray)
                {
                    if (isPrintLotNO)
                    {
                        string temp = CutStr(26, item.Value);//超过26位换行
                        if (temp.Equals(item.Value))
                        {
                            j++;
                        }
                        else//LOTNO换行
                        {
                            j += 2;
                        }
                    }
                    foreach (var detail in item.Item)
                    {
                        j++;
                    }
                }
                if (j > 63)//一张标签上记录超过63个，排4列
                {
                    indexX = 200;//每个GlassID X轴间隔
                    IsModel2 = true;

                }
                foreach (var item in strPrintArray)
                {
                    if (i == MaxRowCount)//Y轴只能打下20
                    {
                        iCurrentY = printY;
                        iCurrentX += indexX;
                        i = 0;
                    }

                    //sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, " "));
                    //iCurrentY += indexY;
                    //i++;

                    if (isPrintLotNO)
                    {
                        if (i == MaxRowCount)//Y轴只能打下20
                        {
                            iCurrentY = printY;
                            iCurrentX += indexX;
                            i = 0;
                        }
                        string temp = CutStr(26, item.Value);//超过26位换行
                        if (temp.Equals(item.Value))
                        {
                            sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, item.Value));
                            iCurrentY += indexY;
                            i++;
                        }
                        else//LOTNO换行
                        {
                            string str1 = temp;
                            string str2 = item.Value.Replace(temp, "");
                            sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, str1));
                            iCurrentY += indexY;
                            i++;

                            if (i == MaxRowCount)//Y轴只能打下20
                            {
                                iCurrentY = printY;
                                iCurrentX += indexX;
                                i = 0;
                            }

                            sb.Append(string.Format("AZ1,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, str2));
                            iCurrentY += indexY;
                            i++;
                        }
                    }
                    foreach (var detail in item.Item)
                    {
                        if (i == MaxRowCount)//Y轴只能打下20
                        {
                            iCurrentY = printY;
                            iCurrentX += indexX;
                            i = 0;
                        }
                        if (!IsModel2 && CutStr(17, detail.GlassID).Equals(detail.GlassID))//glassid超过17位或者记录数超过63，换更小字体
                            sb.Append(string.Format("AB,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, detail.GlassID));
                        else
                            sb.Append(string.Format("AA,{0},{1},1,1,0,0,{2}\r\n", iCurrentX, iCurrentY, detail.GlassID));

                        iCurrentY += indexY;
                        i++;
                    }

                }



                string[] tmpstrArray = StrSplitHelp.StringSplit(strCMD + sb.ToString() + strEnd, "\r\n");
                foreach (string item in tmpstrArray)
                {
                    string strTmp = item;

                    //TODO:打印到文件
                    //Common.TestHelper.WriteDataLine("D:\\testBox.txt", strTmp.Trim(), ref ErrMsg);
                    byte[] by = System.Text.Encoding.Default.GetBytes(strTmp.Trim());
                    sendcommand(by);
                    //  strTest += strTmp + "\r\n";
                }
                closeport();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        /// <param name="str">原字符串</param>
        /// <returns></returns>
        public static string CutStr(int maxLength, string str)
        {
            string temp = str;
            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= maxLength)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= maxLength - 3)
                {
                    return temp;
                }
            }
            return "";
        }

    }

    public class PrintHelperEx
    {
        public PrintHelperEx()
        {
            Item = new List<StockDetail>();
        }
        public string Value { get; set; }
        public List<StockDetail> Item { get; set; }
    }

    public class PrintItemType
    {
        public PrintTypeEnum Type { get; set; }
        public string Value { get; set; }
    }
    public enum PrintTypeEnum
    {
        GlassID, LOTNO
    }


}
