using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CafeManager.WPF.Services.Converter
{
    public class SeatingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                2 => "2 chỗ",
                4 => "4 chỗ",
                6 => "6 chỗ",
                8 => "8 chỗ",
                10 => "10 chỗ",
                _ => "Không xác định"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "2 chỗ" => 2,
                "4 chỗ" => 4,
                "6 chỗ" => 6,
                "8 chỗ" => 8,
                "10 chỗ" => 10,
                _ => 0
            };
        }
    }
}