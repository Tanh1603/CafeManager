using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeManager.WPF.MessageBox;
using System.Windows.Forms.VisualStyles;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;

        [ObservableProperty]
        private bool _isOpenAddMaterialSupplier;

        [ObservableProperty]
        private int _importid;

        [ObservableProperty]
        private string _deliveryperson;

        [ObservableProperty]
        private string _phone;

        [ObservableProperty]
        private string _shippingcompany;

        [ObservableProperty]
        private DateTime _receiveddate;

        [ObservableProperty]
        private string _receiver;

        [ObservableProperty]
        private int _materialsupplierid;

        //ImportDetail
        [ObservableProperty]
        private Supplier _selectedSupplier;

        [ObservableProperty]
        private Material _selectedMaterial;

        [ObservableProperty]
        private DateTime _manufactureDate;

        [ObservableProperty]
        private DateTime _expirationDate;

        [ObservableProperty]
        private string _original;

        [ObservableProperty]
        private string _manufacturer;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private int _quantity;

        [ObservableProperty]
        private ObservableCollection<MaterialDetailDTO> _listMaterialDetailDTO = [];

        [ObservableProperty]
        private List<MaterialDetailDTO> _listAddMaterialDetailDTOs = [];

        [ObservableProperty]
        private ObservableCollection<Supplier> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<Material> _listMaterial = [];

        [ObservableProperty]
        public bool _isAdding = false;

        public event Action<Import> UpdateImportNameChanged;

        public event Action<List<MaterialDetailDTO>, Import, Material, Supplier> AddImportChanged;

        public event Action Close;

        [ObservableProperty]
        private object _selectedViewModel;

        [ObservableProperty]
        private AddMaterialViewModel _addMaterialVM;

        [ObservableProperty]
        private AddSuppierViewModel _addSupplierVM;

        public AddImportViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();

            AddMaterialVM = _provider.GetRequiredService<AddMaterialViewModel>();
            AddMaterialVM.AddMaterialChanged += AddMaterialVM_AddMaterialChanged;
            AddMaterialVM.Close += AddMaterialSupplierVM_Close;

            AddSupplierVM = _provider.GetRequiredService<AddSuppierViewModel>();
            AddSupplierVM.AddSupplierChanged += AddSupplierVM_AddSupplierChanged;
            AddSupplierVM.Close += AddMaterialSupplierVM_Close;
        }

        public void HandleImportFromParent(Import import)
        {
            Importid = import.Importid;
            Deliveryperson = import.Deliveryperson;
            Phone = import.Phone;
            Shippingcompany = import.Shippingcompany;
            Receiveddate = import.Receiveddate;
            //Receiver = import.Receiver;
        }

        private void AddMaterialSupplierVM_Close()
        {
            IsOpenAddMaterialSupplier = false;
        }

        //Supplier
        private async void AddSupplierVM_AddSupplierChanged(Supplier obj)
        {
            try
            {
                IsOpenAddMaterialSupplier = false;
                var addedSupplier = await _materialSupplierServices.AddSupplier(obj);
                ListSupplier.Add(addedSupplier);

                MyMessageBox.Show("Thêm nhà cung cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            catch (InvalidOperationException ex)
            {
                MyMessageBox.Show(ex.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        //Material
        private async void AddMaterialVM_AddMaterialChanged(Material obj)
        {
            try
            {
                IsOpenAddMaterialSupplier = false;
                var addedMaterial = await _materialSupplierServices.AddMaterial(obj);
                ListMaterial.Add(addedMaterial);

                MyMessageBox.Show("Thêm vật liệu cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            catch (InvalidOperationException ex)
            {
                MyMessageBox.Show(ex.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        //[RelayCommand]
        //private void SubmitImport()
        //{
        //    Import newImport = new()
        //    {
        //        Deliveryperson = this.Deliveryperson,
        //        Phone = this.Phone,
        //        Shippingcompany = this.Shippingcompany,
        //        Receiveddate = this.Receiveddate,
        //        Receiver = this.Receiver
        //    };

        //    AddImportChanged?.Invoke(newImport);

        //}

        [RelayCommand]
        private async void SubmitMaterialSupplier()
        {
            MaterialDetailDTO newMaterialDetailDTO = new()
            {
                Materialname = SelectedMaterial.Materialname,
                Suppliername = SelectedSupplier.Suppliername,
                Unit = SelectedMaterial.Unit,
                Quantity = this.Quantity,
                Price = this.Price,
                Original = this.Original,
                Manufacturer = this.Manufacturer,
                Manufacturedate = this.ManufactureDate,
                Expirationdate = this.ExpirationDate
            };

            ListAddMaterialDetailDTOs.Add(newMaterialDetailDTO);
            ListMaterialDetailDTO.Add(newMaterialDetailDTO);
        }

        [RelayCommand]
        private async void SubmitAllImportDetail()
        {
            Import newImport = new()
            {
                Deliveryperson = this.Deliveryperson,
                Phone = this.Phone,
                Shippingcompany = this.Shippingcompany,
                Receiveddate = this.Receiveddate,
                //Receiver = this.Receiver
            };

            AddImportChanged?.Invoke(ListAddMaterialDetailDTOs, newImport, SelectedMaterial, SelectedSupplier);
        }

        #region Open View

        [RelayCommand]
        private void OpenAddMaterial()
        {
            SelectedViewModel = AddMaterialVM;
            IsOpenAddMaterialSupplier = true;

            AddMaterialVM.IsAdding = true;
            AddMaterialVM.IsUpdating = false;

            AddMaterialVM.Materialname = string.Empty;
            AddMaterialVM.Unit = string.Empty;
        }

        [RelayCommand]
        private void OpenAddSupplier()
        {
            SelectedViewModel = AddSupplierVM;
            IsOpenAddMaterialSupplier = true;

            AddSupplierVM.IsAdding = true;
            AddSupplierVM.IsUpdating = false;

            AddSupplierVM.Suppliername = string.Empty;
            AddSupplierVM.Representativesupplier = string.Empty;
            AddSupplierVM.Address = string.Empty;
            AddSupplierVM.Email = string.Empty;
            AddSupplierVM.Phone = string.Empty;
            AddSupplierVM.Notes = string.Empty;
        }

        #endregion Open View

        [RelayCommand]
        private void DeleteImportDetail(MaterialDetailDTO materialDetailDTO)
        {
            try
            {
                ListAddMaterialDetailDTOs.Remove(materialDetailDTO);
                ListMaterialDetailDTO.Remove(materialDetailDTO);
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }

        public void Dispose()
        {
            if (AddSupplierVM != null)
            {
                AddSupplierVM.AddSupplierChanged -= AddSupplierVM_AddSupplierChanged;
                AddSupplierVM.Close -= AddMaterialSupplierVM_Close;

                AddMaterialVM.AddMaterialChanged -= AddMaterialVM_AddMaterialChanged;
                AddMaterialVM.Close -= AddMaterialSupplierVM_Close;
            }
            GC.SuppressFinalize(this);
        }
    }
}