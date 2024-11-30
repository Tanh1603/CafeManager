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
using AutoMapper;
using System.Windows.Media.Media3D;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class ImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ImportServices _importServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly StaffServices _staffServices;
        //private IMapper _mapper;

        [ObservableProperty]
        private bool _isOpenModifyImportView;

        [ObservableProperty]
        private AddImportViewModel _modifyImportVM;

        [ObservableProperty]
        private decimal _totalPrice = 0;

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
            var list = (await _materialSupplierServices.GetListSupplier()).ToList().Where(x => x.Isdeleted == false);
            ModifyImportVM.ListSupplier = [.. ModifyImportVM._mapper.Map<List<SupplierDTO>>(list)];

            var dbListMaterial = (await _materialSupplierServices.GetListMaterial()).ToList().Where(x => x.Isdeleted == false);
            ModifyImportVM.ListMaterial = [.. ModifyImportVM._mapper.Map<List<MaterialDTO>>(dbListMaterial)];

            var dbListStaff = await _staffServices.GetListStaff();
            ModifyImportVM.ListStaff = new ObservableCollection<Staff>(dbListStaff);

            var importList = await _importServices.GetListImport();
            ListImport = new ObservableCollection<Import>(importList);

            TotalPrice = await _importServices.GetTotalImportPrice();
        }

        private async Task LoadDetailData(Import import)
        {
            var dbImportMaterailDetails = await _importServices.GetListImportDetailByImportId(import.Importid);
            ModifyImportVM.CurrentListImportdetail = new ObservableCollection<ImportMaterialDetailDTO>(dbImportMaterailDetails);
            ModifyImportVM.ImportPrice = await _importServices.GetImportPriceById(import.Importid);
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
                        //Import? updateImport = await _importServices.GetImportById(import.Importid);
                        //if (updateImport != null)
                        //{
                        //    updateImport.Deliveryperson = import.Deliveryperson;
                        //    updateImport.Phone = import.Phone;
                        //    updateImport.Shippingcompany = import.Shippingcompany;
                        //    updateImport.Receiveddate = import.Receiveddate;
                        //    updateImport.Staffid = import.Staffid;
                        //    updateImport.Supplierid = import.Supplierid;
                        //    await _importServices.UpdateImport(import, importMaterials);
                        //}
                        await _importServices.UpdateImport(import, importMaterials);
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);

                    }
                    else
                    {
                        MyMessageBox.Show("Sửa thông tin phiếu nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                    IsOpenModifyImportView = false;
                }
                ModifyImportVM.ClearValueOfViewModel();
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
            //ModifyImportVM.IsAdding = false;
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