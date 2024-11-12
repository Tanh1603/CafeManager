using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.Services.Converter
{
    internal class DefaultAccountImageConverter : IValueConverter
    {
        public string DefaultImagePath { get; set; } = "/Assets/Images/coffee.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? new BitmapImage(new Uri(DefaultImagePath, UriKind.Relative)) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}