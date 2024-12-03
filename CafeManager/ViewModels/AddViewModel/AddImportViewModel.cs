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

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddImportViewModel : ObservableObject
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
        private ImportDetailDTO _currentImportDetail = new(){ Materialsupplier = new() {Material = new() } };

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaff = [];

        [ObservableProperty]
        private ObservableCollection<SupplierDTO> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listMaterial = [];

        public ObservableCollection<ImportDetailDTO> ListExisted
            => [.. ModifyImport.Importdetails.Where(x => x.Isdeleted == false)];

        public event Action<ImportDTO> ImportChanged;

        [ObservableProperty]
        private object _selectedViewModel;

        //private ImportDetailDTO updateImportdetail;

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
            OnPropertyChanged(nameof(ListExisted));
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
            CurrentImportDetail = new() { Materialsupplier = new() { Material = new() } };
        }

        [RelayCommand]
        private void ClearAddImportDetail()
        {
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
            CurrentImportDetail = new() { Materialsupplier = new() { Material = new() } };
        }

        [RelayCommand]
        private void ModifyImportDetail()
        {
            //CurrentImportDetail.Materialsupplier.Materialid = CurrentImportDetail.Materialsupplier.Material.Materialid;

            if (IsAddingImportDetail)
            {
                var existedMateirlasupplier = ModifyImport.Importdetails
                    .FirstOrDefault(x => x.Isdeleted == false &&
                    x.Materialsupplier.Material == CurrentImportDetail.Materialsupplier.Material &&
                    x.Materialsupplier.Price == CurrentImportDetail.Materialsupplier.Price &&
                    x.Materialsupplier.Original == CurrentImportDetail.Materialsupplier.Original &&
                    x.Materialsupplier.Manufacturer == CurrentImportDetail.Materialsupplier.Manufacturer &&
                    x.Materialsupplier.Manufacturedate == CurrentImportDetail.Materialsupplier.Manufacturedate &&
                    x.Materialsupplier.Expirationdate == CurrentImportDetail.Materialsupplier.Expirationdate);

                if (existedMateirlasupplier != null)
                {
                    MyMessageBox.Show("Chi tiết đã tồn tại, đã thêm số lượng");
                    existedMateirlasupplier.Quantity += CurrentImportDetail.Quantity;
                }
                else
                {
                    MyMessageBox.Show("Thêm lịch sử lương thành công");
                    ModifyImport.Importdetails.Add(CurrentImportDetail.Clone());
                }
            }
            else if (IsUpdatingImportDetail)
            {
                var find = ModifyImport.Importdetails.FirstOrDefault(x => x.Isdeleted == false && x.Id == CurrentImportDetail.Id);

                if (find != null)
                {
                    find.Materialsupplier.Material = CurrentImportDetail.Materialsupplier.Material;
                    find.Materialsupplier.Price = CurrentImportDetail.Materialsupplier.Price;
                    find.Materialsupplier.Original = CurrentImportDetail.Materialsupplier.Original;
                    find.Materialsupplier.Manufacturer = CurrentImportDetail.Materialsupplier.Manufacturer;
                    find.Materialsupplier.Manufacturedate = CurrentImportDetail.Materialsupplier.Manufacturedate;
                    find.Materialsupplier.Expirationdate = CurrentImportDetail.Materialsupplier.Expirationdate;
                    find.Quantity = CurrentImportDetail.Quantity;
                    MyMessageBox.Show("Sửa chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
                else
                {
                    MyMessageBox.Show("Sửa chi tiết đơn hàng thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            ClearAddImportDetail();
            OnPropertyChanged(nameof(ModifyImport));
            OnPropertyChanged(nameof(ListExisted));
        }

        [RelayCommand]
        private async void DeleteImportDetail(ImportDetailDTO importDetail)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No_Cancel, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                var deleted = ModifyImport.Importdetails.FirstOrDefault(x => x.Isdeleted == false && x.Id == importDetail.Id);
                if (deleted != null)
                {
                    deleted.Isdeleted = true;
                    MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    OnPropertyChanged(nameof(ModifyImport));
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
            CurrentImportDetail.Materialsupplier = importDetail.Materialsupplier.Clone();
            CurrentImportDetail.Materialsupplier.Material = importDetail.Materialsupplier.Material.Clone();
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
                MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công");
                AddSupplierVM.ClearValueOfFrom();
                IsOpenAddMaterialSupplier = false;
            }
            else
            {
                MyMessageBox.Show("Thêm nhà cung cấp thất bại");
            }
        }

        //Material
        private async void AddMaterialVM_ModifyMaterialChanged(MaterialDTO obj)
        {
            var addMaterial = await _materialSupplierServices.AddMaterial(_mapper.Map<Material>(obj));
            if (addMaterial != null)
            {
                ListMaterial.Add(_mapper.Map<MaterialDTO>(addMaterial));
                MyMessageBox.ShowDialog("Thêm nhà cung cấp thành công");
                AddSupplierVM.ClearValueOfFrom();
                IsOpenAddMaterialSupplier = false;
            }
            else
            {
                MyMessageBox.Show("Thêm nhà cung cấp thất bại");
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

        #endregion

        [RelayCommand]
        private void SubmitModifyImport()
        {
            
            if (IsAdding)
            {
                ModifyImport.Supplier = null;
                ModifyImport.Staff = null;
                if (ModifyImport.Importdetails != null)
                {
                    foreach (var importDetail in ModifyImport.Importdetails)
                    {
                        importDetail.Materialsupplier.Supplierid = ModifyImport.Supplierid;
                        importDetail.Materialsupplier.Material = null;
                    }
                }
                ModifyImport.Importdetails = [.. ModifyImport.Importdetails.Where(x => x.Isdeleted == false)];
                ImportChanged?.Invoke(ModifyImport.Clone());
            }
            if (IsUpdating)
            {
                ModifyImport.Importdetails.ToList().ForEach(x => x.Importid = ModifyImport.Importid);
                ImportChanged?.Invoke(ModifyImport.Clone());
            }
        }
    }
}