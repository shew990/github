using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCG.SINOStock.ServiceRule.Excel
{
    public class NPOIHelper
    {
        #region 定义单元格常用到样式的枚举
        public enum stylexls
        {
            头,
            url,
            时间,
            数字,
            整数,
            钱,
            百分比,
            中文大写,
            科学计数法,
            默认
        }
        #endregion


        #region 定义单元格常用到样式
        public ICellStyle Getcellstyle(IWorkbook wb, stylexls str)
        {
            ICellStyle cellStyle = wb.CreateCellStyle();

            //定义几种字体  
            //也可以一种字体，写一些公共属性，然后在下面需要时加特殊的  
            IFont font18 = wb.CreateFont();
            font18.FontHeightInPoints = 18;
            font18.FontName = "微软雅黑";

            IFont font12 = wb.CreateFont();
            font12.FontHeightInPoints = 12;
            font12.FontName = "微软雅黑";


            IFont font = wb.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 10;
            //font.Underline = 1;下划线  


            IFont fontcolorblue = wb.CreateFont();
            fontcolorblue.Color = HSSFColor.OliveGreen.Blue.Index;
            fontcolorblue.IsItalic = true;  
            fontcolorblue.FontName = "微软雅黑";


            //边框  
            //cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOTTED;
            //cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.HAIR;
            //cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.HAIR;
            //cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.DOTTED;

            //边框颜色  
            //cellStyle.BottomBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;
            //cellStyle.TopBorderColor = HSSFColor.OLIVE_GREEN.BLUE.index;

            //背景图形
            //cellStyle.FillBackgroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;  
            //cellStyle.FillForegroundColor = HSSFColor.OLIVE_GREEN.BLUE.index;  
            //cellStyle.FillForegroundColor = HSSFColor.WHITE.index;
            // cellStyle.FillPattern = FillPatternType.NO_FILL;  
            //cellStyle.FillBackgroundColor = HSSFColor.BLUE.index;

            //水平对齐  
            cellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;

            //垂直对齐  
            cellStyle.VerticalAlignment = VerticalAlignment.Center;

            //自动换行  
            cellStyle.WrapText = true;

            //缩进;当设置为1时，前面留的空白太大了。
            cellStyle.Indention = 0;

            //上面基本都是设共公的设置  
            //下面列出了常用的字段类型  
            switch (str)
            {
                case stylexls.头:
                    //cellStyle.FillPattern = FillPatternType.LEAST_DOTS;  
                    cellStyle.SetFont(font18);
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    break;
                case stylexls.时间:
                    IDataFormat datastyle = wb.CreateDataFormat();

                    cellStyle.DataFormat = datastyle.GetFormat("yyyy/mm/dd");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.数字:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
                    cellStyle.Alignment = HorizontalAlignment.Right;
                    cellStyle.SetFont(font);
                    break;
                case stylexls.整数:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0");
                    cellStyle.Alignment = HorizontalAlignment.Right;
                    cellStyle.SetFont(font);
                    break;
                case stylexls.钱:
                    IDataFormat format = wb.CreateDataFormat();
                    cellStyle.DataFormat = format.GetFormat("￥#,##0");
                    cellStyle.Alignment = HorizontalAlignment.Right;
                    cellStyle.SetFont(font);
                    break;
                case stylexls.url:
                    fontcolorblue.Underline = FontUnderlineType.Single;
                    cellStyle.SetFont(fontcolorblue);
                    break;
                case stylexls.百分比:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00%");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.中文大写:
                    IDataFormat format1 = wb.CreateDataFormat();
                    cellStyle.DataFormat = format1.GetFormat("[DbNum2][$-804]0");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.科学计数法:
                    cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00E+00");
                    cellStyle.SetFont(font);
                    break;
                case stylexls.默认:
                    cellStyle.SetFont(font);
                    break;
            }
            return cellStyle;


        }
        #endregion  

        public ICellStyle CreateBorderCellStyle(HSSFWorkbook wb)
        {
            ICellStyle style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;
            style.BorderTop = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            return style;
        }
        public IRow CreateRowCell(ISheet sheet, int rowIdx, int toColIdx)
        {
            return CreateRowCell(sheet, rowIdx, 0, toColIdx, null);
        }
        public IRow CreateRowCell(ISheet sheet, int rowIdx, int toColIdx, ICellStyle cellStyle)
        {
            return CreateRowCell(sheet, rowIdx, 0, toColIdx, cellStyle);
        }
        public IRow CreateRowCell(ISheet sheet, int rowIdx, int fromColIdx, int toColIdx, ICellStyle cellStyle)
        {
            IRow row = sheet.CreateRow(rowIdx);
            for (int i = fromColIdx; i <= toColIdx; i++)
            {
                ICell cell = row.CreateCell(i);
                if (cellStyle != null)
                {
                    cell.CellStyle = cellStyle;
                }
            }
            return row;
        }

    }
}
