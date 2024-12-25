using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CafeManager.WPF.Services.Converter
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chuyển đổi từ decimal sang string
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString(culture); // Sử dụng format mặc định của hệ thống
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chuyển đổi từ string sang decimal
            if (value is string stringValue)
            {
                if (decimal.TryParse(stringValue, NumberStyles.Any, culture, out decimal result))
                {
                    return result;
                }
            }
            // Trả về giá trị mặc định nếu chuỗi không hợp lệ
            return 0m;
        }
    }

    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chuyển đổi từ int sang string
            if (value is int intValue)
            {
                return intValue.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Chuyển đổi từ string sang int
            if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out int result))
                {
                    return result;
                }
            }
            // Trả về 0 nếu chuỗi không hợp lệ
            return 0;
        }
    }

    public class DecimalToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return $"{decimalValue:0}%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value?.ToString().Replace("%", "").Trim();
            if (decimal.TryParse(stringValue, NumberStyles.Any, culture, out var result))
            {
                return result;
            }
            return 0;
        }
    }
}