﻿using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class AddImportViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly ImportDetailServices _importdetailServices;
        private readonly MaterialSupplierServices _materialSupplierServices;
        public IMapper _mapper;

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

        private ImportMaterialDetailDTO _updateImportDetail = new();

        [ObservableProperty]
        private decimal _importPrice = 0;

        [ObservableProperty]
        private ObservableCollection<ImportMaterialDetailDTO> _currentListImportdetail = [];

        [ObservableProperty]
        private ObservableCollection<Staff> _listStaff = [];

        [ObservableProperty]
        private ObservableCollection<SupplierDTO> _listSupplier = [];

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listMaterial = [];

        public event Action<Import, List<ImportMaterialDetailDTO>?> ImportChanged;

        public event Action Close;

        [ObservableProperty]
        private object _selectedViewModel;

        [ObservableProperty]
        private MaterialDTO _selectedMaterial = new();

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
            ImportPrice = 0;
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
                if (IsAdding)
                {
                    var find = CurrentListImportdetail.FirstOrDefault(x => x.Materialid == _updateImportDetail.Materialid
                    && x.Price == _updateImportDetail.Price && x.Original == _updateImportDetail.Original
                    && x.Manufacturer == _updateImportDetail.Manufacturer && x.Manufacturedate == _updateImportDetail.Manufacturedate
                    && x.Expirationdate == _updateImportDetail.Expirationdate && x.Quantity == _updateImportDetail.Quantity);
                    if (find != null)
                    {
                        find.Materialname = CurrentImportDetail.Materialname;
                        find.Price = CurrentImportDetail.Price;
                        find.Original = CurrentImportDetail.Original;
                        find.Manufacturer = CurrentImportDetail.Manufacturer;
                        find.Manufacturedate = CurrentImportDetail.Manufacturedate;
                        find.Expirationdate = CurrentImportDetail.Expirationdate;
                        find.Quantity = CurrentImportDetail.Quantity;
                    }
                }
                else
                {
                    var find = CurrentListImportdetail.FirstOrDefault(x => x.Importdetailid == _updateImportDetail.Importdetailid);
                    if (find != null)
                    {
                        find.Materialname = CurrentImportDetail.Materialname;
                        find.Price = CurrentImportDetail.Price;
                        find.Original = CurrentImportDetail.Original;
                        find.Manufacturer = CurrentImportDetail.Manufacturer;
                        find.Manufacturedate = CurrentImportDetail.Manufacturedate;
                        find.Expirationdate = CurrentImportDetail.Expirationdate;
                        find.Quantity = CurrentImportDetail.Quantity;
                    }
                }
                MyMessageBox.Show("Sửa chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            ClearAddImportDetail();
            CurrentListImportdetail = [.. CurrentListImportdetail];
            ReloadImportPrice();
        }

        [RelayCommand]
        private async void DeleteImportDetail(ImportMaterialDetailDTO importDetail)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa chi tiết đơn hàng này ko", MyMessageBox.Buttons.Yes_No_Cancel, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                var isDeleted = await _importdetailServices.DeleteImportdetail(importDetail);
                if (isDeleted)
                {
                    CurrentListImportdetail.Remove(importDetail);
                    MyMessageBox.Show("Xoá chi tiết đơn hàng thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            ReloadImportPrice();
        }

        [RelayCommand]
        private void UpdateImportDetail(ImportMaterialDetailDTO value)
        {
            IsUpdatingImportDetail = true;
            IsAddingImportDetail = false;

            _updateImportDetail = value;
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
            ReloadImportPrice();
        }

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

        [RelayCommand]
        private void SubmitModifyImport()
        {
            ImportChanged?.Invoke(ModifyImport, CurrentListImportdetail.ToList());
        }

        [RelayCommand]
        private void ReloadImportPrice()
        {
            ImportPrice = 0;
            foreach (var item in CurrentListImportdetail)
            {
                ImportPrice += item.Price * item.Quantity;
            }
        }

        [RelayCommand]
        private void CloseAddImport()
        {
            Close?.Invoke();
            ClearValueOfViewModel();
        }
    }
}