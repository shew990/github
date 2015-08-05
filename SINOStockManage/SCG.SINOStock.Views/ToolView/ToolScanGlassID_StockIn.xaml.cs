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
    /// ToolScanGlassID_StockIn.xaml 的交互逻辑
    /// </summary>
    public partial class ToolScanGlassID_StockIn : Window
    {
        protected IEventAggregator _eventAggregator;
        public ToolScanGlassID_StockIn()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ToolScanGlassID_StockInViewModel();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {

                switch (param.cmdName)
                {
                    case CmdName.New:
                        StockLot sl = param.Entity as StockLot;
                        cbInfoList.IsEnabled = sl.DetailInfoHOLD.Contains("入库");
                        this.btnOK.Focus();
                        cbInfoList.SetCheckList("入库");
                        break;

                    case CmdName.Close:
                        this.Close();
                        break;
                    default:
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "ToolScanGlassID_StockInView");
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(cbList.GetCheckList());
            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                cmdName = CmdName.SendTag,
                //Entity = cbList.GetCheckList(),
                Tag = cbInfoList.GetCheckList(),
                Target = "ToolScanGlassID_StockInViewModel",
            });

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


    }
}
