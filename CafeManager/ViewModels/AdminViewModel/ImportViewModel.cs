﻿using AutoMapper;
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;

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

        [ObservableProperty]
        private ObservableCollection<ImportDTO> _listImportDTO = [];
        //public ObservableCollection<ImportDTO> CurrentListImport => [.. _filterListImport];

        //private List<ImportDTO> _filterListImport = [];


        #region Filter Declare
        private string SearchText = string.Empty;

        [RelayCommand]
        private void Search(string searchText)
        {
            // Logic xử lý tìm kiếm
            SearchText = searchText; // Cập nhật nếu cần
            _ = LoadImport();
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
                    OnPropertyChanged();
                    _ = LoadImport();
                }
            }
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
                    OnPropertyChanged();
                    _ = LoadImport();
                }
            }
        }
        #endregion

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

                await LoadImport();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của ImportViewModel bị hủy");
            }
        }

        private async Task LoadImport()
        {
            Expression<Func<Import, bool>> filter = import =>
                (import.Isdeleted == false) &&
                (StartDate == null || import.Receiveddate >= StartDate) &&
                (EndDate == null || import.Receiveddate <= EndDate) &&
                (string.IsNullOrEmpty(SearchText) ||
                    import.Supplier.Suppliername.Contains(SearchText) ||
                    import.Staff.Staffname.Contains(SearchText));

            var dbListImport = await _importServices.GetSearchPaginateListImport(filter, pageIndex, pageSize);
            ListImportDTO = [.. _mapper.Map<List<ImportDTO>>(dbListImport.Item1)];
            totalPages = (dbListImport.Item2 + pageSize - 1) / pageSize;
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
                    var isDeleted = await _importServices.DeleteImport(import.Importid);
                    if (isDeleted)
                    {
                        ListImportDTO.Remove(import);
                    }
                    MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
            await LoadImport();
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
                        ListImportDTO.Add(_mapper.Map<ImportDTO>(addImport));
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
                        var res = ListImportDTO.FirstOrDefault(x => x.Importid == import.Importid);
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
                await LoadImport();
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


        #region Phan trang
        private int pageIndex = 1;

        private int pageSize = 10;
        private int totalPages = 0;

        public string PageUI => $"{pageIndex}/{totalPages}";

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;

            await LoadImport();
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == totalPages)
            {
                return;
            }
            pageIndex += 1;
            await LoadImport();
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            await LoadImport();
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = totalPages;
            await LoadImport();
        }
        #endregion
    }
}