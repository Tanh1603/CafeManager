using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using CafeManager.WPF.MessageBox;

namespace CafeManager.WPF.ViewModels.AdminViewModel
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
            AddSupplierVM.AddSupplierChanged += AddSupplierVM_AddSupplierChanged;
            AddSupplierVM.UpdateSupplierNameChanged += AddSupplierVM_UpdateSupplierNameChanged;
            AddSupplierVM.Close += AddSupplierVM_Close; ;

            _ = LoadData();
        }

        private void AddSupplierVM_Close()
        {
            IsOpenAddSupplier = false;
        }

        private async void AddSupplierVM_UpdateSupplierNameChanged(Supplier obj)
        {
            //try
            //{
            //    IsOpenAddSupplier = false;
            //    Supplier oldSupplier = await _materialSupplierServices.GetSupplierById(obj.Supplierid);
            //    Supplier? tmp = ListSupplier.FirstOrDefault(x => x.Supplierid == obj.Supplierid);

            //    oldSupplier.Suppliername = obj.Suppliername;
            //    oldSupplier.Representativesupplier = obj.Representativesupplier;
            //    oldSupplier.Address = obj.Address;
            //    oldSupplier.Email = obj.Email;
            //    oldSupplier.Phone = obj.Phone;
            //    oldSupplier.Notes = obj.Notes;

            //    var newSupplier = _materialSupplierServices.UpdateSupplier(oldSupplier);
            //    if (newSupplier != null)
            //    {
            //        tmp = newSupplier;
            //        ListSupplier = new(ListSupplier);
            //        MyMessageBox.Show("Cập nhật nhà cung cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            //    }
            //}
            //catch (InvalidOperationException ex)
            //{
            //    MyMessageBox.Show(ex.Message, MyMessageBox.Buttons.OK,MyMessageBox.Icons.Warning);
            //}
        }

        private async void AddSupplierVM_AddSupplierChanged(Supplier obj)
        {
            try
            {
                IsOpenAddSupplier = false;
                var addedSupplier = await _materialSupplierServices.AddSupplier(obj);
                ListSupplier.Add(addedSupplier);

                MyMessageBox.Show("Thêm nhà cung cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            catch (InvalidOperationException ex)
            {
                MyMessageBox.Show(ex.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
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
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        public void Dispose()
        {
            if (AddSupplierVM != null)
            {
                AddSupplierVM.AddSupplierChanged -= AddSupplierVM_AddSupplierChanged;
                AddSupplierVM.UpdateSupplierNameChanged -= AddSupplierVM_UpdateSupplierNameChanged;
                AddSupplierVM.Close -= AddSupplierVM_Close;
            }
            GC.SuppressFinalize(this);
        }
    }
}