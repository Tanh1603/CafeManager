﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CafeManager.WPF.Assets.UsersControls
{
    /// <summary>
    /// Interaction logic for InfoFood.xaml
    /// </summary>
    public partial class InfoFood : UserControl
    {
        public InfoFood()
        {
            InitializeComponent();
        }



        public string SourceImage
        {
            get { return (string)GetValue(SourceImageProperty); }
            set { SetValue(SourceImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceImageProperty =
            DependencyProperty.Register("SourceImage", typeof(string), typeof(InfoFood));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InfoFood));

        public int Price
        {
            get { return (int)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register("Price", typeof(int), typeof(InfoFood));


  
    }
    public class Utf8Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(text);
                return Encoding.UTF8.GetString(utf8Bytes);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrEmpty(imagePath))
            {
                try
                {
                    return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                }
                catch
                {
                    return null; // Trả về null nếu không tìm thấy ảnh
                }
            }
            return null; // Trả về null nếu giá trị không phải là chuỗi hợp lệ
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // Không cần xử lý ConvertBack
        }
    }
}