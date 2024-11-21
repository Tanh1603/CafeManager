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
using System.Diagnostics;
using System.Windows.Forms;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;

        [ObservableProperty]
        private bool _isOpenAddMaterialSupplier;

        [ObservableProperty]
        public bool _isAdding = false;

        [ObservableProperty]
        public bool _isUpdating = false;

        [ObservableProperty]
        public bool _isAddingImportDetail = false;

        [ObservableProperty]
        public bool _isUpdatingImportDetail = false;

        [ObservableProperty]
        private Import _modifyImport = new()
        {
            Receiveddate = DateTime.Now
        };

        [ObservableProperty]
        private ImportMaterialDetailDTO _currentImportDetail = new()
        {
            Manufacturedate = DateTime.Now,
            Expirationdate = DateTime.Now,
        };

        [ObservableProperty]
        private decimal _importPrice;

        [ObservableProperty]
        private ObservableCollection<ImportMaterialDetailDTO> _currentListImportdetail = [];

        [ObservableProperty]
        private ObservableCollection<Staff> _listStaff = [];

        [ObservableProperty]
        private ObservableCollection<Supplier> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<Material> _listMaterial = [];

        public event Action<Import, List<ImportMaterialDetailDTO>?> ImportChanged;

        public event Action Close;

        [ObservableProperty]
        private object _selectedViewModel;

        [ObservableProperty]
        private Material _selectedMaterial = new() { Isdeleted = false };

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

        public void RecieveImport(Import import)
        {
            ModifyImport = import;
        }

        public void ClearValueOfViewModel()
        {
            IsAdding = false;
            IsUpdating = false;
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
            ModifyImport = new()
            {
                Receiveddate = DateTime.Now,
            };
            CurrentImportDetail = new()
            {
                Manufacturedate = DateTime.Now,
                Expirationdate = DateTime.Now,
            };
            SelectedMaterial = new();
            CurrentListImportdetail.Clear();
        }

        [RelayCommand]
        private void ClearAddImportDetail()
        {
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
            CurrentImportDetail = new()
            {
                Manufacturedate = DateTime.Now,
                Expirationdate = DateTime.Now,
            };
            SelectedMaterial = new();
        }

        [RelayCommand]
        private void ModifyImportDetailList()
        {
            CurrentImportDetail.Materialid = SelectedMaterial.Materialid;
            CurrentImportDetail.Materialname = SelectedMaterial.Materialname;
            CurrentImportDetail.Unit = SelectedMaterial.Unit;

            if (IsAddingImportDetail)
            {
                bool isOldMaterialDetail = false;
                for (int i = 0; i < CurrentListImportdetail.Count; i++)
                {
                    ImportMaterialDetailDTO item = CurrentListImportdetail[i];
                    if (item.Materialname == CurrentImportDetail.Materialname &&
                            item.Price == CurrentImportDetail.Price &&
                            item.Original == CurrentImportDetail.Original &&
                            item.Manufacturer == CurrentImportDetail.Manufacturer &&
                            item.Manufacturedate == CurrentImportDetail.Manufacturedate &&
                            item.Expirationdate == CurrentImportDetail.Expirationdate)
                    {
                        isOldMaterialDetail = true;
                        item.Quantity += CurrentImportDetail.Quantity;
                        MyMessageBox.Show("Chi tiết đã tồn tại, số lượng nhập được cộng thêm", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                        break;
                    }
                }
                if (!isOldMaterialDetail)
                {
                    CurrentListImportdetail.Add(CurrentImportDetail);
                    MyMessageBox.Show("Thêm chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            else if (IsUpdatingImportDetail)
            {
                bool ok = false;

                var find = CurrentListImportdetail.FirstOrDefault(x => x.Importdetailid == CurrentImportDetail.Importdetailid);
                if (find != null)
                {
                    find.Materialname = CurrentImportDetail.Materialname;
                    find.Price = CurrentImportDetail.Price;
                    find.Original = CurrentImportDetail.Original;
                    find.Manufacturer = CurrentImportDetail.Manufacturer;
                    find.Manufacturedate = CurrentImportDetail.Manufacturedate;
                    find.Expirationdate = CurrentImportDetail.Expirationdate;
                    find.Quantity = CurrentImportDetail.Quantity;
                    MyMessageBox.Show("Sửa chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            CurrentImportDetail = new() { Manufacturedate = DateTime.Now, Expirationdate = DateTime.Now };
            SelectedMaterial = new();
            CurrentListImportdetail = [.. CurrentListImportdetail];
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
        }

        [RelayCommand]
        private void DeleteImportDetail(ImportMaterialDetailDTO importDetail)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No_Cancel, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                CurrentListImportdetail.Remove(importDetail);
                MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
        }

        [RelayCommand]
        private void UpdateImportDetail(ImportMaterialDetailDTO value)
        {
            IsUpdatingImportDetail = true;
            IsAddingImportDetail = false;
            CurrentImportDetail = new ImportMaterialDetailDTO
            {
                Importdetailid = value.Importdetailid,
                Materialid = value.Materialid,
                Materialsupplierid = value.Materialsupplierid,
                Materialname = value.Materialname,
                Price = value.Price,
                Original = value.Original,
                Manufacturer = value.Manufacturer,
                Manufacturedate = value.Manufacturedate,
                Expirationdate = value.Expirationdate,
                Quantity = value.Quantity
            };
            SelectedMaterial = new()
            {
                Materialid = value.Materialid,
                Materialname = value.Materialname,
                Unit = value.Unit,
                Isdeleted = value.Isdeleted
            };
            
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
        private void SubmitModifyImport()
        {
            ImportChanged?.Invoke(ModifyImport, CurrentListImportdetail.ToList());
        }

        [RelayCommand]
        private void CloseAddImport()
        {
            Close?.Invoke();
            ClearValueOfViewModel();
        }
    }
}