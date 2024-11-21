﻿using CafeManager.Core.Data;
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
using CafeManager.WPF.MessageBox;
using CafeManager.Core.Services;
using System.Windows.Documents;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class ImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ImportServices _importServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly StaffServices _staffServices;

        [ObservableProperty]
        private bool _isOpenModifyImportView;

        [ObservableProperty]
        private AddImportViewModel _modifyImportVM;

        [ObservableProperty]
        private decimal _totalPrice;

        [ObservableProperty]
        private ObservableCollection<Import> _listImport = [];

        //[ObservableProperty]
        //private ObservableCollection<MaterialDetailDTO> _listMaterialDetailDTO = [];

        //[ObservableProperty]
        //private ObservableCollection<Supplier> _listSupplier = [];

        //[ObservableProperty]
        //private ObservableCollection<Material> _listMaterial = [];

        public ImportViewModel(IServiceProvider provider)
        {
            _serviceProvider = provider;

            _importServices = provider.GetRequiredService<ImportServices>();
            _staffServices = provider.GetRequiredService<StaffServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();

            ModifyImportVM = _serviceProvider.GetRequiredService<AddImportViewModel>();
            ModifyImportVM.ImportChanged += ModifyImportVM_ImportChanged;
            _isOpenModifyImportView = false;
            Task.Run(LoadData);
        }
        private async Task LoadData()
        {
            var dbListSupplier = await _materialSupplierServices.GetListSupplier();
            ModifyImportVM.ListSupplier = new ObservableCollection<Supplier>(dbListSupplier);

            var dbListMaterial = await _materialSupplierServices.GetListMaterial();
            ModifyImportVM.ListMaterial = new ObservableCollection<Material>(dbListMaterial);

            var dbListStaff = await _staffServices.GetListStaff();
            ModifyImportVM.ListStaff = new ObservableCollection<Staff>(dbListStaff);

            var importList = await _importServices.GetListImport();
            ListImport = new ObservableCollection<Import>(importList);
        }

        private async Task LoadDetailData(Import import)
        {
            var dbImportMaterailDetails = await _importServices.GetListImportDetailByImportId(import.Importid);
            ModifyImportVM.CurrentListImportdetail = new ObservableCollection<ImportMaterialDetailDTO>(dbImportMaterailDetails);
        }

        private async void ModifyImportVM_ImportChanged(Import import, List<ImportMaterialDetailDTO> importMaterials)
        {
            try
            {
                if (ModifyImportVM.IsAdding)
                {
                    Import? addImport = await _importServices.AddImport(import, importMaterials);

                    if (addImport != null)
                    {
                        ListImport.Add(addImport);
                        IsOpenModifyImportView = false;
                        MyMessageBox.Show("Thêm thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                }
                if (ModifyImportVM.IsUpdating)
                {
                    if(import != null)
                    {
                        Import? updateImport = await _importServices.GetImportById(import.Importid);
                        if (updateImport != null)
                        {
                            updateImport.Deliveryperson = import.Deliveryperson;
                            updateImport.Phone = import.Phone;
                            updateImport.Shippingcompany = import.Shippingcompany;
                            updateImport.Receiveddate = import.Receiveddate;
                            updateImport.Staffid = import.Staffid;
                            updateImport.Supplierid = import.Supplierid;
                            await _importServices.UpdateImport(import, importMaterials);
                        }
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);

                    }
                    else
                    {
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                    IsOpenModifyImportView = false;
                    ModifyImportVM.ClearValueOfViewModel();
                }
                ListImport = [.. ListImport];
            }
            catch
            {
                MyMessageBox.Show("Lỗi", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }



        [RelayCommand]
        private void OpenModifyImport(ImportDTO importDTO)
        {
            IsOpenModifyImportView = true;
            ModifyImportVM.IsAdding = true;
            ModifyImportVM.IsAddingImportDetail = true;
        }

        [RelayCommand]
        private void CloseAddImport()
        {
            IsOpenModifyImportView = false;
            ModifyImportVM.ClearValueOfViewModel();
        }

        [RelayCommand]
        private void OpenUpdateImport(Import import)
        {
            ModifyImportVM.RecieveImport(import);
            _ = LoadDetailData(import);
            IsOpenModifyImportView = true;
            ModifyImportVM.IsUpdating = true;
            ModifyImportVM.IsAddingImportDetail = true;
        }

        [RelayCommand]
        private async void DeleteImport(Import import)
        {
            try
            {
                var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No_Cancel, MyMessageBox.Icons.Warning);
                if (res.Equals("1"))
                {
                    var isDeleted = await _importServices.DeleteImport(import.Importid);
                    if (isDeleted)
                    {
                        ListImport.Remove(import);
                    }
                    MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }

        }

        public void Dispose()
        {
            ModifyImportVM.ImportChanged -= ModifyImportVM_ImportChanged;
        }
    }
}



//private void AddImportVM_Close()
//{
//    IsOpenAddImportView = false;
//}

//private async void AddImportVM_UpdateImportChanged(List<MaterialDetailDTO> materialdetailDTOs, Import import, Material material, Supplier supplier)
//{
//    try
//    {
//        IsOpenAddImport = false;
//        _importServices.Update(import);
//        await _importDetailServices.UpdateImportDetailArange(materialdetailDTOs, import, material, supplier);
//        MyMessageBox.Show("Thay đổi chi tiết phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
//    }
//    catch (InvalidOperationException ex)
//    {
//        MyMessageBox.Show(ex.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
//    }
//}

//private async void AddImportVM_AddImportChanged(List<MaterialDetailDTO> materialdetailDTOs, Import addimport, Material addmaterial, Supplier addsupplier)
//{
//    try
//    {
//        IsOpenAddImport = false;
//        await _importDetailServices.AddImportDetailArange(materialdetailDTOs, addimport, addmaterial, addsupplier);
//        ListImport.Add(addimport);
//        MyMessageBox.Show("Thêm chi tiết phiếu nhập cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
//    }
//    catch (InvalidOperationException ex)
//    {
//        MyMessageBox.Show(ex.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
//    }
//}

//[RelayCommand]
//public void OpenAddImport()
//{
//    IsOpenAddImport = true;
//    AddImportVM.IsAdding = true;
//    AddImportVM.IsUpdating = false;

//    AddImportVM.Deliveryperson = string.Empty;
//    AddImportVM.Phone = string.Empty;
//    AddImportVM.Shippingcompany = string.Empty;
//    AddImportVM.Receiveddate = DateTime.Now;

//    AddImportVM.ManufactureDate = DateTime.Now;
//    AddImportVM.ExpirationDate = DateTime.Now;
//    AddImportVM.Original = string.Empty;
//    AddImportVM.Manufacturer = string.Empty;
//}


//private bool _isUpdateImportChangedRegistered = false;

//[RelayCommand]
//private void OpenImportDetail (Import selectedImport)
//{
//    if (selectedImport != null)
//    {
//        IsOpenAddImport = true;

//        AddImportVM.IsAdding = false;
//        AddImportVM.IsUpdating = true;

//        AddImportVM.HandleImportFromParent(selectedImport);

//        _ = LoadImportDetails(selectedImport.Importid);

//        AddImportVM.IsAdding = false;
//        AddImportVM.IsUpdating = true;
//    }
//}

//[RelayCommand]
//private async Task DeleteImport(Import import)
//{
//    try
//    {
//        var isDeleted = await _importServices.DeleteImport(import.Importid);
//        if (isDeleted)
//        {
//            ListImport.Remove(import);
//        }
//    }
//    catch (InvalidOperationException ivd)
//    {
//        MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
//    }
//}