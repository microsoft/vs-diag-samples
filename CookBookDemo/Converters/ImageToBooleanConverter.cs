using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Cookbook.Converters
{
    public class ImageToBooleanConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Uri imageUri = new Uri("ms-appx:///" + (string)value);
            StorageFile file;
            try
            {
                file = StorageFile.GetFileFromApplicationUriAsync(imageUri).GetAwaiter().GetResult();
            }
            catch (FileNotFoundException)
            {
                file = null;
            }

            if(file == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

    }
}
