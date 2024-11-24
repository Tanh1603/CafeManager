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

        [ObservableProperty]
        private ObservableCollection<SupplierDTO> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<SupplierDTO> _listDeletedSupplier = [];

        [ObservableProperty]
        private AddSuppierViewModel _modifySupplierVM;

        public SupplierViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
            ModifySupplierVM = _serviceProvider.GetRequiredService<AddSuppierViewModel>();
            ModifySupplierVM.ModifySupplierChanged += ModifySupplierVM_ModifySupplierChanged;
            Task.Run(LoadData);
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
                        ListSupplier.Add(_mapper.Map<SupplierDTO>(addSupplier));
                        MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công");
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
                    var res = await _materialSupplierServices.UpdateSupplierById(obj.Supplierid, _mapper.Map<Supplier>(obj));
                    if (res != null)
                    {
                        var updateSupplierDTO = ListSupplier.FirstOrDefault(x => x.Supplierid == res.Supplierid);
                        if (updateSupplierDTO != null)
                        {
                            _mapper.Map(res, updateSupplierDTO);
                            MyMessageBox.ShowDialog("Sửa nhà cung cấp thành công");
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

        private async Task LoadData()
        {
            var all = await _materialSupplierServices.GetListSupplier();

            var listExisted = all.ToList().Where(x => x.Isdeleted == false);
            var listDeleted = all.ToList().Where(x => x.Isdeleted == true);

            ListSupplier = [.. _mapper.Map<List<SupplierDTO>>(listExisted)];
            ListDeletedSupplier = [.. _mapper.Map<List<SupplierDTO>>(listDeleted)];
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
                        ListSupplier.Remove(supplier);
                        ListDeletedSupplier.Add(supplier);
                        MyMessageBox.ShowDialog("Ẩn nhà cung cấp thanh công");
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
                    var res = await _materialSupplierServices.UpdateSupplierById(supplier.Supplierid, _mapper.Map<Supplier>(supplier));
                    if (res != null)
                    {
                        ListSupplier.Add(supplier);
                        ListDeletedSupplier.Remove(supplier);
                        MyMessageBox.ShowDialog("Hiển thị nhà cung cấp thanh công");
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