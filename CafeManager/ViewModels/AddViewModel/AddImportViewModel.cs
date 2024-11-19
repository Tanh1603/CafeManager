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
        private ImportDTO _modifyImport = new()
        {
            Receiveddate = DateTime.Now
        };

        [ObservableProperty]
        private ImportDetailDTO _currentImportDetail = new()
        {
            ModifyMaterialDetail = new() { Manufacturedate = DateTime.Now, Expirationdate = DateTime.Now }
        };

        private int _currentImportDetailIndex = -1;

        [ObservableProperty]
        private ObservableCollection<Staff> _listStaff = [];

        [ObservableProperty]
        private ObservableCollection<Supplier> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<Material> _listMaterial = [];

        public event Action<ImportDTO>? ImportChanged;

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

        public void RecieveImport(ImportDTO import)
        {
            ModifyImport = import;
        }

        public void ClearValueOfViewModel()
        {
            IsAdding = false;
            IsUpdating = false;
            ModifyImport = new()
            {
                Receiveddate = DateTime.Now,
            };
            CurrentImportDetail = new() { ModifyMaterialDetail = new() { Manufacturedate = DateTime.Now, Expirationdate = DateTime.Now } };
        }

        [RelayCommand]
        private void ClearAddImportDetail()
        {
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
            CurrentImportDetail = new() { ModifyMaterialDetail = new() { Manufacturedate = DateTime.Now, Expirationdate = DateTime.Now } };
        }

        [RelayCommand]
        private void ModifyImportDetailList()
        {
            if (IsAddingImportDetail)
            {
                bool isOldMaterialDetail = false;
                for (int i = 0; i < ModifyImport.ListImportDetailDTO.Count; i++)
                {
                    ImportDetailDTO item = ModifyImport.ListImportDetailDTO[i];
                    if (item.ModifyMaterialDetail.CurrentMaterial == CurrentImportDetail.ModifyMaterialDetail.CurrentMaterial &&
                            item.ModifyMaterialDetail.Price == CurrentImportDetail.ModifyMaterialDetail.Price &&
                            item.ModifyMaterialDetail.Original == CurrentImportDetail.ModifyMaterialDetail.Original &&
                            item.ModifyMaterialDetail.Manufacturer == CurrentImportDetail.ModifyMaterialDetail.Manufacturer &&
                            item.ModifyMaterialDetail.Manufacturedate == CurrentImportDetail.ModifyMaterialDetail.Manufacturedate &&
                            item.ModifyMaterialDetail.Expirationdate == CurrentImportDetail.ModifyMaterialDetail.Expirationdate)
                    {
                        isOldMaterialDetail = true;
                        item.Quantity += CurrentImportDetail.Quantity;
                        MyMessageBox.Show("Chi tiết đã tồn tại, số lượng nhập được cộng thêm", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                        break;
                    }
                }
                if (!isOldMaterialDetail)
                {
                    ModifyImport.ListImportDetailDTO.Add(CurrentImportDetail);
                    _currentImportDetailIndex = ModifyImport.ListImportDetailDTO.IndexOf(CurrentImportDetail);
                    MyMessageBox.Show("Thêm chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
                IsAddingImportDetail = false;
            }
            else if (IsUpdatingImportDetail)
            {
                bool ok = false;

                var find = ModifyImport.ListImportDetailDTO.FirstOrDefault(x => x.Importdetailid == CurrentImportDetail.Importdetailid);
                if (find != null)
                {
                    find.ModifyMaterialDetail = CurrentImportDetail.ModifyMaterialDetail;
                    find.Quantity = CurrentImportDetail.Quantity;
                    MyMessageBox.Show("Sửa chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
                IsUpdatingImportDetail = false;
            }
            CurrentImportDetail = new() { ModifyMaterialDetail = new() { Manufacturedate = DateTime.Now, Expirationdate = DateTime.Now } };
        }

        [RelayCommand]
        private void DeleteImportDetail(ImportDetailDTO importDetail)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No_Cancel, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                ModifyImport.ListImportDetailDTO.Remove(importDetail);
                MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
        }

        [RelayCommand]
        private void UpdateImportDetail(ImportDetailDTO value)
        {
            IsUpdatingImportDetail = true;
            IsAddingImportDetail = false;
            CurrentImportDetail = new ImportDetailDTO
            {
                Importdetailid = value.Importdetailid,
                ImportId = value.ImportId,
                MaterialsupplierId = value.MaterialsupplierId,
                Quantity = value.Quantity,
                ModifyMaterialDetail = value.ModifyMaterialDetail,
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
            ImportChanged?.Invoke(ModifyImport.Clone());
        }

        [RelayCommand]
        private void CloseAddImport()
        {
            Close?.Invoke();
        }
        //[RelayCommand]
        //private void CloseUserControl()
        //{
        //    Close?.Invoke();
        //}

        //public void Dispose()
        //{
        //    if (AddSupplierVM != null)
        //    {
        //        AddSupplierVM.AddSupplierChanged -= AddSupplierVM_AddSupplierChanged;
        //        AddSupplierVM.Close -= AddMaterialSupplierVM_Close;

        //        AddMaterialVM.AddMaterialChanged -= AddMaterialVM_AddMaterialChanged;
        //        AddMaterialVM.Close -= AddMaterialSupplierVM_Close;
        //    }
        //    GC.SuppressFinalize(this);
        //}


    }
}