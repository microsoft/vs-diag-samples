using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Cookbook.Converters
{
    public class TimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan time = (TimeSpan)value;
            if (time.Hours == 0)
            {
                return time.Minutes.ToString() + "m";
            }
            else
            {
                return time.Hours.ToString() + "h " + time.Minutes.ToString() + "m";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
