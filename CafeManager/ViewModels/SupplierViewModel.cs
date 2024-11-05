using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace CafeManager.WPF.ViewModels
{
    public partial class SupplierViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MaterialSupplierServices _materialSupplierServices;

        [ObservableProperty]
        private bool _isOpenAddSupplier;

        [ObservableProperty]
        private ObservableCollection<Supplier> _listSupplier = [];

        [ObservableProperty]
        private AddSuppierViewModel _addSupplierVM;

        public SupplierViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();

            AddSupplierVM = _serviceProvider.GetRequiredService<AddSuppierViewModel>();
            AddSupplierVM.AddSupplierChanged += async (supplier) =>
            {
                try
                {
                    IsOpenAddSupplier = false;
                    var addedSupplier = await _materialSupplierServices.AddSupplier(supplier);
                    ListSupplier.Add(addedSupplier);

                    MessageBox.Show("Thêm nhà cung cấp thành công");
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
            AddSupplierVM.UpdateSupplierNameChanged += async (supplier) =>
            {
                try
                {
                    IsOpenAddSupplier = false;
                    Supplier oldSupplier = await _materialSupplierServices.GetSupplierById(supplier.Supplierid);
                    Supplier? tmp = ListSupplier.FirstOrDefault(x => x.Supplierid == supplier.Supplierid);

                    oldSupplier.Suppliername = supplier.Suppliername;
                    oldSupplier.Representativesupplier = supplier.Representativesupplier;
                    oldSupplier.Address = supplier.Address;
                    oldSupplier.Email = supplier.Email;
                    oldSupplier.Phone = supplier.Phone;

                    var newSupplier = _materialSupplierServices.UpdateSupplier(oldSupplier);
                    if (newSupplier != null)
                    {
                        tmp = newSupplier;
                        ListSupplier = new(ListSupplier);
                        MessageBox.Show("Cập nhật nhà cung cấp thành công");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            };
            AddSupplierVM.Close += () => IsOpenAddSupplier = false;

            _ = LoadData();
        }

        private async Task LoadData()
        {
            var list = await _materialSupplierServices.GetListSupplier();
            ListSupplier = new ObservableCollection<Supplier>(list);
        }

        [RelayCommand]
        private void OpenAddSupplier()
        {
            IsOpenAddSupplier = true;
            AddSupplierVM.IsAdding = true;
            AddSupplierVM.IsUpdating = false;

            AddSupplierVM.Suppliername = string.Empty;
            AddSupplierVM.Representativesupplier = string.Empty;
            AddSupplierVM.Address = string.Empty;
            AddSupplierVM.Email = string.Empty;
            AddSupplierVM.Phone = string.Empty;
            AddSupplierVM.Notes = string.Empty;
        }

        private bool _isUpdateSupplierChangedRegistered = false;

        [RelayCommand]
        private void UpdateSupplier(Supplier supplier)
        {
            IsOpenAddSupplier = true;
            AddSupplierVM.HandleSupplierFromParent(supplier);
            AddSupplierVM.IsUpdating = true;
            AddSupplierVM.IsAdding = false;
        }

        [RelayCommand]
        private async Task DeleteSupplier(Supplier supplier)
        {
            try
            {
                var isDeleted = await _materialSupplierServices.DeleteSupplier(supplier.Supplierid);
                if (isDeleted)
                {
                    ListSupplier.Remove(supplier);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MessageBox.Show(ivd.Message);
            }
        }

        public void Dispose()
        {
            if (AddSupplierVM != null)
            {
                AddSupplierVM.AddSupplierChanged -= async (supplier) =>
                {
                    try
                    {
                        IsOpenAddSupplier = false;
                        var addedSupplier = await _materialSupplierServices.AddSupplier(supplier);
                        ListSupplier.Add(addedSupplier);
                        MessageBox.Show("Thêm nhà cung cấp thành công");
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                };
                AddSupplierVM.UpdateSupplierNameChanged -= async (supplier) =>
                {
                    try
                    {
                        IsOpenAddSupplier = false;
                        Supplier oldSupplier = await _materialSupplierServices.GetSupplierById(supplier.Supplierid);
                        oldSupplier.Suppliername = supplier.Suppliername;
                        oldSupplier.Representativesupplier = supplier.Representativesupplier;
                        oldSupplier.Address = supplier.Address;
                        oldSupplier.Email = supplier.Email;
                        oldSupplier.Phone = supplier.Phone;

                        var newSupplier = _materialSupplierServices.UpdateSupplier(oldSupplier);
                        MessageBox.Show("Cập nhật nhà cung cấp thành công");
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                };
                AddSupplierVM.Close -= () => IsOpenAddSupplier = false;
                GC.SuppressFinalize(this);
            }
        }
    }
}