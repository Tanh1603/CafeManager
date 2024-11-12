﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CafeManager.WPF.Services.Converter
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
            {
                return decimalValue.ToString("C0", CultureInfo.GetCultureInfo("vi-VN"));
            }
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("vi-VN"), out var result))
            {
                return result;
            }
            return value;
        }
    }
}