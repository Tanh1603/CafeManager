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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class ImportViewModel : ObservableObject, IDataViewModel
    {
        private readonly ImportServices _importServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly StaffServices _staffServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isOpenModifyImportView;

        [ObservableProperty]
        private AddImportViewModel _modifyImportVM;

        private ObservableCollection<ImportDTO> ListImport = [];
        public ObservableCollection<ImportDTO> CurrentListImport => [.. _filterListImport];

        private List<ImportDTO> _filterListImport = [];
        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    FilterImport();
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public ImportViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _importServices = provider.GetRequiredService<ImportServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = provider.GetRequiredService<IMapper>();

            ModifyImportVM = provider.GetRequiredService<AddImportViewModel>();
            ModifyImportVM.ImportChanged += ModifyImportVM_ImportChanged;
            _isOpenModifyImportView = false;
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var dbListStaff = await _staffServices.GetListStaff();
                ModifyImportVM.ListStaff = [.. _mapper.Map<List<StaffDTO>>(dbListStaff)];

                var dbListSupplier = (await _materialSupplierServices.GetListSupplier()).Where(x => x.Isdeleted == false);
                ModifyImportVM.ListSupplier = [.. _mapper.Map<List<SupplierDTO>>(dbListSupplier)];

                var dbListMaterial = (await _materialSupplierServices.GetListMaterial()).Where(x => x.Isdeleted == false);
                ModifyImportVM.ListMaterial = [.. _mapper.Map<List<MaterialDTO>>(dbListMaterial)];

                var importList = (await _importServices.GetListImport())?.Where(x => x.Isdeleted == false);
                ListImport = [.. _mapper.Map<List<ImportDTO>>(importList)];

                _filterListImport = [.. ListImport];
                OnPropertyChanged(nameof(CurrentListImport));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của ImportViewModel bị hủy");
            }
        }

        private void FilterImport()
        {
            var filter = ListImport.Where(x => string.IsNullOrWhiteSpace(SearchText)
            || x.Supplier.Suppliername.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            || x.Receiveddate.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            || x.Staff.Staffname.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            _filterListImport = [.. filter];
            OnPropertyChanged(nameof(CurrentListImport));
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
                    var isDeleted = await _importServices.DeleteImport(import.Importid);
                    if (isDeleted)
                    {
                        ListImport.Remove(import);
                    }
                    MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
            FilterImport();
        }

        private async void ModifyImportVM_ImportChanged(ImportDTO import)
        {
            try
            {
                if (ModifyImportVM.IsAdding)
                {
                    Import? addImport = await _importServices.AddImport(_mapper.Map<Import>(import));

                    if (addImport != null)
                    {
                        ListImport.Add(_mapper.Map<ImportDTO>(addImport));
                        MyMessageBox.Show("Thêm thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                if (ModifyImportVM.IsUpdating)
                {
                    if (import != null)
                    {
                        var updateImport = await _importServices.UpdateImport(_mapper.Map<Import>(import));
                        var res = ListImport.FirstOrDefault(x => x.Importid == import.Importid);
                        _mapper.Map(updateImport, res);
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                IsOpenModifyImportView = false;
                ModifyImportVM.ClearValueOfViewModel();
                FilterImport();
            }
            catch
            {
                MyMessageBox.Show("Lỗi", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        public void Dispose()
        {
            ModifyImportVM.ImportChanged -= ModifyImportVM_ImportChanged;
            GC.SuppressFinalize(this);
        }
    }
}