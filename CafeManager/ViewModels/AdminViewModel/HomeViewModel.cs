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
            throw new NotImplementedException();
        }

        private void CreateStackRowSeries()
        {
            throw new NotImplementedException();
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
            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

        }

        public ColumnSeries FoodSeries { get; set; }
        public ColumnSeries ProfitSeries { get; set; }
        public ColumnSeries InvoiceSeries { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


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