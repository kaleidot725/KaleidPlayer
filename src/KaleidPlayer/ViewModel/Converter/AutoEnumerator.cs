using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace kaleidot725.ViewModel.Converter
{
    public class AutoEnumerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var v = values[0];

            var list = values[1] as IList;
            if (list != null)
            {
                var orgStr = (list.IndexOf(v) + 1).ToString();
                var editStr = orgStr.PadLeft(4, '0');
                return editStr;
            }

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
