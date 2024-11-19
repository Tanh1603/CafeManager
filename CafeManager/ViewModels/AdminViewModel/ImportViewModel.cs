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
        private ObservableCollection<ImportDTO> _listImport = [];

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
            ModifyImportVM.ImportChanged += AddImportVM_ImportChanged;
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
            ListImport = [.. importList.ToList().Select(x => ImportMapper.ToDTO(x))];
            foreach(var import in ListImport)
            {
                foreach (var item in import.ListImportDetailDTO)
                {
                    var materialDetailDTO = await _materialSupplierServices.GetMaterialsuppliers(item.MaterialId, import.ImportSupplier.Supplierid, item.Importdetailid);
                    item.ModifyMaterialDetail = materialDetailDTO;
                }
            }    

        }
        private async void AddImportVM_ImportChanged(ImportDTO import)
        {
            try
            {
                if (ModifyImportVM.IsAdding)
                {
                    Materialsupplier materialsupplier;
                    foreach(var item in import.ListImportDetailDTO)
                    {
                        materialsupplier = MaterialSupplierMapper.ToEntity(item.ModifyMaterialDetail, import.ImportSupplier.Supplierid);
                        Materialsupplier newMaterialsupplier = await _materialSupplierServices.AddMaterialsupplier(materialsupplier);
                    }
                    Import? addImport = await _importServices.AddImport(ImportMapper.ToEntity(import));
                    
                    if (addImport != null)
                    {
                        var addImportDTO = ImportMapper.ToDTO(addImport);
                        foreach (var item in addImportDTO.ListImportDetailDTO)
                        {
                            var materialDetailDTO = await _materialSupplierServices.GetMaterialsuppliers(item.ModifyMaterialDetail.CurrentMaterial.Materialid, addImport.Supplierid, item.Importdetailid);
                            item.ModifyMaterialDetail = materialDetailDTO;
                        }

                        ListImport.Add(addImportDTO);
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
                    var updateImportDTO = ListImport.FirstOrDefault(x => x.Importid == import.Importid);
                    if (updateImportDTO != null)
                    {
                        updateImportDTO.ImportSupplier = import.ImportSupplier;
                        updateImportDTO.Deliveryperson = import.Deliveryperson;
                        updateImportDTO.Phone = import.Phone;
                        updateImportDTO.Shippingcompany = import.Shippingcompany;
                        updateImportDTO.Receiveddate = import.Receiveddate;
                        updateImportDTO.ReceivedStaff = import.ReceivedStaff;
                        updateImportDTO.ListImportDetailDTO = import.ListImportDetailDTO;

                        Import? updateImport = await _importServices.GetImportById(updateImportDTO.Importid);
                        List<Importdetail>? updateListImportDetail = [];
                        if (updateImport != null)
                        {
                            updateImport.Supplierid = import.ImportSupplier.Supplierid;
                            updateImport.Deliveryperson = import.Deliveryperson;
                            updateImport.Phone = import.Phone;
                            updateImport.Shippingcompany = import.Shippingcompany;
                            updateImport.Receiveddate = import.Receiveddate;
                            updateImport.Staffid = import.ReceivedStaff.Staffid;
                            updateListImportDetail = import.ListImportDetailDTO.Select(x => ImportDetailMapper.ToEntity(x)).ToList();

                            await _importServices.UpdateImport(updateImport, updateListImportDetail);
                        }
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    }
                    else
                    {
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                    IsOpenModifyImportView = false;
                    ListImport = [.. ListImport];
                    ModifyImportVM.ClearValueOfViewModel();
                }
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
        private void OpenUpdateImport(ImportDTO importDTO)
        {
            ModifyImportVM.RecieveImport(importDTO.Clone());
            IsOpenModifyImportView = true;
            ModifyImportVM.IsUpdating = true;
        }

        [RelayCommand]
        private async void DeleteImport(ImportDTO import)
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
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }

        }

        public void Dispose()
        {
            ModifyImportVM.ImportChanged -= AddImportVM_ImportChanged;
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