using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using SeriesCollection = LiveCharts.SeriesCollection;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class HomeViewModel : ObservableObject, IDataViewModel, INotifyDataErrorInfo
    {
        private readonly InvoiceServices _invoiceServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly StaffServices _staffServices;
        private readonly FoodServices _foodServices;
        private readonly ImportServices _importServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ErrorViewModel _errorViewModel;

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
        private Dictionary<DateTime, decimal> _revenueDay;

        [ObservableProperty]
        private List<decimal> _revenueMonth;

        [ObservableProperty]
        private Dictionary<DateOnly, decimal> _totalSalaryMonth;

        [ObservableProperty]
        private List<decimal> _totalMaterialCostMonth;

   

        [ObservableProperty]
        private SeriesCollection _chartZoomCollection;

        [ObservableProperty]
        private SeriesCollection _chartColumnCollection;

        [ObservableProperty]
        private List<string> _labelChartZoom;

        [ObservableProperty]
        private List<string> _labelChartCol;

        [ObservableProperty]
        private Func<double, string> _xFormatterChartZoom;

        [ObservableProperty]
        private Func<double, string> _yFormatterChartZoom;

        [ObservableProperty]
        private Func<double, string> _xFormatterChartCol;

   

        [ObservableProperty]
        private string _selectedTopic = "Revenue";

        private DateTime _from = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DateTime From
        {
            get => _from; set
            {
                if (_from != value)
                {
                    _from = value;
                    if (!ValidateFrom()  && !ValidateDates())
                    {
                        OnPropertyChanged();
                        _ = LoadData(_token);
                    }
                }
            }
        }
        private bool ValidateFrom()
        {
            bool hasError = false;
            if(From == null)
            {
                _errorViewModel.AddError(nameof(From),"Không được trống");
                hasError = true;
            }
            return hasError;
        }
        private bool ValidateDates()
        {
            bool hasError = false;
            _errorViewModel.RemoveErrors(nameof(From));
            _errorViewModel.RemoveErrors(nameof(To));
            
          
            if (From > To)
            {
                _errorViewModel.AddError(nameof(From), "Ngày bắt đầu không được lớn hơn ngày kết thúc");
                _errorViewModel.AddError(nameof(To), "Ngày kết thúc không được nhỏ hơn ngày bắt đầu");
                hasError = true;
            }
            return hasError;
        }

        private DateTime _to = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

        public DateTime To
        {
            get => _to; set
            {
                if (_to != value)
                {
                    _to = value;

                    if (!ValidateTo() &&!ValidateDates())
                    {
                        OnPropertyChanged();
                        _ = LoadData(_token);
                    }
                }
            }
        }

        private bool ValidateTo()
        {
            bool hasError = false;
            if (To == null)
            {
                _errorViewModel.AddError(nameof(To), "Không được trống");
                hasError = true;
            }
            return hasError;
        }

        public bool HasErrors => _errorViewModel.HasErrors;

        [ObservableProperty]
        private ZoomingOptions _zoomingMode;

        [ObservableProperty]
        private int _xMin;

        [ObservableProperty]
        private int _xMax;

        [ObservableProperty]
        private List<FoodDTO> _mostSoldFood;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public HomeViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _importServices = provider.GetRequiredService<ImportServices>();
            _errorViewModel = new ErrorViewModel();
            _errorViewModel.ErrorsChanged += _errorViewModel_ErrorsChanged;
        }

        private void _errorViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
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
                MostSoldFood = await _foodServices.GetMostSoldFoods(From, To, token);
             
                await LoadChartZoom(From, To, token);
                await LoadColumnSeries(To, From, token);

                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task LoadChartZoom(DateTime from, DateTime to, CancellationToken token)
        {

            RevenueDay = await _invoiceServices.GetRevenueByDay(From, To, token);



            var lineSeries = new LineSeries()
            {
                Title = "Doanh thu",
                Values = new ChartValues<decimal>(),

                PointGeometrySize = 0,
                Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9b442a")),
                StrokeThickness = 0,
                Fill = new System.Windows.Media.LinearGradientBrush
                {
                    StartPoint = new System.Windows.Point(0.5, 0),
                    EndPoint = new System.Windows.Point(0.5, 1),
                    GradientStops =
                                    {
                                        new GradientStop { Offset = 0.5, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#d5b096") },
                                        new GradientStop { Offset = 1, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#dcd1c3") }
                                    }
                },
                LabelPoint = point => $" {point.Y:N0} VNĐ",
            };

            LabelChartZoom = new List<string>();
            foreach (var item in RevenueDay)
            {
                lineSeries.Values.Add(item.Value);
                LabelChartZoom.Add(item.Key.ToString("dd/MM/yyyy"));
            }

              YFormatterChartZoom = value => value.ToString("N0");
            ChartZoomCollection = new SeriesCollection { lineSeries };
        }

        public async Task LoadColumnSeries(DateTime from, DateTime to, CancellationToken token)
        {
            RevenueMonth = await _invoiceServices.GetRevenueByMonth(From, To, token);

            TotalSalaryMonth = await _staffServices.GetTotalSalaryByMonth(From, To, token);

            TotalMaterialCostMonth = await _importServices.GetTotalMaterialCostByMonth(From, To, token);
          

                var RevenueCol = new ColumnSeries()
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<decimal>(),
                    Fill = new System.Windows.Media.LinearGradientBrush
                    {
                        StartPoint = new System.Windows.Point(0, 0.5),
                        EndPoint = new System.Windows.Point(1, 0.5),
                        GradientStops =
                                    {
                                        new GradientStop { Offset = 0, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CFA944") },
                                        new GradientStop { Offset = 1, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFDD3C") }
                                    }
                    },
                    Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#CFA944")),
                    StrokeThickness = 0,
                    LabelPoint = point => $" {point.Y:N0} VNĐ",
                };

                var SalaryCol = new ColumnSeries()
                {
                    Title = "Lương",
                    Values = new ChartValues<decimal>(),
                    Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#55C595")),
                    StrokeThickness = 0,
                    Fill = new System.Windows.Media.LinearGradientBrush
                    {
                        StartPoint = new System.Windows.Point(0, 0.5),
                        EndPoint = new System.Windows.Point(1, 0.5),
                        GradientStops =
                                    {
                                        new GradientStop { Offset = 0, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#55C595") },
                                        new GradientStop { Offset = 1, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#7CE495") }
                                    }
                    },
                    LabelPoint = point => $" {point.Y:N0} VNĐ",
                };
                var MaterialCostCol = new ColumnSeries()
                {
                    Title = "Vật liệu",
                    Values = new ChartValues<decimal>(),
                    Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#661618")),
                    StrokeThickness = 0,
                    Fill = new System.Windows.Media.LinearGradientBrush
                    {
                        StartPoint = new System.Windows.Point(0, 0.5),
                        EndPoint = new System.Windows.Point(1.5, 0.5),
                        GradientStops =
                                    {
                                        new GradientStop { Offset = 0, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#661618") },
                                        new GradientStop { Offset = 1, Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#844518") }
                                    }
                    },
                    LabelPoint = point => $" {point.Y:N0} VNĐ",
                };


            LabelChartCol = new List<string>();
            foreach(var item in RevenueMonth)
            { 
                RevenueCol.Values.Add(item);

            }

            foreach (var item in TotalSalaryMonth)
            {
                SalaryCol.Values.Add(item.Value);
                LabelChartCol.Add(item.Key.ToString("MM/yyyy"));

            }

            foreach (var item in TotalMaterialCostMonth)
            {
                MaterialCostCol.Values.Add(item);

            }
            ChartColumnCollection = new SeriesCollection { RevenueCol, SalaryCol, MaterialCostCol };

        }

       

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }
    }
}