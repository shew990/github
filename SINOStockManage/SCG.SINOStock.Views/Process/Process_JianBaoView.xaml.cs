using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// Process_JianBaoView.xaml 的交互逻辑
    /// </summary>
    public partial class Process_JianBaoView : UserControl
    {
        private IEventAggregator eventAggregator;
        public Process_JianBaoView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Process_JianBaoViewModel();
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

            }, ThreadOption.UIThread, true, p => p.Target == "Process_JianBaoView");
        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
