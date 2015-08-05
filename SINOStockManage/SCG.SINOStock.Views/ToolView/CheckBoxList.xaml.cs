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

namespace SCG.SINOStock.Views.ToolView
{
    /// <summary>
    /// CheckBoxList.xaml 的交互逻辑
    /// </summary>
    public partial class CheckBoxList : UserControl
    {
        private IEventAggregator eventAggregator;
        private QualityInfoRule _rule;
        public CheckBoxList()
        {
            InitializeComponent();
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();

            //try
            //{
            //    List<CheckBoxListItem> lst = new List<CheckBoxListItem>();
            //    CheckBoxListItem item1 = new CheckBoxListItem();
            //    item1.IsCheck = false;
            //    item1.value = "OK";
            //    CheckBoxListItem item2 = new CheckBoxListItem();
            //    item2.IsCheck = false;
            //    item2.value = "Cancel";
            //    CheckBoxListItem item3 = new CheckBoxListItem();
            //    item3.IsCheck = false;
            //    item3.value = "NO";
            //    lst.Add(item1);
            //    lst.Add(item2);
            //    lst.Add(item3);
            //    DataSourse = lst;
            //}
            //catch (Exception ex)
            //{
            //    int i = 1;
            //}
        }

        public void SetCheckList(string iType)
        {
            ugMain.Children.Clear();
            _rule = new QualityInfoRule();
            string ErrMsg = string.Empty;
            Dictionary<string, string> queryList = new Dictionary<string, string>();
            queryList.Add("Type", iType);
            List<QualityInfo> lst = _rule.GetQualityInfoList(queryList, ref ErrMsg);
            if (lst != null)
            {
                foreach (QualityInfo item in lst)
                {
                    CheckBox cb = new CheckBox();
                    cb.Click += (s, e) =>
                    {
                        string strcondition = string.Empty;
                        foreach (var cbitem in ugMain.Children)
                        {

                            CheckBox tmpcb = cbitem as CheckBox;
                            if (tmpcb != null && tmpcb.IsChecked.Value)
                            {
                                strcondition += tmpcb.Content.ToString() + ",";
                            }

                        }
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = strcondition,
                            Target = "CheckBoxButtonListView",
                        });
                    };
                    cb.Content = item.Name;
                    ugMain.Children.Add(cb);
                }
            }
        }
        public string GetCheckList()
        {
            string result = string.Empty;
            foreach (var cbitem in ugMain.Children)
            {

                CheckBox tmpcb = cbitem as CheckBox;
                if (tmpcb != null && tmpcb.IsChecked.Value)
                {
                    result += tmpcb.Content.ToString() + ",";
                }
            }
            return result;
        }
    }
}
