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
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class ImportViewModel : ObservableObject, IDataViewModel, INotifyDataErrorInfo
    {
        private readonly ImportServices _importServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly StaffServices _staffServices;
        private readonly IMapper _mapper;
        private readonly ErrorViewModel _errorViewModel;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isOpenModifyImportView;

        [ObservableProperty]
        private AddImportViewModel _modifyImportVM;

        [ObservableProperty]
        private ObservableCollection<ImportDTO> _listImportDTO = [];

        #region Filter Declare

        private SupplierDTO? _selectedSupplier;

        public SupplierDTO? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    OnPropertyChanged(nameof(SelectedSupplier));
                    //_ = FirstPage();
                    FirstPageCommand.ExecuteAsync(null);
                }
            }
        }

        private StaffDTO? _selectedStaff;

        public StaffDTO? SelectedStaff
        {
            get => _selectedStaff;
            set
            {
                if (_selectedStaff != value)
                {
                    _selectedStaff = value;

                    OnPropertyChanged(nameof(SelectedStaff));
                    //_ = FirstPage();
                    FirstPageCommand.ExecuteAsync(null);
                }
            }
        }

        private DateTime? _startDate;

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
                        //_ = FirstPage();
                        FirstPageCommand.ExecuteAsync(null);
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
                        //_ = FirstPage();
                        FirstPageCommand.ExecuteAsync(null);
                    }
                }
            }
        }

        #endregion Filter Declare

        public ImportViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _importServices = provider.GetRequiredService<ImportServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = provider.GetRequiredService<IMapper>();

            ModifyImportVM = provider.GetRequiredService<AddImportViewModel>();
            ModifyImportVM.ImportChanged += ModifyImportVM_ImportChanged;
            _errorViewModel = new ErrorViewModel();
            _errorViewModel.ErrorsChanged += _errorViewModel_ErrorsChanged;
            _isOpenModifyImportView = false;
        }

        private void _errorViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListStaff = await _staffServices.GetListExistStaff(token);
                ModifyImportVM.ListStaff = [.. _mapper.Map<List<StaffDTO>>(dbListStaff)];

                var dbListSupplier = (await _materialSupplierServices.GetListSupplier(token)).Where(x => x.Isdeleted == false);
                ModifyImportVM.ListSupplier = [.. _mapper.Map<List<SupplierDTO>>(dbListSupplier)];

                var dbListMaterial = (await _materialSupplierServices.GetListMaterial(token)).Where(x => x.Isdeleted == false);
                ModifyImportVM.ListMaterial = [.. _mapper.Map<List<MaterialDTO>>(dbListMaterial)];

                await LoadImport();
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của ImportViewModel bị hủy");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadImport()
        {
            Expression<Func<Import, bool>> filter = import =>
                (import.Isdeleted == false) &&
                (StartDate == null || import.Receiveddate >= StartDate) &&
                (EndDate == null || import.Receiveddate <= EndDate) &&
                (SelectedSupplier == null || import.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                (SelectedStaff == null || import.Staff.Staffid == SelectedStaff.Staffid);

            var dbListImport = await _importServices.GetSearchPaginateListImport(filter, pageIndex, pageSize);
            ListImportDTO = [.. _mapper.Map<List<ImportDTO>>(dbListImport.Item1)];
            TotalPages = (dbListImport.Item2 + pageSize - 1) / pageSize;
            OnPropertyChanged(nameof(PageUI));
        }

        [RelayCommand]
        private void OpenModifyImport(ImportDTO importDTO)
        {
            IsOpenModifyImportView = true;
            ModifyImportVM.IsAdding = true;
            ModifyImportVM.IsAddingImportDetail = true;
        }

        [RelayCommand]
        private void CloseModifyImport()
        {
            IsOpenModifyImportView = false;
            ModifyImportVM.ClearValueOfViewModel();
        }

        [RelayCommand]
        private void OpenUpdateImport(ImportDTO import)
        {
            ModifyImportVM.RecieveImport(import.Clone());
            IsOpenModifyImportView = true;
            ModifyImportVM.IsUpdating = true;
            ModifyImportVM.IsAddingImportDetail = true;
        }

        [RelayCommand]
        private async Task DeleteImport(ImportDTO import)
        {
            try
            {
                var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
                if (res.Equals("1"))
                {
                    IsLoading = true;
                    var isDeleted = await _importServices.DeleteImport(import.Importid);
                    if (isDeleted)
                    {
                        ListImportDTO.Remove(import);
                    }
                    MyMessageBox.ShowDialog("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.ShowDialog(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
            finally
            {
                await LoadImport();
                IsLoading = false;
            }
        }

        private async void ModifyImportVM_ImportChanged(ImportDTO import)
        {
            try
            {
                IsLoading = true;
                if (ModifyImportVM.IsAdding)
                {
                    Import? addImport = await _importServices.AddImport(_mapper.Map<Import>(import));

                    if (addImport != null)
                    {
                        ListImportDTO.Add(_mapper.Map<ImportDTO>(addImport));
                        await LoadImport();
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Thêm thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Thêm thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                if (ModifyImportVM.IsUpdating)
                {
                    if (import != null)
                    {
                        var updateImport = await _importServices.UpdateImport(_mapper.Map<Import>(import));
                        var res = ListImportDTO.FirstOrDefault(x => x.Importid == import.Importid);
                        _mapper.Map(updateImport, res);
                        await LoadImport();
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Sửa thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                IsOpenModifyImportView = false;
                ModifyImportVM.ClearValueOfViewModel();
            }
            catch
            {
                MyMessageBox.ShowDialog("Lỗi", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void Dispose()
        {
            ModifyImportVM.ImportChanged -= ModifyImportVM_ImportChanged;
            GC.SuppressFinalize(this);
        }

        #region Phan trang

        private int pageIndex = 1;

        private int pageSize = 10;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PageVisibility))]
        private int totalPages = 0;

        public bool PageVisibility => TotalPages > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public string PageUI => $"{pageIndex}/{TotalPages}";

        public bool HasErrors => _errorViewModel.HasErrors;

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;
            IsLoading = true;
            await LoadImport();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == TotalPages)
            {
                return;
            }
            pageIndex += 1;
            IsLoading = true;
            await LoadImport();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            IsLoading = true;
            await LoadImport();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = TotalPages;
            IsLoading = true;
            await LoadImport();
            IsLoading = false;
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorViewModel.GetErrors(propertyName);
        }

        #endregion Phan trang
    }
}