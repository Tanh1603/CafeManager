using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.ComponentModel;
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
        private List<decimal> _revenueDay;

        [ObservableProperty]
        private List<decimal> _revenueMonth;

        [ObservableProperty]
        private List<decimal> _revenueYear;

        [ObservableProperty]
        private List<int> _invoiceMonth;

        [ObservableProperty]
        private List<int> _invoiceDay;

        [ObservableProperty]
        private List<int> _invoiceYear;

        [ObservableProperty]
        private List<decimal> _totalImportDay;

        [ObservableProperty]
        private List<decimal> _totalImportMonth;

        [ObservableProperty]
        private List<decimal> _totalImportYear;

        [ObservableProperty]
        private List<decimal> _totalSalaryMonth;

        [ObservableProperty]
        private List<decimal> _totalSalaryYear;

        [ObservableProperty]
        private List<decimal> _totalMaterialCostMonth;

        [ObservableProperty]
        private List<decimal> _totalMaterialCostYear;

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
        private string _selectedTimeChartZoom = "Daily";

        [ObservableProperty]
        private string _selectedTimeChartCol = "Monthly";

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
                    if (!ValidateDates())
                    {
                        OnPropertyChanged();
                        _ = LoadData(_token);
                    }
                }
            }
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

                    if (!ValidateDates())
                    {
                        OnPropertyChanged();
                        _ = LoadData(_token);
                    }
                }
            }
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
                RevenueDay = await _invoiceServices.GetRevenueByDay(From, To, token);
                RevenueMonth = await _invoiceServices.GetRevenueByMonth(From, To, token);
                RevenueYear = await _invoiceServices.GetRevenueByYear(From, To, token);
                InvoiceDay = await _invoiceServices.GetTotalInvoiceByDay(From, To, token);
                InvoiceMonth = await _invoiceServices.GetTotalInvoiceByMonth(From, To, token);
                InvoiceYear = await _invoiceServices.GetTotalInvoiceByYear(From, To, token);
                TotalImportDay = await _importServices.GetTotalMaterialByDay(From, To, token);
                TotalImportMonth = await _importServices.GetTotalMaterialByMonth(From, To, token);
                TotalImportYear = await _importServices.GetTotalMaterialByYear(From, To, token);
                TotalSalaryMonth = await _staffServices.GetTotalSalaryByMonth(From, To, token);
                TotalSalaryYear = await _staffServices.GetTotalSalaryByYear(From, To, token);
                TotalMaterialCostMonth = await _importServices.GetTotalMaterialCostByMonth(From, To, token);
                TotalMaterialCostYear = await _importServices.GetTotalMaterialCostByYear(From, To, token);
                MostSoldFood = await _foodServices.GetMostSoldFoods(From, To, token);
                await LoadChartZoom(From, To);
                await LoadColumnSeries(To, From);

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

        public async Task LoadChartZoom(DateTime from, DateTime to)
        {
            XMin = 0;

            if (ChartZoomCollection == null)
            {
                ChartZoomCollection = new SeriesCollection()
            { new LineSeries()
            {
                Title = "Doanh thu",
                Values = new ChartValues<decimal>(RevenueDay),

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
            }
            };
            }

            var lineSeries = _chartZoomCollection[0] as LineSeries;

            switch (SelectedTimeChartZoom)
            {
                case "Daily":

                    if (SelectedTopic == "Revenue")
                    {
                        lineSeries.Values = new ChartValues<decimal>(RevenueDay);
                    }
                    else if (SelectedTopic == "Invoice")
                    {
                        lineSeries.Values = new ChartValues<int>(InvoiceDay);
                    }
                    else if (SelectedTopic == "Import")
                    {
                        lineSeries.Values = new ChartValues<decimal>(TotalImportDay);
                    }
                    OnPropertyChanged(nameof(ChartZoomCollection));

                    XMax = (to - from).Days;

                    XFormatterChartZoom = value =>
                    {
                        var date = from.AddDays(value);
                        if (date < DateTime.MinValue || date > DateTime.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException(nameof(value), "Value to add is out of range.");
                        }
                        return date.ToString("dd MMM");
                    };

                    var days = Enumerable.Range(0, (to - from).Days + 1)
                         .Select(offset => from.AddDays(offset).ToString("dd/MM/yyyy"))
                         .ToList();

                    // Gán nhãn cho trục X
                    LabelChartZoom = days;
                    break;

                case "Monthly":

                    if (SelectedTopic == "Revenue")
                    {
                        lineSeries.Values = new ChartValues<decimal>(RevenueMonth);
                    }
                    else if (SelectedTopic == "Invoice")
                    {
                        lineSeries.Values = new ChartValues<int>(InvoiceMonth);
                    }
                    else if (SelectedTopic == "Import")
                    {
                        lineSeries.Values = new ChartValues<decimal>(TotalImportMonth);
                    }
                    OnPropertyChanged(nameof(ChartZoomCollection));

                    XMax = (to.Month - from.Month) + (to.Year - from.Year) * 12;
                    XFormatterChartZoom = value =>
                    {
                        var date = from.AddMonths((int)value);
                        return date.ToString("MMM yyyy");
                    };

                    var months = Enumerable.Range(0, ((to.Year - from.Year) * 12 + to.Month - from.Month) + 1)
                                            .Select(offset => from.AddMonths(offset).ToString("MMM yyyy"))
                                            .ToList();
                    LabelChartZoom = months;
                    break;

                case "Yearly":
                    if (SelectedTopic == "Revenue")
                    {
                        lineSeries.Values = new ChartValues<decimal>(RevenueYear);
                    }
                    else if (SelectedTopic == "Invoice")
                    {
                        lineSeries.Values = new ChartValues<int>(InvoiceYear);
                    }
                    else if (SelectedTopic == "Import")
                    {
                        lineSeries.Values = new ChartValues<decimal>(TotalImportYear);
                    }
                    OnPropertyChanged(nameof(ChartZoomCollection));

                    XMax = to.Year - from.Year;
                    XFormatterChartZoom = value =>
                    {
                        var year = from.Year + (int)value;
                        return year.ToString();
                    };

                    var years = Enumerable.Range(from.Year, (to.Year - from.Year) + 1)
                                           .Select(year => year.ToString())
                                           .ToList();
                    LabelChartZoom = years;
                    break;
            }

            YFormatterChartZoom = value => value.ToString("N0");
            ZoomingMode = ZoomingOptions.X;
        }

        [RelayCommand]
        public void ChangeTimeChartZoom(string time)
        {
            SelectedTimeChartZoom = time;
            _ = LoadChartZoom(From, To);
        }

        [RelayCommand]
        public void ChangeTopic(string topic)
        {
            SelectedTopic = topic;
            var revenueSeries = ChartZoomCollection.FirstOrDefault(s => s.Title == "Doanh thu")
                           ?? new LineSeries
                           {
                               Title = "Doanh thu",

                               Values = new ChartValues<decimal>(RevenueDay),

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

            var invoiceSeries = ChartZoomCollection.FirstOrDefault(s => s.Title == "Hóa đơn")
                                 ?? new LineSeries
                                 {
                                     Title = "Hóa đơn",
                                     Values = new ChartValues<int>(InvoiceDay),
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
                                     LabelPoint = point => $" {point.Y:N0} Hóa đơn",
                                 };

            var importSeries = ChartZoomCollection.FirstOrDefault(s => s.Title == "Nhập hàng") ?? new LineSeries
            {
                Title = "Nhập hàng",
                Values = new ChartValues<decimal>(TotalImportDay),
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
                LabelPoint = point => $" {point.Y:N0} đơn nhập",
            };

            // Xóa toàn bộ series khỏi ChartZoomCollection
            ChartZoomCollection.Clear();

            // Thêm lại series theo SelectedTopic
            switch (SelectedTopic)
            {
                case "Revenue":
                    ChartZoomCollection.Add(revenueSeries);
                    break;

                case "Invoice":
                    ChartZoomCollection.Add(invoiceSeries);
                    break;

                case "Import":
                    ChartZoomCollection.Add(importSeries);
                    break;
            }
            _ = LoadChartZoom(From, To);
        }

        public async Task LoadColumnSeries(DateTime from, DateTime to)
        {
            if (ChartColumnCollection is null)
            {
                ChartColumnCollection = new SeriesCollection()
                {
                    new ColumnSeries()
                    {
                        Title = "Doanh thu",
                        Values = new ChartValues<decimal>(RevenueMonth),
                        Fill = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#EFBF04")),
                         Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9b442a")),
                        StrokeThickness = 0,
                        LabelPoint = point => $" {point.Y:N0} VNĐ",
                    },

                    new ColumnSeries()
                    {
                        Title = "Lương",
                        Values = new ChartValues<decimal>(TotalSalaryMonth),
                         Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9b442a")),
                        StrokeThickness = 0,
                         Fill = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#1B8366")),
                         LabelPoint = point => $" {point.Y:N0} VNĐ",
                    },
                    new ColumnSeries()
                    {
                        Title = "Vật liệu",
                        Values = new ChartValues<decimal>(TotalMaterialCostMonth),
                         Stroke = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#9b442a")),
                        StrokeThickness = 0,
                         Fill = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#612e0f")),
                         LabelPoint = point => $" {point.Y:N0} VNĐ",
                    }
                };
            }

            ColumnSeries columnSeries = ChartColumnCollection[0] as ColumnSeries;
            ColumnSeries columnSeries1 = ChartColumnCollection[1] as ColumnSeries;
            ColumnSeries columnSeries2 = ChartColumnCollection[2] as ColumnSeries;
            switch (SelectedTimeChartCol)
            {
                case "Monthly":

                    columnSeries.Values = new ChartValues<decimal>(RevenueMonth);
                    columnSeries1.Values = new ChartValues<decimal>(TotalSalaryMonth);
                    columnSeries2.Values = new ChartValues<decimal>(TotalMaterialCostMonth);
                    OnPropertyChanged(nameof(ChartColumnCollection));

                    XMax = (to.Month - from.Month) + (to.Year - from.Year) * 12;
                    XFormatterChartCol = value =>
                    {
                        var date = from.AddMonths((int)value);
                        return date.ToString("MMM yyyy");
                    };

                    var months = Enumerable.Range(0, ((to.Year - from.Year) * 12 + to.Month - from.Month) + 1)
                                            .Select(offset => from.AddMonths(offset).ToString("MMM yyyy"))
                                            .ToList();
                    LabelChartCol = months;

                    break;

                case "Yearly":

                    columnSeries.Values = new ChartValues<decimal>(RevenueYear);
                    columnSeries1.Values = new ChartValues<decimal>(TotalSalaryYear);
                    columnSeries2.Values = new ChartValues<decimal>(TotalMaterialCostYear);
                    OnPropertyChanged(nameof(ChartZoomCollection));

                    XMax = to.Year - from.Year;
                    XFormatterChartCol = value =>
                    {
                        var year = from.Year + (int)value;
                        return year.ToString();
                    };

                    var years = Enumerable.Range(from.Year, (to.Year - from.Year) + 1)
                                           .Select(year => year.ToString())
                                           .ToList();
                    LabelChartCol = years;

                    break;
            }
        }

        [RelayCommand]
        public void ChangeTimeChartCol(string time)
        {
            SelectedTimeChartCol = time;
            _ = LoadColumnSeries(From, To);
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }
    }
}