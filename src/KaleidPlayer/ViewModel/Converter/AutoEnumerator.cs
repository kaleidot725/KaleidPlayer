using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace kaleidot725.ViewModel.Converter
{
    public class AutoEnumerator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return "0000";
            }

            var item = values[0];
            var list = values[1] as IList;

            var index = (list.IndexOf(item) + 1).ToString();
            return index.PadLeft(4, '0');
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
