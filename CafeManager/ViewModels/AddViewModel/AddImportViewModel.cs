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

            // Clone danh sách ImportDetails và các tham chiếu liên quan
            ListExisted = new ObservableCollection<ImportDetailDTO>(
                ModifyImport.Importdetails.Where(x => !x.Isdeleted)
                                          .Select(x => new ImportDetailDTO
                                          {
                                              Id = x.Id,
                                              Importdetailid = x.Importdetailid,
                                              Importid = x.Importid,
                                              Materialsupplierid = x.Materialsupplierid,
                                              Isdeleted = x.Isdeleted,
                                              Quantity = x.Quantity,
                                              Materialsupplier = new MaterialSupplierDTO
                                              {
                                                  Id = x.Id,
                                                  Materialsupplierid = x.Materialsupplierid,
                                                  Materialid = x.Materialsupplier.Materialid,
                                                  Supplierid = x.Materialsupplier.Supplierid,
                                                  Manufacturedate = x.Materialsupplier.Manufacturedate,
                                                  Expirationdate = x.Materialsupplier.Expirationdate,
                                                  Original = x.Materialsupplier.Original,
                                                  Manufacturer = x.Materialsupplier.Manufacturer,
                                                  Price = x.Materialsupplier.Price,
                                                  Isdeleted = x.Materialsupplier.Isdeleted,
                                                  Material = x.Materialsupplier.Material.Clone()
                                              }
                                          }));
        }

        public void ClearValueOfViewModel()
        {
            IsAdding = false;
            IsUpdating = false;
            ClearAddImportDetail();
            ListExisted.Clear();
            listDeletedImportdetail.Clear();
        }

        [RelayCommand]
        private void ClearAddImportDetail()
        {
            IsAddingImportDetail = true;
            IsUpdatingImportDetail = false;
            CurrentImportDetail = new()
            {
                Materialsupplier = new()
                {
                    Material = new(),
                    Manufacturedate = DateTime.Now.Date,
                    Expirationdate = DateTime.Now.Date
                }
            };
        }

        [RelayCommand]
        private void ModifyImportDetail()
        {
            if (IsAddingImportDetail)
            {
                var existedMateirlasupplier = ListExisted
                    .FirstOrDefault(x => x.Isdeleted == false &&
                    x.Materialsupplier.Material.Materialid == CurrentImportDetail.Materialsupplier.Material.Materialid &&
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
                    CurrentImportDetail.Importid = ModifyImport.Importid;
                    MyMessageBox.Show("Thêm lịch sử lương thành công");
                    ListExisted.Add(CurrentImportDetail.Clone());
                }
            }
            else if (IsUpdatingImportDetail)
            {
                var find = ListExisted.FirstOrDefault(x => x.Isdeleted == false && x.Id == CurrentImportDetail.Id);

                if (find != null)
                {
                    find.Materialsupplier.Materialid = CurrentImportDetail.Materialsupplier.Materialid;
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
        }

        [RelayCommand]
        private async void DeleteImportDetail(ImportDetailDTO importDetail)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                var deleted = ListExisted.FirstOrDefault(x => x.Isdeleted == false && x.Id == importDetail.Id);
                if (deleted != null)
                {
                    deleted.Isdeleted = true;
                    if(IsUpdating)
                    {
                        listDeletedImportdetail.Add(deleted);
                    }
                    ListExisted.Remove(deleted);
                    MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
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
            ModifyImport.Importdetails = new ObservableCollection<ImportDetailDTO>(
                    ListExisted.Select(x => x.Clone())
                    .Concat(listDeletedImportdetail)
            );

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

            if (IsAdding)
            {
                ModifyImport.Importdetails = [.. ModifyImport.Importdetails.Where(x => x.Isdeleted == false)];
                ImportChanged?.Invoke(ModifyImport.Clone());
            }
            if (IsUpdating)
            {
                ModifyImport.Importdetails.ToList().ForEach(x => x.Importid = ModifyImport.Importid);
                ImportChanged?.Invoke(ModifyImport.Clone());
            }
            listDeletedImportdetail.Clear();
        }
    }
}