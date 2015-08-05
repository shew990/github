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
using System.Windows.Shapes;

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// ToolEnterNoToPrint.xaml 的交互逻辑
    /// </summary>
    public partial class ToolEnterNoToPrint : Window
    {
        protected IEventAggregator _eventAggregator;
        public ToolEnterNoToPrint()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ToolEnterNoToPrintViewModel();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.CancelPrint:
                        this.Close();
                        break;

                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "StockOutMainView");
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
