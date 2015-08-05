using Microsoft.Practices.Prism.Events;
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
using System.Windows.Shapes;


namespace SCG.SINOStock.Views
{
    /// <summary>
    /// AgainEnterNoPrint.xaml 的交互逻辑
    /// </summary>
    public partial class AgainEnterNoPrint : Window
    {
        protected IEventAggregator _eventAggregator;
        private string strTypaName = "BOX ID";
        private StockBoxRule _stockBoxRule = null;
        private TrayRule _trayRule = null;
        private int iType = 0;
        public AgainEnterNoPrint()
        {
            InitializeComponent();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            _stockBoxRule = new StockBoxRule();
            _trayRule = new TrayRule();
            //订阅
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.New:
                        iType = (int)param.Entity;
                        dtEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        dtStart.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                        DateTime Startdt = Convert.ToDateTime(dtStart.Text);
                        DateTime Enddt = Convert.ToDateTime(dtEnd.Text);
                        string ErrMsg = string.Empty;
                        txtChangeBoxID.Visibility = Visibility.Hidden;
                        txtChangeBoxID.Text = string.Empty;
                        switch (iType)
                        {
                            case 0:
                                strTypaName = "BOX ID";
                                TitleInfo.Content = "补打外箱标签";
                                title.Content = "选择BOX ID";
                                btnChange.Visibility = Visibility.Visible;

                                var boxList = _stockBoxRule.GetBoxListByDt(Startdt, Enddt.AddDays(1), ref ErrMsg).ToList();
                                if (boxList != null)
                                {
                                    boxList = boxList.OrderByDescending(p => p.CreateDt).ToList();
                                    cbBarCode.Items.Clear();
                                    foreach (var item in boxList)
                                    {
                                        cbBarCode.Items.Add(item.BarCode);
                                    }
                                }
                                break;
                            case 1:
                                strTypaName = "托号";
                                TitleInfo.Content = "补打托号";
                                title.Content = "选择托号";

                                var list = _trayRule.GetTrayListByDt(Startdt, Enddt.AddDays(1), ref ErrMsg);
                                if (list != null)
                                {
                                    list = list.OrderByDescending(p => p.CreateDt).ToList();
                                    cbBarCode.Items.Clear();
                                    foreach (var item in list)
                                    {
                                        cbBarCode.Items.Add(item.BarCode);
                                    }
                                }
                                btnChange.Visibility = Visibility.Hidden;
                                break;
                            default:
                                break;
                        }

                        break;

                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "AgainEnterNoPrint");
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DateTime Startdt = Convert.ToDateTime(dtStart.Text);
            DateTime Enddt = Convert.ToDateTime(dtEnd.Text);
            string ErrMsg = string.Empty;
            switch (iType)
            {
                case 0:
                    strTypaName = "BOX ID";
                    TitleInfo.Content = "补打外箱标签";
                    title.Content = "选择BOX ID";

                    var boxList = _stockBoxRule.GetBoxListByDt(Startdt, Enddt.AddDays(1), ref ErrMsg).ToList();
                    if (boxList != null)
                    {
                        boxList = boxList.OrderByDescending(p => p.CreateDt).ToList();
                        cbBarCode.Items.Clear();
                        foreach (var item in boxList)
                        {
                            cbBarCode.Items.Add(item.BarCode);
                        }
                    }
                    break;
                case 1:
                    strTypaName = "托号";
                    TitleInfo.Content = "补打托号";
                    title.Content = "选择托号";

                    var list = _trayRule.GetTrayListByDt(Startdt, Enddt.AddDays(1), ref ErrMsg);
                    if (list != null)
                    {
                        list = list.OrderByDescending(p => p.CreateDt).ToList();
                        cbBarCode.Items.Clear();
                        foreach (var item in list)
                        {
                            cbBarCode.Items.Add(item.BarCode);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (cbBarCode.SelectedIndex < 0)
            {
                Common.MessageBox.Show("请选择需要打印的条码");
                return;
            }
            string barcode = cbBarCode.SelectedItem.ToString();
           
            if (txtChangeBoxID.Visibility == Visibility.Visible && !string.IsNullOrWhiteSpace(txtChangeBoxID.Text))
            {
                string ErrMsg = string.Empty;
                if (_stockBoxRule.ModifyBoxBarCode(cbBarCode.SelectedItem.ToString(), txtChangeBoxID.Text.Trim(), ref ErrMsg))
                    barcode = txtChangeBoxID.Text.Trim();
            }

            switch (iType)
            {
                case 0:
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.SendPrintData_Box_Again,
                        Entity = barcode,
                        Target = "StockOutMainViewModel",
                    });
                    break;
                case 1:
                    _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                    {
                        cmdName = CmdName.SendPrintData_Tray_Again,
                        Entity = barcode,
                        Target = "StockOutMainViewModel",
                    });
                    break;


            }
            this.Close();
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            if (cbBarCode.SelectedIndex < 0)
            {
                Common.MessageBox.Show("请先选择BoxID");
                return;
            }
            txtChangeBoxID.Visibility = Visibility.Visible;
            txtChangeBoxID.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
