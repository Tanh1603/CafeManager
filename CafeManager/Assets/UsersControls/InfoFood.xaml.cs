using System;
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

        public BitmapImage SourceImage
        {
            get { return (BitmapImage)GetValue(SourceImageProperty); }
            set { SetValue(SourceImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceImageProperty =
            DependencyProperty.Register("SourceImage", typeof(BitmapImage), typeof(InfoFood));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InfoFood));

        public decimal? Price
        {
            get { return (int)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PriceProperty =
            DependencyProperty.Register("Price", typeof(decimal?), typeof(InfoFood));

        public decimal? Discountfood
        {
            get { return (decimal?)GetValue(DiscountfoodProperty); }
            set { SetValue(DiscountfoodProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Discountfood.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiscountfoodProperty =
            DependencyProperty.Register("Discountfood", typeof(decimal?), typeof(InfoFood));

        public static readonly RoutedEvent UpdateFoodEvent = EventManager.RegisterRoutedEvent("UpdateFood", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InfoFood));

        public event RoutedEventHandler UpdateFood
        {
            add { AddHandler(UpdateFoodEvent, value); }
            remove { RemoveHandler(UpdateFoodEvent, value); }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UpdateFoodEvent));
        }

        public static readonly RoutedEvent DeleteFoodEvent = EventManager.RegisterRoutedEvent("DeleteFood", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InfoFood));

        public event RoutedEventHandler DeleteFood
        {
            add { AddHandler(DeleteFoodEvent, value); }
            remove { RemoveHandler(DeleteFoodEvent, value); }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DeleteFoodEvent));
        }

        public bool Isdeleted
        {
            get { return (bool)GetValue(IsdeletedProperty); }
            set { SetValue(IsdeletedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Isdeleted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsdeletedProperty =
            DependencyProperty.Register("Isdeleted", typeof(bool), typeof(InfoFood));

        public static readonly RoutedEvent RestoreFoodEvent = EventManager.RegisterRoutedEvent("RestoreFood", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InfoFood));

        public event RoutedEventHandler RestoreFood
        {
            add { AddHandler(RestoreFoodEvent, value); }
            remove { RemoveHandler(RestoreFoodEvent, value); }
        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RestoreFoodEvent));
        }
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
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage bitmapImage)
            {
                return bitmapImage;
            }
            return new BitmapImage(new Uri("pack://application:,,,/Assets/Images/defaultImage.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}