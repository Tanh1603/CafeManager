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
                    var updateSupplier = await _materialSupplierServices.GetSupplierById(obj.Supplierid);
                    if (updateSupplier != null)
                    {
                        updateSupplier.Supplierid = obj.Supplierid;
                        updateSupplier.Suppliername = obj.Suppliername;
                        updateSupplier.Representativesupplier = obj.Representativesupplier;
                        updateSupplier.Phone = obj.Phone;
                        updateSupplier.Email = obj.Email;
                        updateSupplier.Address = obj.Address;
                        updateSupplier.Notes = obj.Notes;
                        updateSupplier.Isdeleted = obj.Isdeleted;

                        var res = await _materialSupplierServices.UpdateSupplier(updateSupplier);
                        var updateSupplierDTO = ListSupplier.FirstOrDefault(x => x.Supplierid == res?.Supplierid);
                        if (updateSupplierDTO != null && res != null)
                        {
                            updateSupplierDTO.Supplierid = res.Supplierid;
                            updateSupplierDTO.Suppliername = res.Suppliername;
                            updateSupplierDTO.Representativesupplier = res.Representativesupplier;
                            updateSupplierDTO.Phone = res.Phone;
                            updateSupplierDTO.Email = res.Email;
                            updateSupplierDTO.Address = res.Address;
                            updateSupplierDTO.Notes = res.Notes;
                            updateSupplierDTO.Isdeleted = res.Isdeleted;

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
            var list = await _materialSupplierServices.GetListSupplier();
            ListSupplier = [.. _mapper.Map<List<SupplierDTO>>(list)];
        }

        [RelayCommand]
        private void OpenAddSupplier()
        {
            IsOpenAddSupplier = true;
            ModifySupplierVM.IsAdding = true;
        }

        private bool _isUpdateSupplierChangedRegistered = false;

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
                bool isDeleted = await _materialSupplierServices.DeleteSupplier(supplier.Supplierid);
                if (isDeleted)
                {
                    ListSupplier.Remove(supplier);
                    MyMessageBox.ShowDialog("Xóa nhà cung cấp thanh công");
                }
                else
                {
                    MyMessageBox.ShowDialog("Xóa nhà cung cấp thất bại");
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
            }
            GC.SuppressFinalize(this);
        }
    }
}