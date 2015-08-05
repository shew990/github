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

namespace SCG.SINOStock.Views.ToolView
{
    /// <summary>
    /// CheckBoxButtonList.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxButtonList : UserControl
    {
        private string _strResult = string.Empty;
        private IEventAggregator eventAggregator;
        public CheckBoxButtonList()
        {
            InitializeComponent();
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                gridMain.IsEnabled = false;
                string strcondition = param.Entity as string;
                spMain.Children.Clear();
                if (!string.IsNullOrWhiteSpace(strcondition))
                {
                    gridMain.IsEnabled = true;
                    string[] strArray = strcondition.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in strArray)
                    {

                        CheckBox cb = new CheckBox();
                        cb.Margin = new Thickness(5, 0, 5, 0);
                        cb.Content = item;
                        spMain.Children.Add(cb);

                    }
                }
            }, ThreadOption.UIThread, true, p => p.Target == "CheckBoxButtonListView");
        }
        public void InitializeCheckBox(int Row, int Col)
        {
            //int Row = 6;
            //int Col = 6;
            _strResult = string.Empty;
            ufgMain.Children.Clear();
            ufgMain.Columns = Col;



            for (int i = Row; i > 0; i--)
            {
                char cRow = (char)(i + 64);
                for (int j = 0; j < Col; j++)
                {
                    Button btn = new Button();
                    CheckBox cb = new CheckBox();

                    btn.Click += btn_Click;
                    cb.Click += (s, e) => e.Handled = true;
                    cb.Content = cRow.ToString() + ((char)(j + 97)).ToString();
                    btn.Content = cb;

                    ufgMain.Children.Add(btn);
                }
            }
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                CheckBox cb = btn.Content as CheckBox;
                if (cb.IsChecked == null || (!cb.IsChecked.Value))
                {


                    cb.IsChecked = true;
                }
                else
                    cb.IsChecked = false;
            }

        }


        public string GetCheckList()
        {
            return _strResult;
        }


        public void CheckAll(bool condition)
        {
            foreach (var item in ufgMain.Children)
            {
                Button btn = item as Button;
                if (btn != null)
                {
                    CheckBox cb = btn.Content as CheckBox;
                    cb.IsChecked = condition;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //判断是否选中选项
            bool bIsSelect = false;
            foreach (var item in spMain.Children)
            {

                CheckBox cb = item as CheckBox;
                if (cb != null && cb.IsChecked.Value)
                {
                    bIsSelect = true;
                    break;
                }

            }
            if (!bIsSelect)
            {
                Common.MessageBox.Show("请勾选品质信息再提交");
                return;
            }

            foreach (var item in ufgMain.Children)
            {
                Button btn = item as Button;
                if (btn != null)
                {
                    CheckBox cb = btn.Content as CheckBox;
                    string strinfo = getSpMainCheck();
                    if (cb != null && cb.IsChecked.Value && cb.IsEnabled)
                    {
                        _strResult += cb.Content.ToString() + strinfo + ",";
                        cb.IsEnabled = false;
                        btn.IsEnabled = false;

                    }
                }
            }
            // return _strResult;
        }
        private string getSpMainCheck()
        {
            string result = string.Empty;
            foreach (var item in spMain.Children)
            {

                CheckBox cb = item as CheckBox;
                if (cb != null && cb.IsChecked.Value)
                {
                    result += cb.Content.ToString() + "、";
                }

            }
            return result;
        }
        private void cbAll_Click(object sender, RoutedEventArgs e)
        {
            CheckAll(cbAll.IsChecked.Value);
        }
    }
}
