using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class SupplierViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private IMapper _mapper;

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

        public SupplierViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            ModifySupplierVM = _serviceProvider.GetRequiredService<AddSuppierViewModel>();
            ModifySupplierVM.ModifySupplierChanged += ModifySupplierVM_ModifySupplierChanged;
            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            AllSupplier = _mapper.Map<List<SupplierDTO>>(await _materialSupplierServices.GetListSupplier());
            _filterSupplierList = _mapper.Map<List<SupplierDTO>>(AllSupplier);

            OnPropertyChanged(nameof(ListExistedSupplier));
            OnPropertyChanged(nameof(ListDeletedSupplier));
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
                if (ModifySupplierVM.IsAdding)
                {
                    var addSupplier = await _materialSupplierServices.AddSupplier(_mapper.Map<Supplier>(obj));
                    if (addSupplier != null)
                    {
                        AllSupplier.Add(_mapper.Map<SupplierDTO>(addSupplier));
                        MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công");
                        FilterSupplier();
                        ModifySupplierVM.ClearValueOfFrom();
                        IsOpenAddSupplier = false;
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
                            MyMessageBox.ShowDialog("Sửa nhà cung cấp thành công");
                            FilterSupplier();
                            ModifySupplierVM.ClearValueOfFrom();
                            IsOpenAddSupplier = false;
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

        [RelayCommand]
        private void CloseModifySupplier()
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
                    bool isDeleted = await _materialSupplierServices.DeleteSupplier(supplier.Supplierid);
                    if (isDeleted)
                    {
                        var deleted = AllSupplier.First(x => x.Supplierid == supplier.Supplierid);
                        deleted.Isdeleted = true;
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
        }

        [RelayCommand]
        private async Task RestoreSupplier(SupplierDTO supplier)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn hiển thị nhà cung cấp không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    supplier.Isdeleted = false;
                    var res = await _materialSupplierServices.UpdateSupplier(_mapper.Map<Supplier>(supplier));
                    if (res != null)
                    {
                        var restore = AllSupplier.First(x => x.Supplierid == supplier.Supplierid);
                        restore.Isdeleted = false;
                        MyMessageBox.ShowDialog("Hiển thị nhà cung cấp thanh công");
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