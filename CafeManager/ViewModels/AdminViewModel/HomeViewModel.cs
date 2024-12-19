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
using Microsoft.Extensions.DependencyInjection;
using CafeManager.WPF.Services;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class HomeViewModel : ObservableObject, IDataViewModel
    {
        private readonly InvoiceServices _invoiceServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly StaffServices _staffServices;
        private readonly FoodServices _foodServices;
        private readonly MaterialSupplierServices _materialSupplierServices;


        private CancellationToken _token = default;
        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private decimal _revenue = 0;

        [ObservableProperty]
        private int _totalInvoice;

        [ObservableProperty]
        private int _totalMaterialSupplier;

        [ObservableProperty]
        private int _totalStaff;

        [ObservableProperty]
        private int _totalFood;

        [ObservableProperty]
        private int _totalTable;

        [ObservableProperty]
        private List<decimal> _revenueDay;

        [ObservableProperty]
        private List<decimal> _revenueMonth;

        [ObservableProperty]
        private SeriesCollection _revenueSeries;

        [ObservableProperty]
        private Func<double, string> _xFormatter;

        [ObservableProperty]
        private Func<double, string> _yFormatter;

        [ObservableProperty]
        

        private DateTime _from = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime From
        {
            get => _from; set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged();
                    _ = LoadData(_token);
                }
            }
        }

        private DateTime _to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

        public DateTime To
        {
            get => _to; set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged();
                    _ = LoadData(_token);
                }
            }
        }
        [ObservableProperty]
        private ZoomingOptions _zoomingMode;

        [ObservableProperty]
        private int _xMin;

        [ObservableProperty]
        private int _xMax;

        public HomeViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();

        
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                IsLoading = true;
                Revenue = await _invoiceServices.GetRevenue(From, To, token);
                TotalStaff = await _staffServices.GetTotalStaff(From, To, token);
                TotalInvoice = await _invoiceServices.GetTotalInvoice(From, To, token);
                TotalFood = await _foodServices.GetTotalFood(token);
                TotalMaterialSupplier = await _materialSupplierServices.GetTotalMaterialSuplier(token);
                TotalTable = await _coffeTableServices.GetTotalTable(token);
                await LoadChartZoom(From, To, token);
               


                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task LoadChartZoom(DateTime from, DateTime to, CancellationToken token)
        {
            

            RevenueDay = await _invoiceServices.GetRevenueByDay(from, to, token);

            

            RevenueSeries = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Revenue by Day",
                Values = new ChartValues<decimal>(RevenueDay),
                PointGeometry = DefaultGeometries.Circle,
                PointGeometrySize = 5
            }
        };
            XMin = 0;
            XMax = (to - from).Days;

            XFormatter = val =>
            {
                var date = from.AddDays(val);
                return date.ToString("dd MMM");
            };

            YFormatter = val => val.ToString("C");
            ZoomingMode = ZoomingOptions.X;

        }







        //[RelayCommand]
        //public void ToggleSeries0() => FoodSeries.Visibility = FoodSeries.Visibility == Visibility.Visible
        //        ? Visibility.Hidden
        //        : Visibility.Visible;

        //[RelayCommand]
        //public void ToggleSeries1() =>
        //   ProfitSeries.Visibility = ProfitSeries.Visibility == Visibility.Visible
        //        ? Visibility.Hidden
        //        : Visibility.Visible;

        //[RelayCommand]
        //public void ToggleSeries2() =>
        //    InvoiceSeries.Visibility = InvoiceSeries.Visibility == Visibility.Visible
        //        ? Visibility.Hidden
        //        : Visibility.Visible;



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

        #endregion handleComboBox
    }
}