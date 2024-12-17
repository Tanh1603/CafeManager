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
    public partial class SupplierViewModel : ObservableObject, IDisposable, IDataViewModel
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isOpenAddSupplier = false;

        private List<SupplierDTO> AllSupplier = [];

        private List<SupplierDTO> _filterSupplierList = [];

        public ObservableCollection<SupplierDTO> ListExistedSupplier => [.. _filterSupplierList.Where(x => x.Isdeleted == false) ?? []];

        public ObservableCollection<SupplierDTO> ListDeletedSupplier => [.. _filterSupplierList.Where(x => x.Isdeleted == true) ?? []];

        [ObservableProperty]
        private AddSuppierViewModel _modifySupplierVM;

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    FilterSupplier();
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public SupplierViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            ModifySupplierVM = provider.GetRequiredService<AddSuppierViewModel>();
            ModifySupplierVM.ModifySupplierChanged += ModifySupplierVM_ModifySupplierChanged;
            ModifySupplierVM.Close += ModifySupplierVM_Close;
        }


        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                AllSupplier = _mapper.Map<List<SupplierDTO>>(await _materialSupplierServices.GetListSupplier());
                _filterSupplierList = _mapper.Map<List<SupplierDTO>>(AllSupplier);

                OnPropertyChanged(nameof(ListExistedSupplier));
                OnPropertyChanged(nameof(ListDeletedSupplier));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của SupplierViewModel bị hủy");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void FilterSupplier()
        {
            var filtered = AllSupplier.Where(x =>
                string.IsNullOrWhiteSpace(SearchText) ||
                x.Suppliername.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                x.Representativesupplier.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                x.Phone.Contains(SearchText) ||
                x.Email.Contains(SearchText) ||
                x.Address.Contains(SearchText) ||
                x.Notes.Contains(SearchText)
                ).ToList();
            _filterSupplierList = [.. filtered];
            OnPropertyChanged(nameof(ListExistedSupplier));
            OnPropertyChanged(nameof(ListDeletedSupplier));
        }

        private async void ModifySupplierVM_ModifySupplierChanged(SupplierDTO obj)
        {
            try
            {
                IsLoading = true;
                if (ModifySupplierVM.IsAdding)
                {
                    var addSupplier = await _materialSupplierServices.AddSupplier(_mapper.Map<Supplier>(obj));
                    if (addSupplier != null)
                    {
                        AllSupplier.Add(_mapper.Map<SupplierDTO>(addSupplier));
                        FilterSupplier();
                        ModifySupplierVM.ClearValueOfFrom();
                        IsOpenAddSupplier = false;
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công");
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm nhà cung cấp thất bại");
                    }
                }
                if (ModifySupplierVM.IsUpdating)
                {
                    var res = await _materialSupplierServices.UpdateSupplier(_mapper.Map<Supplier>(obj));
                    if (res != null)
                    {
                        var updateSupplierDTO = AllSupplier.FirstOrDefault(x => x.Supplierid == res.Supplierid);
                        if (updateSupplierDTO != null)
                        {
                            _mapper.Map(res, updateSupplierDTO);
                            FilterSupplier();
                            ModifySupplierVM.ClearValueOfFrom();
                            IsOpenAddSupplier = false;
                            IsLoading = false;
                            MyMessageBox.ShowDialog("Sửa nhà cung cấp thành công");
                        }
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa nhà cung cấp thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void OpenAddSupplier()
        {
            IsOpenAddSupplier = true;
            ModifySupplierVM.IsAdding = true;
        }

        [RelayCommand]
        private void OpenUpdateSupplier(SupplierDTO supplier)
        {
            IsOpenAddSupplier = true;
            ModifySupplierVM.IsUpdating = true;
            ModifySupplierVM.RecieveSupplierDTO(supplier);
        }

        //[RelayCommand]
        //private void CloseModifySupplier()
        //{
        //    IsOpenAddSupplier = false;
        //    ModifySupplierVM.ClearValueOfFrom();
        //}

        private void ModifySupplierVM_Close()
        {
            IsOpenAddSupplier = false;
            ModifySupplierVM.ClearValueOfFrom();
        }


        [RelayCommand]
        private async Task DeleteSupplier(SupplierDTO supplier)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn ẩn nhà cung cấp không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    IsLoading = true;
                    bool isDeleted = await _materialSupplierServices.DeleteSupplier(supplier.Supplierid);
                    if (isDeleted)
                    {
                        var deleted = AllSupplier.First(x => x.Supplierid == supplier.Supplierid);
                        deleted.Isdeleted = true;
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Ẩn nhà cung cấp thanh công");
                        FilterSupplier();
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Ẩn nhà cung cấp thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task RestoreSupplier(SupplierDTO supplier)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn hiển thị nhà cung cấp không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    IsLoading = true;
                    supplier.Isdeleted = false;
                    var res = await _materialSupplierServices.UpdateSupplier(_mapper.Map<Supplier>(supplier));
                    if (res != null)
                    {
                        var restore = AllSupplier.First(x => x.Supplierid == supplier.Supplierid);
                        restore.Isdeleted = false;
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Hiển thị nhà cung cấp thành công");
                        FilterSupplier();
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Hiển thị nhà cung cấp thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        public void Dispose()
        {
            if (ModifySupplierVM != null)
            {
                ModifySupplierVM.ModifySupplierChanged -= ModifySupplierVM_ModifySupplierChanged;
            }
            GC.SuppressFinalize(this);
        }
    }
}