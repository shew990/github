using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
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

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// ToolScanGlassID_StockOut.xaml 的交互逻辑
    /// </summary>
    public partial class ToolScanGlassID_StockOut : Window
    {
        protected IEventAggregator _eventAggregator;
        public ToolScanGlassID_StockOut()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ToolScanGlassID_StockOutViewModel();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {

                switch (param.cmdName)
                {
                    case CmdName.New:
                        StockLot sl = param.Entity1 as StockLot;
                        cbInfoList.IsEnabled = sl.DetailInfoHOLD.Contains("镀膜");
                        this.btnOK.Focus();
                        cbInfoList.SetCheckList("镀膜");
                        FormWork fw = param.Entity as FormWork;
                        if (fw != null)
                            cbList.InitializeCheckBox(fw.RowQty, fw.ColumnQty);
                        else
                            cbList.InitializeCheckBox(5, 5);

                        _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = "",
                            Target = "CheckBoxButtonListView",
                        });
                        //cbList.CheckAll(true);
                        break;

                    case CmdName.Close:
                        this.Close();
                        break;
                    default:
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "ToolScanGlassID_StockOutView");
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
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(cbList.GetCheckList());
            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                cmdName = CmdName.SendTag,
                Entity = cbList.GetCheckList(),
                Tag = cbInfoList.GetCheckList(),
                Target = "ToolScanGlassID_StockOutViewModel",
            });

        }

        private void cbAll_Click(object sender, RoutedEventArgs e)
        {
            //cbList.CheckAll(cbAll.IsChecked.Value);
        }
    }
}
