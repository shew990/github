using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace SCG.SINOStock.Common
{
    public class DictBoolValueConvert : UserControl, IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           // if (parameter == null) return false;
            if (value == null) return false;

           // BuildDictData(parameter.ToString());
            bool typevalue = (bool)value;
            return !typevalue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
