//using Cirrious.CrossCore.Converters;
using MyShuttle.Client.Core.DocumentResponse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Core.Converters
{
    //public class RatingToBoolConverter : MvxValueConverter<double, bool>
    //{
    //    protected override bool Convert(double value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        if (parameter == null)
    //        {
    //            // Hide stars by default
    //            return true;
    //        }

    //        double rateToCompareWith;

    //        try
    //        {
    //            rateToCompareWith = (long)parameter;
    //        }
    //        catch (InvalidCastException)
    //        {
    //            rateToCompareWith = (int)parameter;
    //        }

    //        var hideCurrentRate = value < rateToCompareWith;

    //        return hideCurrentRate;
    //    }
    //}
}
