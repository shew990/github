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
    /// StockOutMainView.xaml 的交互逻辑
    /// </summary>
    public partial class StockOutMainView : UserControl
    {
        protected IEventAggregator _eventAggregator;
        private StockLotRule _rule = null;
        public StockOutMainView()
        {
            InitializeComponent();
            _rule = new StockLotRule();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this.DataContext = new ViewModels.StockOutMainViewModel();
            _eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdName)
                {
                    case CmdName.Refresh:
                        spMain.Children.Clear();
                        StackPanel spHand = new StackPanel();

                        Label lbHand1 = new Label();
                        lbHand1.Content = "LONO：";
                        Label lbHand2 = new Label();
                        lbHand2.Content = "实际入库数：";
                        Label lbHand3 = new Label();
                        lbHand3.Content = "实际出库数：";
                        spHand.Children.Add(lbHand1);
                        spHand.Children.Add(lbHand2);
                        spHand.Children.Add(lbHand3);


                        spMain.Children.Add(spHand);

                        List<int> lst = param.Entity as List<int>;

                        if (lst != null && lst.Count() > 0)
                        {
                            StockOutQtyHelper[] arr = _rule.GetStockOutQtys(lst.ToArray());

                            foreach (StockOutQtyHelper item in arr)
                            {

                                StackPanel sp = new StackPanel();

                                Label lb1 = new Label();
                                lb1.Content = item.LOTNO;
                                Label lb2 = new Label();
                                lb2.Content = item.Qty;
                                Label lb3 = new Label();
                                lb3.Content = item.StockOutQty;
                                sp.Children.Add(lb1);
                                sp.Children.Add(lb2);
                                sp.Children.Add(lb3);


                                spMain.Children.Add(sp);
                                //Label lb = new Label();
                                //lb.Content = item;
                                //lb.Margin = new Thickness(10, 0, 0, 0);
                                //spMain.Children.Add(lb);
                            }
                        }
                        //if (lst != null)
                        //{

                        //}
                        break;
                    case CmdName.New:
                        txtLOTNO.Focus();
                        break;
                    case CmdName.Enter:
                        txtGlassID.Focus();
                        break;
                    default:
                        break;
                }

            }, ThreadOption.UIThread, true, p => p.Target == "StockOutMainView");
        }
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
