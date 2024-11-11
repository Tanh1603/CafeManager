using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class ImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ImportServices _importServices;
        private readonly ImportDetailServices _importDetailServices;
        private readonly MaterialSupplierServices _materialSupplierServices;

        [ObservableProperty]
        private bool _isOpenAddImport;

        [ObservableProperty]
        private ObservableCollection<Import> _listImport = [];

        //[ObservableProperty]
        //private ObservableCollection<MaterialDetailDTO> _listMaterialDetailDTO = [];

        //[ObservableProperty]
        //private ObservableCollection<Supplier> _listSupplier = [];

        //[ObservableProperty]
        //private ObservableCollection<Material> _listMaterial = [];

        [ObservableProperty]
        private AddImportViewModel _addImportVM;

        public ImportViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;
             
            _importServices = provider.GetRequiredService<ImportServices>();
            _importDetailServices = provider.GetRequiredService<ImportDetailServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();

            AddImportVM = _serviceProvider.GetRequiredService<AddImportViewModel>();
            AddImportVM.AddImportChanged += AddImportVM_AddImportChanged;
            AddImportVM.Close += AddImportVM_Close;


            _ = LoadData();
        }

        private async Task LoadData()
        {
            var supplierList = await _materialSupplierServices.GetListSupplier();
            var materialList = await _materialSupplierServices.GetListMaterial();
            var listimport = await _importServices.GetListImport();

            ListImport = new ObservableCollection<Import>(listimport);
            AddImportVM.ListSupplier = new ObservableCollection<Supplier>(supplierList);
            AddImportVM.ListMaterial = new ObservableCollection<Material>(materialList);
        }

        public async Task LoadImportDetails(int importId)
        {
            var listimportdetails = await _importServices.GetListImportDetailByImportId(importId);
            AddImportVM.ListMaterialDetailDTO = new ObservableCollection<MaterialDetailDTO>(listimportdetails);
        }

        private void AddImportVM_Close()
        {
            IsOpenAddImport = false;
        }

        private async void AddImportVM_AddImportChanged(List<MaterialDetailDTO> materialdetailDTOs, Import addimport, Material addmaterial, Supplier addsupplier)
        {
            try
            {
                IsOpenAddImport = false;
                await _importDetailServices.AddImportDetailArange(materialdetailDTOs, addimport, addmaterial, addsupplier);
                ListImport.Add(addimport);
                MessageBox.Show("Thêm chi tiết phiếu nhập cấp thành công");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [RelayCommand]
        public void OpenAddImport()
        {
            IsOpenAddImport = true;
            AddImportVM.IsAdding = true;

            AddImportVM.Deliveryperson = string.Empty;
            AddImportVM.Phone = string.Empty;
            AddImportVM.Shippingcompany = string.Empty;
            AddImportVM.Receiveddate = DateTime.Now;
            AddImportVM.Receiver = string.Empty;

            AddImportVM.ManufactureDate = DateTime.Now;
            AddImportVM.ExpirationDate = DateTime.Now;
            AddImportVM.Original = string.Empty;
            AddImportVM.Manufacturer = string.Empty;
        }


        private bool _isUpdateImportChangedRegistered = false;

        [RelayCommand]
        private void OpenImportDetail (Import selectedImport)
        {
            if (selectedImport != null)
            {
                IsOpenAddImport = true;

                AddImportVM.IsAdding = false;

                AddImportVM.HandleImportFromParent(selectedImport);

                _ = LoadImportDetails(selectedImport.Importid);

                AddImportVM.IsAdding = false;
            }
        }

        [RelayCommand]
        private async Task DeleteImport(Import import)
        {
            try
            {
                var isDeleted = await _importServices.DeleteImport(import.Importid);
                if (isDeleted)
                {
                    ListImport.Remove(import);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MessageBox.Show(ivd.Message);
            }
        }

        public void Dispose()
        {
            if (AddImportVM != null)
            {
                AddImportVM.AddImportChanged -= AddImportVM_AddImportChanged;
                AddImportVM.Close -= AddImportVM_Close;

            }
            GC.SuppressFinalize(this);
        }
    }
}