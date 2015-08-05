using Microsoft.Win32;
using SCG.SINOStock.ServiceRule.Excel;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCG.SINOStock.Views.ToolView
{
    /// <summary>
    /// ExportGlassIDs.xaml 的交互逻辑
    /// </summary>
    public partial class ExportGlassIDs : Window
    {
        private ExportGlassID _export = null;
        private List<StockDetail> _lst;
        public ExportGlassIDs(List<StockDetail> lst)
        {
            _lst = lst;
            _export = new ExportGlassID();
            InitializeComponent();
        }
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<ExportRowHelper> ExportColumnNames = new List<ExportRowHelper>();

            foreach (var item in ufgMain.Children)
            {
                CheckBox cb = item as CheckBox;
                if (cb.IsChecked.Value)
                {
                    ExportColumnNames.Add(new ExportRowHelper() { ColumnName = cb.Tag.ToString(), ColumnValue = cb.Content.ToString() });
                }
            }
            if (ExportColumnNames.Count() <= 0)
            {
                Common.MessageBox.Show("请选择需要导出的列");
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = string.Format("GlassID导出列表.xls");
            if (sfd.ShowDialog() == true)
            {
                string filename = sfd.FileName;
                string ErrMsg = string.Empty;
                bool bSucc = false;

                bSucc = _export.ExportGlassIDToExcel(_lst, ExportColumnNames, filename, ref ErrMsg);
                if (bSucc)
                {
                    var process = System.Diagnostics.Process.Start(filename);
                }
                else
                    Common.MessageBox.Show(ErrMsg);
            }
            this.Close();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
