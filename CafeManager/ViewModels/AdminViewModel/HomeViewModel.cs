using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using PdfSharp.Charting;
using System.DirectoryServices;
using LiveCharts.Wpf;
using SeriesCollection = LiveCharts.SeriesCollection;
using System.Windows;
using Newtonsoft.Json.Linq;
using LiveCharts.Defaults;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using CafeManager.Core.Services;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class HomeViewModel : ObservableObject, IDataViewModel
    {
        private readonly IServiceProvider _provider;
        public IEnumerable<Product> items { get; set; }
        public HomeViewModel(IServiceProvider provider)
        {
            _provider = provider;
            items = new List<Product>
        {
            new Product { Title = "Product 1", Price = 1000 },
            new Product { Title = "Product 2", Price = 2000 },
            new Product { Title = "Product 3", Price = 3000 }
        };
            //CreateDynamicVisibility();
            //CreateStackRowSeries();
            //CreatePieSeries();
        }

        private void CreatePieSeries()
        {
            PieSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Chrome",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Mozilla",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Opera",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Explorer",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                    DataLabels = true
                }
            };
        }

        private void CreateStackRowSeries()
        {
            StackRowSeries = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {4, 5, 6, 8},
                    StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
                    DataLabels = true
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<double> {2, 5, 6, 7},
                    StackMode = StackMode.Values,
                    DataLabels = true
                }
            };

            LabelsStackRow = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            FormatterStackRow = value => value + " Mill";
        }

        private void CreateDynamicVisibility()
        {
            FoodSeries = new ColumnSeries
            {
                Title = "Food",
                Values = new ChartValues<double> { 10, 50, 39, 50 }
            };

            ProfitSeries = new ColumnSeries
            {
                Title = "Profit",
                Values = new ChartValues<double> { 10, 50, 39, 50 }
            };
            InvoiceSeries = new ColumnSeries
            {
                Title = "Invoice",
                Values = new ChartValues<double> { 10, 50, 39, 50 }
            };

            ColumnSeries = new SeriesCollection
            {
                FoodSeries,
                ProfitSeries,
                InvoiceSeries
            };
            LabelsCol = new[] { "Maria", "Susan", "Charles", "Frida" };
            FormatterCol = value => value.ToString("N");
        }

        public SeriesCollection PieSeries { get; set; }
        public SeriesCollection StackRowSeries { get; set; }
        public string[] LabelsStackRow { get; set; }
        public Func<double, string> FormatterStackRow { get; set; }

        public ColumnSeries FoodSeries { get; set; }
        public ColumnSeries ProfitSeries { get; set; }
        public ColumnSeries InvoiceSeries { get; set; }
        public string[] LabelsCol { get; set; }
        public Func<double, string> FormatterCol { get; set; }

        public SeriesCollection ColumnSeries { get; set; }

        [RelayCommand]
        public void ToggleSeries0() => FoodSeries.Visibility = FoodSeries.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;

        [RelayCommand]
        public void ToggleSeries1() =>
           ProfitSeries.Visibility = ProfitSeries.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;

        [RelayCommand]
        public void ToggleSeries2() =>
            InvoiceSeries.Visibility = InvoiceSeries.Visibility == Visibility.Visible
                ? Visibility.Hidden
                : Visibility.Visible;


        public class Product
        {
            public string Title { get; set; }
            public decimal Price { get; set; }
        }

        


        #region handleDatePicker

        [RelayCommand]
        public void ClearDatePicker(DatePicker datePicker)
        {
            if (datePicker != null)
            {
                datePicker.SelectedDate = null;

                var textBox = FindChild<DatePickerTextBox>(datePicker);
                if (textBox != null)
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                    return tChild;

                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        #endregion handleDatePicker

        #region handleComboBox

        [RelayCommand]
        private void ClearComboBox(ComboBox comboBox)
        {
            if (comboBox != null)
            {
                comboBox.SelectedItem = null; // Đặt SelectedItem về null
                comboBox.Text = string.Empty; // Xóa nội dung Text
            }
        }

        public Task LoadData(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        #endregion handleComboBox
    }
}