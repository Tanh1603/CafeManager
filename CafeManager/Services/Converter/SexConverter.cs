using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CafeManager.WPF.Services.Converter
{
    public class SexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool sex)
            {
                return sex ? "Nam" : "Nữ";
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string sexString)
            {
                return sexString == "Nam";
            }
            return Binding.DoNothing;
        }
    }
}