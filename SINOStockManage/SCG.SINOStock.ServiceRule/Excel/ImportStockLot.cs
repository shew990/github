using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/**
 *   命名空间:   SCG.SINOStock.ServiceRule
 *   文件名:     ImportStockLot
 *   说明:       
 *   创建时间:   2014/2/10 13:20:01
 *   作者:       liende
 */
namespace SCG.SINOStock.ServiceRule
{
    public class ImportStockLot : RuleBase
    {

        public event EventHandler<ResultsArgs<StockDetail>> ImportToXlsCompleted;

        public void ImportToXlsAsyns(string fileName)
        {
            ThreadHelper.BeginInvokeOnUIThread(() =>
                {
                    List<StockDetail> list = new List<StockDetail>();
                    try
                    {

                        using (var file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                        {

                            IWorkbook wb = new XSSFWorkbook(file);
                            ISheet sheet = wb.GetSheetAt(0);

                            for (int i = 0; i < sheet.LastRowNum; i++)
                            {
                                try
                                {
                                    IRow row = sheet.GetRow(i);
                                    for (int j = 0; j < 1; j++)
                                    {
                                        ICell readCell = row.GetCell(j);
                                        StockDetail glass = new StockDetail();
                                        glass.GlassID = readCell.ToString();
                                        readCell.ToString();//这就是当前格子的值了
                                        list.Add(glass);
                                    }
                                }
                                catch { }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        if (ImportToXlsCompleted != null)
                        {
                            ImportToXlsCompleted(this, new ResultsArgs<StockDetail>(null, ex, true, null));
                        }
                    }
                    if (ImportToXlsCompleted != null)
                    {
                        ImportToXlsCompleted(this, new ResultsArgs<StockDetail>(list, null, false, null));
                    }
                });
        }
        public List<StockDetail> ImportToXls(string fileName, ref string ErrMsg)
        {
            List<StockDetail> list = new List<StockDetail>();
            try
            {

                using (var file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {

                    string strExt = System.IO.Path.GetExtension(fileName);
                    IWorkbook wb;// = new XSSFWorkbook(file);
                    if (strExt.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(file);
                    }
                    else
                        wb = new XSSFWorkbook(file);
                    ISheet sheet = wb.GetSheetAt(0);

                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        try
                        {
                            IRow row = sheet.GetRow(i);
                            for (int j = 0; j < 1; j++)
                            {
                                ICell readCell = row.GetCell(j);
                                StockDetail glass = new StockDetail();
                                glass.GlassID = readCell.ToString().ToUpper();
                                readCell.ToString();//这就是当前格子的值了

                                glass.Qty = 1;
                                glass.AccountID = CurrentAccount.ID;
                                glass.AccountName = CurrentAccount.Name;
                                glass.Status = 0;
                                glass.CreateDt = DateTime.Now;
                                if (list.Any(p => p.GlassID.Equals(glass.GlassID)))
                                {
                                    ErrMsg = "Excel中存在重复的GlassID";
                                    return null;
                                }
                                list.Add(glass);
                            }
                        }
                        catch { }
                    }

                }

            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
            if (list.Count() <= 0)
            {
                ErrMsg = "当前不存在需要导入的GlassID，请检查Excel格式";
            }
            return list;
        }
    }

}
