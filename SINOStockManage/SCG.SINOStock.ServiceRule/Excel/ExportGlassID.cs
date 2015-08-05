using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule.Excel
 *   文件名:     ExportGlassID
 *   说明:       
 *   创建时间:   2014/3/4 22:20:07
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule.Excel
{
    public class ExportGlassID
    {
        public bool ExportGlassIDToExcel(List<StockDetail> lst, List<ExportRowHelper> ExportColumnNames, string fileName, ref string ErrMsg)
        {
            bool bSucc = true;

            try
            {
                NPOIHelper npoiHelper = new NPOIHelper();
                IWorkbook wb = new HSSFWorkbook();
                //创建表  
                ISheet sh = wb.CreateSheet("GlassID列表");
                for (int i = 0; i < ExportColumnNames.Count(); i++)
                {
                    sh.SetColumnWidth(i, 20 * 256);

                }

                int iRowIndex = 0;
                int iColIndex = 0;

                //设置标题

                sh.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, ExportColumnNames.Count()));  //合并单元格
                //CellRangeAddress（）该方法的参数次序是：开始行号，结束行号，开始列号，结束列号。

                IRow rowHead = sh.CreateRow(iRowIndex++);
                rowHead.Height = 40 * 20;
                ICell icellHead = rowHead.CreateCell(0);
                icellHead.CellStyle = npoiHelper.Getcellstyle(wb, NPOIHelper.stylexls.头);
                icellHead.SetCellValue("GlassID列表");

                rowHead = sh.CreateRow(iRowIndex++);
                rowHead.RowStyle = npoiHelper.Getcellstyle(wb, NPOIHelper.stylexls.默认);
                rowHead.Height = 20 * 20;


                ICellStyle tmptopstyle = npoiHelper.Getcellstyle(wb, NPOIHelper.stylexls.默认);
                tmptopstyle.BorderTop = BorderStyle.Thin;
                tmptopstyle.BorderLeft = BorderStyle.Thin;
                tmptopstyle.BorderBottom = BorderStyle.Thin;
                tmptopstyle.Alignment = HorizontalAlignment.Center;

                //设置列头
                ICell icelltop;
                foreach (var item in ExportColumnNames)
                {
                    icelltop = rowHead.CreateCell(iColIndex++);
                    icelltop.CellStyle = tmptopstyle;
                    icelltop.SetCellValue(item.ColumnValue);
                }
                #region 设置行数据
                ICellStyle tmpdatastyle = npoiHelper.Getcellstyle(wb, NPOIHelper.stylexls.默认);
                tmpdatastyle.BorderBottom = BorderStyle.Thin;
                tmpdatastyle.BorderLeft = BorderStyle.Thin;
                tmpdatastyle.BorderRight = BorderStyle.Thin;


                ICellStyle tmpRowStyle = npoiHelper.Getcellstyle(wb, NPOIHelper.stylexls.默认);
                
                foreach (StockDetail item in lst)//循环需要导出的数据
                {
                    IRow rowData = sh.CreateRow(iRowIndex++);
                    rowData.RowStyle = tmpRowStyle;
                    rowData.Height = 20 * 20;
                    iColIndex = 0;

                    Type t = item.GetType();
                    foreach (var column in ExportColumnNames)//循环需要导出的列
                    {
                        ICell icellData = rowData.CreateCell(iColIndex++);
                        icellData.CellStyle = tmpdatastyle;


                        if (column.ColumnName == "LotNoCreateAcccount")
                        {
                            if (item.StockLot != null )
                            {
                                icellData.SetCellValue(item.StockLot.CreateAccountName);
                            }
                            else
                                icellData.SetCellValue("");
                            continue;
                        }

                        foreach (var pro in t.GetProperties())//遍历实体属性
                        {
                            if (pro.Name == column.ColumnName)
                            {
                                object value = pro.GetValue(item, null);
                                string tmpcellvalue = "";
                                if (value != null)
                                {

                                    if (value.GetType() == typeof(DateTime))
                                    {
                                        DateTime dtvalue = (DateTime)value;
                                        tmpcellvalue = dtvalue.ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    else
                                        if (value.GetType() == typeof(StockLot))
                                        {
                                            StockLot lotValue = (StockLot)value;
                                            tmpcellvalue = lotValue.LotNo;
                                        }
                                        else
                                            if (value.GetType() == typeof(StockBox))
                                            {
                                                StockBox sb = value as StockBox;
                                                if (sb != null)
                                                    tmpcellvalue = sb.BarCode;
                                                else
                                                    tmpcellvalue = "";
                                            }
                                            else
                                                if (value.GetType() == typeof(Tray))
                                                {
                                                    Tray tray = value as Tray;
                                                    if (tray != null)
                                                        tmpcellvalue = tray.BarCode;
                                                    else
                                                        tmpcellvalue = "";
                                                }
                                                else
                                                    if (value.GetType() == typeof(bool))
                                                    {
                                                        bool bValue = (bool)value;
                                                        if (bValue)
                                                            tmpcellvalue = "是";
                                                        else
                                                            tmpcellvalue = "否";
                                                    }
                                                    else
                                                        tmpcellvalue = value.ToString();
                                    //  icellData.SetCellValue(value.ToString());
                                }

                                icellData.SetCellValue(tmpcellvalue);
                                break;
                            }
                            else
                                if (column.ColumnName == "TrayID")
                                {
                                    if (item.StockBox != null && item.StockBox.Tray != null)
                                    {
                                        icellData.SetCellValue(item.StockBox.Tray.BarCode);
                                    }
                                    else
                                        icellData.SetCellValue("");
                                }
                        }
                    }
                }
                #endregion
                using (FileStream stm = File.Create(fileName))
                {
                    wb.Write(stm);
                }
                bSucc = true;





            }
            catch (Exception ex)
            {
                bSucc = false;
                ErrMsg = "导出Excel不成功。" + ex.Message;
            }
            return bSucc;
        }
    }
    public class ExportRowHelper
    {
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
    }
}
