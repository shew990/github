using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

/**
 *   命名空间:   SCG.SINOStock.Common
 *   文件名:     DictValueConvert
 *   说明:       
 *   创建时间:   2014/2/21 14:37:20
 *   作者:       liende
 */
namespace SCG.SINOStock.Common
{
    public class DictValueConvert : UserControl, IValueConverter
    {
        private int typevalue = -1;
        private void BuildDictData(string dictName)
        {
            switch (dictName.Trim())
            {
                case "StockDetailStatus":
                    Dict = GetStockDetailStatusDicts();
                    break;
                case "BoolConvertYesOrNo":
                    Dict = GetBoolConvertYesOrNoDicts();
                    break;

            }
        }
        private List<SystemDict> GetStockDetailStatusDicts()
        {
            List<SystemDict> dicts = new List<SystemDict>();
            SystemDict status1 = new SystemDict();
            SystemDict status2 = new SystemDict();
            SystemDict status3 = new SystemDict();
            SystemDict status4 = new SystemDict();
            SystemDict status5 = new SystemDict();
            SystemDict status6 = new SystemDict();
            SystemDict status7 = new SystemDict();
            SystemDict status8 = new SystemDict();
            status1.DictName = "未入库";
            status1.DictValue = 0;

            status2.DictName = "已入库";
            status2.DictValue = 1;

            status3.DictName = "减薄后";
            status3.DictValue = 2;

            status4.DictName = "抛光后";
            status4.DictValue = 3;

            status5.DictName = "已出库";//做完镀膜便为已出库
            status5.DictValue = 4;

            status6.DictName = "已出库";
            status6.DictValue = 5;

            status7.DictName = "返工中";
            status7.DictValue = 8;

            status8.DictName = "退货";
            status8.DictValue = -1;
            dicts.Add(status1);
            dicts.Add(status2);
            dicts.Add(status3);
            dicts.Add(status4);
            dicts.Add(status5);
            dicts.Add(status6);
            dicts.Add(status7);
            dicts.Add(status8);
            return dicts;
        }

        private List<SystemDict> GetBoolConvertYesOrNoDicts()
        {
            List<SystemDict> dicts = new List<SystemDict>();
            SystemDict status1 = new SystemDict();
            SystemDict status2 = new SystemDict();
            status1.DictName = "否";
            status1.DictValue = 0;

            status2.DictName = "是";
            status2.DictValue = 1;

            dicts.Add(status1);
            dicts.Add(status2);
            return dicts;
        }
        private List<SystemDict> Dict = new List<SystemDict>();
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null) return "未知";
            if (parameter.ToString().Equals("GetScheduleValue"))
            {
                typevalue = (int)value;
                return "";
            }
            if (value == null) return "未知";

            BuildDictData(parameter.ToString());

            if (Dict == null) return "未知";
            if (!Dict.Any()) return "未知";

            var q = from c in Dict where c.DictValue == System.Convert.ToInt32(value) select c;
            if (q.Any())
            {
                return q.FirstOrDefault().DictName;
            }
            else
            {
                return "未知";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   // public class boolUnConvert:
}
