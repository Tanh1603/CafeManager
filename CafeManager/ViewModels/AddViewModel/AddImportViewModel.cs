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
using System.Runtime.CompilerServices;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddImportViewModel : ObservableValidator
    {
        private readonly IServiceProvider _provider;
        private readonly ImportDetailServices _importdetailServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly IMapper _mapper;

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
        private decimal _importPrice = 0;

        [ObservableProperty]
        private MaterialDTO? _selectedMaterialDTO;

        [ObservableProperty]
        private SupplierDTO? _selectedSupplierDTO;

        [ObservableProperty]
        private StaffDTO? _selectedStaffDTO;

        [ObservableProperty]
        private ImportDetailDTO _currentImportDetail = new()
        {
            Materialsupplier = new()
            {
                Material = new(),
                Manufacturedate = DateTime.Now.Date,
                Expirationdate = DateTime.Now.Date
            }
        };

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaff = [];

        [ObservableProperty]
        private ObservableCollection<SupplierDTO> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listMaterial = [];

        [ObservableProperty]
        private ObservableCollection<ImportDetailDTO> _listExisted = [];

        private List<ImportDetailDTO> listDeletedImportdetail = new();

        public event Action<ImportDTO> ImportChanged;

        [ObservableProperty]
        private object _selectedViewModel;

        [ObservableProperty]
        private AddMaterialViewModel _addMaterialVM;

        [ObservableProperty]
        private AddSuppierViewModel _addSupplierVM;

        public AddImportViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _importdetailServices = provider.GetRequiredService<ImportDetailServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _mapper = _provider.GetRequiredService<IMapper>();

            AddMaterialVM = _provider.GetRequiredService<AddMaterialViewModel>();
            AddMaterialVM.ModifyMaterialChanged += AddMaterialVM_ModifyMaterialChanged;
            AddMaterialVM.Close += AddMaterialSupplierVM_Close;

            AddSupplierVM = _provider.GetRequiredService<AddSuppierViewModel>();
            AddSupplierVM.ModifySupplierChanged += AddSupplierVM_ModifySupplierChanged;
            AddSupplierVM.Close += AddMaterialSupplierVM_Close;
        }

        public void RecieveImport(ImportDTO import)
        {
            ModifyImport = import;
            ImportPrice = import.TotalPrice;
            ListExisted = [.. import.Importdetails];
            SelectedSupplierDTO = ListSupplier.FirstOrDefault(x => x.Supplierid == import.Supplierid);
            SelectedStaffDTO = ListStaff.FirstOrDefault(x => x.Staffid == import.Staffid);
        }

        public void ClearValueOfViewModel()
        {
            IsAdding = false;
            IsUpdating = false;
            ModifyImport = new()
            {
                Receiveddate = DateTime.Now
            };
            ImportPrice = 0;
            ClearAddImportDetail();
            ListExisted.Clear();
            listDeletedImportdetail.Clear();
            CurrentImportDetail = new();
            SelectedMaterialDTO = null;
            SelectedStaffDTO = null;
            SelectedSupplierDTO = null;
        }

        [RelayCommand]
        private void ClearAddImportDetail()
        {
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
            SelectedMaterialDTO = null;
            CurrentImportDetail = new();
        }

        private bool CommpareDateTinme(DateTime first, DateTime second)
        {
            return first.Date == second.Date;
        }

        [RelayCommand]
        private void ModifyImportDetail()
        {
            CurrentImportDetail.Materialsupplier.ValidateDTO();
            if (CurrentImportDetail.Materialsupplier.HasErrors)
            {
                return;
            }
            if (SelectedMaterialDTO == null || SelectedMaterialDTO?.Materialid == 0)
            {
                MyMessageBox.ShowDialog("Vui lòng chọn loại vật tư", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                return;
            }
            if (IsAddingImportDetail && SelectedMaterialDTO != null)
            {
                var existedMateirlasupplier = ListExisted
                    .FirstOrDefault(x => x.Isdeleted == false &&
                    x.Materialsupplier.Material.Materialid == SelectedMaterialDTO.Materialid &&
                    x.Materialsupplier.Price == CurrentImportDetail.Materialsupplier.Price &&
                    x.Materialsupplier.Original == CurrentImportDetail.Materialsupplier.Original &&
                    x.Materialsupplier.Manufacturer == CurrentImportDetail.Materialsupplier.Manufacturer &&
                    CommpareDateTinme(x.Materialsupplier.Manufacturedate, CurrentImportDetail.Materialsupplier.Manufacturedate) &&
                    CommpareDateTinme(x.Materialsupplier.Expirationdate, CurrentImportDetail.Materialsupplier.Expirationdate));
                ImportPrice += CurrentImportDetail.Quantity * CurrentImportDetail.Materialsupplier.Price;
                if (existedMateirlasupplier != null)
                {
                    MyMessageBox.ShowDialog("Chi tiết đã tồn tại, đã thêm số lượng", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    existedMateirlasupplier.Quantity += CurrentImportDetail.Quantity;
                }
                else
                {
                    CurrentImportDetail.Materialsupplier.Materialid = SelectedMaterialDTO.Materialid;
                    CurrentImportDetail.Materialsupplier.Material = SelectedMaterialDTO;
                    MyMessageBox.ShowDialog("Thêm chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    ListExisted.Add(CurrentImportDetail.Clone());
                }
            }
            else if (IsUpdatingImportDetail)
            {
                var find = ListExisted.FirstOrDefault(x => x.Isdeleted == false && x.Id == CurrentImportDetail.Id);

                if (find != null && SelectedMaterialDTO != null)
                {
                    ImportPrice += (CurrentImportDetail.Quantity * CurrentImportDetail.Materialsupplier.Price
                        - find.Quantity * find.Materialsupplier.Price);

                    find.Materialsupplier.Materialid = SelectedMaterialDTO.Materialid;
                    find.Materialsupplier.Material = SelectedMaterialDTO;
                    find.Materialsupplier.Price = CurrentImportDetail.Materialsupplier.Price;
                    find.Materialsupplier.Original = CurrentImportDetail.Materialsupplier.Original;
                    find.Materialsupplier.Manufacturer = CurrentImportDetail.Materialsupplier.Manufacturer;
                    find.Materialsupplier.Manufacturedate = CurrentImportDetail.Materialsupplier.Manufacturedate;
                    find.Materialsupplier.Expirationdate = CurrentImportDetail.Materialsupplier.Expirationdate;
                    find.Quantity = CurrentImportDetail.Quantity;
                    MyMessageBox.ShowDialog("Sửa chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
                else
                {
                    MyMessageBox.ShowDialog("Sửa chi tiết đơn hàng thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            ClearAddImportDetail();
        }

        [RelayCommand]
        private void DeleteImportDetail(ImportDetailDTO importDetail)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                var deleted = ListExisted.FirstOrDefault(x => x.Isdeleted == false && x.Id == importDetail.Id);
                if (deleted != null)
                {
                    deleted.Isdeleted = true;
                    ImportPrice -= deleted.Quantity * deleted.Materialsupplier.Price;
                    if (IsUpdating)
                    {
                        listDeletedImportdetail.Add(deleted);
                    }
                    ListExisted.Remove(deleted);
                    MyMessageBox.ShowDialog("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    OnPropertyChanged(nameof(ListExisted));
                }
            }
        }

        [RelayCommand]
        private void UpdateImportDetail(ImportDetailDTO importDetail)
        {
            IsUpdatingImportDetail = true;
            IsAddingImportDetail = false;
            CurrentImportDetail = importDetail.Clone();
            CurrentImportDetail.Materialsupplier = CurrentImportDetail.Materialsupplier.Clone();
            SelectedMaterialDTO = ListMaterial.FirstOrDefault(x => x.Materialid == importDetail.Materialsupplier.Material.Materialid);
        }

        #region Add Material/Supplier

        private void AddMaterialSupplierVM_Close()
        {
            IsOpenAddMaterialSupplier = false;
        }

        //Supplier
        private async void AddSupplierVM_ModifySupplierChanged(SupplierDTO obj)
        {
            var addSupplier = await _materialSupplierServices.AddSupplier(_mapper.Map<Supplier>(obj));
            if (addSupplier != null)
            {
                ListSupplier.Add(_mapper.Map<SupplierDTO>(addSupplier));
                MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                AddSupplierVM.ClearValueOfFrom();
                IsOpenAddMaterialSupplier = false;
            }
            else
            {
                MyMessageBox.ShowDialog("Thêm nhà cung cấp thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
            }
        }

        //Material
        private async void AddMaterialVM_ModifyMaterialChanged(MaterialDTO obj)
        {
            var addMaterial = await _materialSupplierServices.AddMaterial(_mapper.Map<Material>(obj));
            if (addMaterial != null)
            {
                ListMaterial.Add(_mapper.Map<MaterialDTO>(addMaterial));
                MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                AddSupplierVM.ClearValueOfFrom();
                IsOpenAddMaterialSupplier = false;
            }
            else
            {
                MyMessageBox.ShowDialog("Thêm nhà cung cấp thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
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

            AddMaterialVM.ModifyMaterial = new();
        }

        [RelayCommand]
        private void OpenAddSupplier()
        {
            SelectedViewModel = AddSupplierVM;
            IsOpenAddMaterialSupplier = true;

            AddSupplierVM.IsAdding = true;
            AddSupplierVM.IsUpdating = false;
        }

        #endregion Open View

        #endregion Add Material/Supplier

        [RelayCommand]
        private void SubmitModifyImport()
        {
            ModifyImport.ValidateDTO();
            if (ModifyImport.HasErrors)
            {
                return;
            }
            else if (SelectedSupplierDTO == null || SelectedSupplierDTO.Supplierid == 0)
            {
                MyMessageBox.ShowDialog("Vui lòng chọn nhà cung cấp", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                return;
            }
            else if (SelectedStaffDTO == null || SelectedStaffDTO.Staffid == 0)
            {
                MyMessageBox.ShowDialog("Vui lòng chọn nhân viên nhập hàng", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                return;
            }
            ModifyImport.Importdetails = [.. ListExisted.Select(x => x.Clone()).Concat(listDeletedImportdetail)];

            foreach (var item in ModifyImport.Importdetails)
            {
                var recieveDate = DateOnly.FromDateTime(ModifyImport.Receiveddate);
                var manufactureDate = DateOnly.FromDateTime(item.Materialsupplier.Manufacturedate);
                var expirationDate = DateOnly.FromDateTime(item.Materialsupplier.Expirationdate);
                if (recieveDate < manufactureDate || recieveDate > expirationDate)
                {
                    MyMessageBox.ShowDialog("Ngày giao hàng không hợp lệ (Kiểm tra chi tiết nhập kho)", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    return;
                }
            }
            ModifyImport.Supplierid = SelectedSupplierDTO.Supplierid;
            ModifyImport.Staffid = SelectedStaffDTO.Staffid;

            if (ModifyImport.Importdetails != null)
            {
                foreach (var importDetail in ModifyImport.Importdetails)
                {
                    importDetail.Materialsupplier.Supplierid = SelectedSupplierDTO.Supplierid;
                }
            }

            if (IsAdding)
            {
                ModifyImport.Importdetails = [.. ModifyImport.Importdetails?.Where(x => x.Isdeleted == false)];
                ImportChanged?.Invoke(ModifyImport.Clone());
            }
            if (IsUpdating)
            {
                ModifyImport.Importdetails?.ToList().ForEach(x => x.Importid = ModifyImport.Importid);
                ImportChanged?.Invoke(ModifyImport.Clone());
            }
            listDeletedImportdetail.Clear();
        }
    }
}