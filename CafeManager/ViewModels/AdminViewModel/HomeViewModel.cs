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

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        public HomeViewModel(IServiceProvider provider)
        {
            _provider = provider;
            CreateDynamicVisibility();
            CreateStackRowSeries();
            CreatePieSeries();
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
            FoodSeries  =  new ColumnSeries
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
    }
}