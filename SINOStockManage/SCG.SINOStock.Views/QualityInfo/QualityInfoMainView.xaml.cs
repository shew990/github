﻿using System;
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
    /// QualityInfoMainView.xaml 的交互逻辑
    /// </summary>
    public partial class QualityInfoMainView : UserControl
    {
        public QualityInfoMainView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.QualityInfoMainViewModel();
        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
