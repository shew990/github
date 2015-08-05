using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// StockInMainView.xaml 的交互逻辑
    /// </summary>
    public partial class StockInMainView : UserControl
    {
        private IEventAggregator eventAggregator;
        public StockInMainView()
        {
            InitializeComponent();
            this.DataContext = new SCG.SINOStock.ViewModels.StockInMainViewModel();
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        txtLOTNO.Focus();
                        break;
                    case CmdName.Enter:
                        txtGlassID.Focus();
                        break;

                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "StockInMainView");
        }

        private void DataGrid_Sorting_1(object sender, DataGridSortingEventArgs e)
        {

        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
