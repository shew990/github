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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// RoleManageView.xaml 的交互逻辑
    /// </summary>
    public partial class RoleManageView : UserControl
    {
        public RoleManageView()
        {
            InitializeComponent();
        }
        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            ScaleTransform st = new ScaleTransform(1, 1);
            DoubleAnimation cartoonClear = new DoubleAnimation(0.1, 1, new Duration(TimeSpan.FromMilliseconds(500)));
            this.RenderTransform = st;
            st.BeginAnimation(ScaleTransform.ScaleYProperty, cartoonClear);
            st.BeginAnimation(ScaleTransform.ScaleXProperty, cartoonClear);
        }
    }
}
