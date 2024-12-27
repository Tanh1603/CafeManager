using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CafeManager.WPF.Services.Converter
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string category)
            {
                var foodCategory = category.Trim().ToLower();

                switch(foodCategory)
                {
                    case "cà phê":
                               return PackIconKind.LocalCafe;
                    case "sinh tố":
                        return PackIconKind.GlassFlute;
                    case "trà sữa":
                                 return PackIconKind.GlassCocktail;
                    case "nước ép":
                        return PackIconKind.CupFull;
                    default:
                        return PackIconKind.Food;

                }
            }
            return PackIconKind.Food; // Icon mặc định nếu không có dữ liệu
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
