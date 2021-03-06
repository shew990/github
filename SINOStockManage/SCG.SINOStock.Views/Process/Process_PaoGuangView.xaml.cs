﻿using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
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
    /// Process_PaoGuangView.xaml 的交互逻辑
    /// </summary>
    public partial class Process_PaoGuangView : UserControl
    {
        private IEventAggregator eventAggregator;
        public Process_PaoGuangView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Process_PaoGuangViewModel();
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

            }, ThreadOption.UIThread, true, p => p.Target == "Process_PaoGuangView");
        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
