using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class TestImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly ImportServices _importServices;
        private readonly ImportDetailServices _importDetailServices;
        private readonly MaterialSupplierServices _materialSupplierServices;

        #region Property Mở modal, dialog

        [ObservableProperty]
        private bool _isOpenImportDetailList;

        [ObservableProperty]
        private bool _isOpenAddImportView;

        #endregion Property Mở modal, dialog

        #region Property hiển thị thông tin

        [ObservableProperty]
        private ObservableCollection<Import> _importList = new();

        [ObservableProperty]
        private ObservableCollection<MaterialDetailDTO> _importDetailListById = new();

        [ObservableProperty]
        private ObservableCollection<Material> _materialChoice = new();

        [ObservableProperty]
        private ObservableCollection<Supplier> _supplierChoice = new();

        [ObservableProperty]
        private ObservableCollection<MaterialDetailDTO> _currentImportDetailList = new();

        #endregion Property hiển thị thông tin

        #region Property binding lấy dữ liệu ui

        // import add property
        [ObservableProperty]
        private string _deliveryperson;

        [ObservableProperty]
        private string _phone;

        [ObservableProperty]
        private string _shippingcompany;

        [ObservableProperty]
        private string _receiver;

        [ObservableProperty]
        private DateTime _receiveddate = DateTime.Now;

        // end import add property

        // import detail biding ui
        [ObservableProperty]
        private Material _selectedMaterial;

        [ObservableProperty]
        private Supplier _selectedSupplier;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private int _quantity;

        [ObservableProperty]
        private string _original;

        [ObservableProperty]
        private string _manufacturer;

        [ObservableProperty]
        private DateTime _manufacturedate = DateTime.Now;

        [ObservableProperty]
        private DateTime _expirationdate = DateTime.Now;

        [ObservableProperty]
        private ObservableCollection<object> _currentAddImportDaetailList;

        // end

        #endregion Property binding lấy dữ liệu ui

        public TestImportViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _importServices = provider.GetRequiredService<ImportServices>();
            _importDetailServices = provider.GetRequiredService<ImportDetailServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            ImportList = new ObservableCollection<Import>(
                await _importServices.GetImportList() ?? Enumerable.Empty<Import>());
            return;
        }

        [RelayCommand]
        private async Task InfoImportDetail(Import import)
        {
            IsOpenImportDetailList = true;
            if (import != null)
            {
                ImportDetailListById = new ObservableCollection<MaterialDetailDTO>(
                        await _importServices.GetImportDetailByImportId(import.Importid) ?? Enumerable.Empty<MaterialDetailDTO>()
                    );
            };
        }

        [RelayCommand]
        private void CloseAddImport()
        {
            IsOpenAddImportView = false;
            if (CurrentAddImportDaetailList != null)
            {
                CurrentAddImportDaetailList.Clear();
            }

            Deliveryperson = string.Empty;
            Phone = string.Empty;
            Shippingcompany = string.Empty;
            Receiver = string.Empty;
            Receiveddate = DateTime.Now;

            SelectedMaterial = new Material();
            SelectedSupplier = new Supplier();
            Price = 0;
            Quantity = 0;
            Original = string.Empty;
            Manufacturer = string.Empty;
            Manufacturedate = DateTime.Now;
            Expirationdate = DateTime.Now;
        }

        [RelayCommand]
        private async Task OpenAddImport()
        {
            MaterialChoice = new(await _materialSupplierServices.GetMaterialList());
            SupplierChoice = new(await _materialSupplierServices.GetSupplierList());

            IsOpenAddImportView = true;
        }

        [RelayCommand]
        private async Task SubmitAddImport()
        {
            Import addImport = new Import()
            {
                Deliveryperson = Deliveryperson,
                Phone = Phone,
                Shippingcompany = Shippingcompany,
                Receiver = Receiver,
                Receiveddate = Receiveddate
            };

            await _importDetailServices.AddImportDetailArange(CurrentImportDetailList.ToList(), addImport, SelectedMaterial, SelectedSupplier);
            ImportList.Add(addImport);
        }

        [RelayCommand]
        private void AddImportDetail()
        {
            CurrentImportDetailList.Add(new MaterialDetailDTO
            {
                Materialname = SelectedMaterial.Materialname,
                Suppliername = SelectedSupplier.Suppliername,
                Unit = SelectedMaterial.Unit,
                Quantity = Quantity,
                Price = Price,
                Original = Original,
                Manufacturer = Manufacturer,
                Manufacturedate = Manufacturedate,
                Expirationdate = Expirationdate,
            });
        }
    }
}