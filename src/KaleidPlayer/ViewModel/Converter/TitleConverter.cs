using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using kaleidot725.Model;

namespace kaleidot725.ViewModel.Converter
{
    /// <summary>
    /// 
    /// </summary>
    class TitleConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IAudioDetail audio = (IAudioDetail)value;
            string title = string.Format("{0}", audio.Title, audio.Artist);
            if (25 < title.Length)
            {
                title = title.Substring(0, 25);
                title = title + "…";
            }

            return title;
        }

        /// <summary>
        /// 
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
