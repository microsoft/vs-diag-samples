//using Cirrious.CrossCore.Converters;
using MyShuttle.Client.Core.DocumentResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace MyShuttle.Client.Core.Converters
{
    public class VehicleStatusToBoolConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var compValue = (VehicleStatus)value;
            var vehicleNotFree = compValue != VehicleStatus.Free;

            return vehicleNotFree;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
