using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace kaleidot725.ViewModel.Converter
{
    /// <summary>
    /// TimeSpan to mm:ss 文字列
    /// </summary>
    class TimeConverter : IValueConverter
    {
        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time = (TimeSpan)value;
            var seconds = string.Format("{0:00}", time.Seconds);
            var minutes = string.Format("{0:00}", time.Minutes);
            return minutes + ":" + seconds;
        }

        /// <summary>
        /// 逆変換
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
