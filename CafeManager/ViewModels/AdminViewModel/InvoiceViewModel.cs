using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InvoiceViewModel : ObservableObject, IDataViewModel, INotifyDataErrorInfo
    {
        private readonly InvoiceServices _invoiceServices;
        private readonly IMapper _mapper;
        private CancellationToken _token = default;

        private readonly ErrorViewModel _errorViewModel;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ModifyInvoiceViewModel _modifyInvoiceVM;

        [ObservableProperty]
        private bool _isOpenModifyInvoiceVM;

        [ObservableProperty]
        private ObservableCollection<InvoiceDTO> _listInvoiceDTO = [];

        private DateTime? _startDate;

        private Expression<Func<Invoice, bool>> filter => invoice =>
                (invoice.Isdeleted == false) &&
                (StartDate == null || invoice.Paymentstartdate >= StartDate) &&
                (EndDate == null || invoice.Paymentenddate <= EndDate) &&
                (string.IsNullOrEmpty(Status) || invoice.Paymentstatus.Contains(Status)) &&
                (string.IsNullOrEmpty(PaymentMethod) || invoice.Paymentmethod.Contains(PaymentMethod));

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
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
            // Xóa lỗi trước đó cho cả hai trường
            _errorViewModel.RemoveErrors(nameof(StartDate));
            _errorViewModel.RemoveErrors(nameof(EndDate));

            // Thêm lỗi mới nếu StartDate lớn hơn EndDate
            if (StartDate.HasValue && EndDate.HasValue)
            {
                if (StartDate.Value > EndDate.Value)
                {
                    _errorViewModel.AddError(nameof(StartDate), "Ngày bắt đầu không được lớn hơn ngày kết thúc");
                    _errorViewModel.AddError(nameof(EndDate), "Ngày kết thúc không được nhỏ hơn ngày bắt đầu");
                    hasError = true;
                }
            }
            return hasError;
        }

        private DateTime? _endDate;

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    if (!ValidateDates())
                    {
                        OnPropertyChanged();
                        _ = LoadData(_token);
                    }
                }
            }
        }

        private string? _status;

        public string? Status
        {
            get => _status; set
            {
                if (_status != value)
                {
                    _status = value;

                    OnPropertyChanged();
                    _ = LoadData(_token);
                }
            }
        }

        private string? _paymentMethod;

        public string? PaymentMethod
        {
            get => _paymentMethod; set
            {
                if (_paymentMethod != value)
                {
                    _paymentMethod = value;
                    OnPropertyChanged();
                    _ = LoadData(_token);
                }
            }
        }

        public InvoiceViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            ModifyInvoiceVM = provider.GetRequiredService<ModifyInvoiceViewModel>();
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
                _token = token;
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListInvoice = await _invoiceServices.GetSearchPaginateListInvoice(filter, pageIndex, pageSize, token);
                ListInvoiceDTO = [.. _mapper.Map<List<InvoiceDTO>>(dbListInvoice.Item1)];
                TotalPages = (dbListInvoice.Item2 + pageSize - 1) / pageSize;
                OnPropertyChanged(nameof(PageUI));
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của InvoiceViewModel bị hủy");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void InfoInvoice(InvoiceDTO invoiceDTO)
        {
            IsOpenModifyInvoiceVM = true;
            ModifyInvoiceVM.RecieveInvoiceDTO(invoiceDTO.Clone());
        }

        [RelayCommand]
        private async Task DeleteInvoice(InvoiceDTO invoiceDTO)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn xóa hóa đơn này không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    IsLoading = true;
                    bool isSuccessDeleted = await _invoiceServices.DeleteInvoice(invoiceDTO.Invoiceid);
                    if (isSuccessDeleted)
                    {
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Xóa hóa đơn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                        await LoadData(_token);
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private void CloseModifyInvoiceView()
        {
            IsOpenModifyInvoiceVM = false;
        }

        #region Phân trang

        private int pageIndex = 1;

        private int pageSize = 15;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PageVisibility))]
        private int totalPages = 0;

        public bool PageVisibility => TotalPages > 0;

        public string PageUI => $"{pageIndex}/{TotalPages}";

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;
            await LoadData(_token);
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == TotalPages)
            {
                return;
            }
            pageIndex += 1;
            await LoadData(_token);
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            await LoadData(_token);
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = TotalPages;
            await LoadData(_token);
        }

        #endregion Phân trang

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

        public bool HasErrors => _errorViewModel.HasErrors;
    }
}