using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CafeManager.WPF.Views.AddUpdateView;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using PdfSharp;
using PdfSharp.Pdf.Filters;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InventoryViewModel : ObservableObject, IDataViewModel, IDisposable
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;

        // ===================== Material Declare =====================
        #region Material Declare
        [ObservableProperty]
        private List<MaterialDTO> _listMaterialDTO = [];

        [ObservableProperty]
        private List<SupplierDTO> _listSupplierDTO = [];

        [ObservableProperty]
        private bool _isOpenMaterialView = false;

        [ObservableProperty]
        private MaterialViewModel _materialVM;

        #endregion Material Declare

        // ===================== Inventory Declare =====================
        #region Inventory Delcare

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private ConsumedMaterialDTO _modifyConsumedMaterial = new();

        private decimal currentQuantity;

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];
        //public ObservableCollection<ConsumedMaterialDTO> CurrentListConsumedMaterial => [.. _filterListConsumedMaterial];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listInventoryDTO = [];
        //public ObservableCollection<MaterialSupplierDTO> CurrentListInventory => [.. _filterListInventory];

        //private List<ConsumedMaterialDTO> _filterListConsumedMaterial = [];
        //private List<MaterialSupplierDTO> _filterListInventory = [];

        #endregion Inventory Delcare

        // ===================== Filter Declare =====================
        #region Filter Declare

        [ObservableProperty]
        private bool _isFilterExpiring = false;

        [ObservableProperty]
        private bool _isFilterExpired = false;

        private SupplierDTO? _selectedSupplier;

        public SupplierDTO? SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                if (_selectedSupplier != value)
                {
                    _selectedSupplier = value;
                    OnPropertyChanged(nameof(SelectedSupplier));
                    _ = LoadInventory();
                }
            }
        }

        private MaterialDTO? _selectedMaterial;

        public MaterialDTO? SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                if (_selectedMaterial != value)
                {
                    _selectedMaterial = value;
                    OnPropertyChanged(nameof(SelectedMaterial));
                    _ = LoadInventory();
                }
            }
        }

        private DateTime? _filterManufacturedate;

        public DateTime? FilterManufacturedate
        {
            get => _filterManufacturedate;
            set
            {
                if (_filterManufacturedate != value)
                {
                    _filterManufacturedate = value;
                    OnPropertyChanged(nameof(FilterManufacturedate));
                    _ = LoadInventory();
                }
            }
        }

        private DateTime? _filterExpirationdate;

        public DateTime? FilterExpirationdate
        {
            get => _filterExpirationdate;
            set
            {
                if (_filterExpirationdate != value)
                {
                    _filterExpirationdate = value;
                    OnPropertyChanged(nameof(FilterExpirationdate));
                    _ = LoadInventory();
                }
            }
        }

        #endregion Filter Declare

        public InventoryViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();
            MaterialVM = provider.GetRequiredService<MaterialViewModel>();
            _mapper = provider.GetRequiredService<IMapper>();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var dbMaterial = await _materialSupplierServices.GetListMaterial(token);
                var dbSupplier = (await _materialSupplierServices.GetListSupplier(token)).Where(x => x.Isdeleted == false);

                var listExistedMaterial = dbMaterial.Where(x => x.Isdeleted == false); // List vật liệu đang có
                var listDeletedMaterial = dbMaterial.Where(x => x.Isdeleted == true); // List vật liệu đã ẩn

                ListMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(listExistedMaterial)];
                MaterialVM.ListMaterial = [.. _mapper.Map<List<MaterialDTO>>(listExistedMaterial)];
                MaterialVM.ListDeletedMaterial = [.. _mapper.Map<List<MaterialDTO>>(listDeletedMaterial)];
                ListSupplierDTO = [.. _mapper.Map<List<SupplierDTO>>(dbSupplier)];
                
                await LoadInventory(token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của ImportViewModel bị hủy");
            }
        }

        //private async Task CheckTotalQuantity()
        //{
        //    var itemsToRemove = new List<MaterialSupplierDTO>();

        //    foreach (var item in ListInventoryDTO)
        //    {
        //        if (item.TotalQuantity == 0)
        //        {
        //            var materialsupplier = await _materialSupplierServices.GetMaterialsupplierById(item.Materialsupplierid);
        //            item.Isdeleted = true;
        //            itemsToRemove.Add(item);
        //        }
        //    }

        //    foreach (var item in itemsToRemove)
        //    {
        //        ListInventoryDTO.Remove(item);
        //    }
        //}

        private async Task LoadInventory(CancellationToken token = default)
        {
            Expression<Func<Consumedmaterial, bool>> consumedFilter;
            Expression<Func<Materialsupplier, bool>> inventoryFilter;
            if (IsFilterExpiring)
            {
                consumedFilter = consumedMaterial =>
                    (consumedMaterial.Isdeleted == false) &&
                    (SelectedMaterial == null || consumedMaterial.Materialsupplier.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || consumedMaterial.Materialsupplier.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                    (consumedMaterial.Materialsupplier.Expirationdate >= DateTime.Now) &&
                    ((consumedMaterial.Materialsupplier.Expirationdate - DateTime.Now).TotalDays <= 15);

                inventoryFilter = inventory =>
                (inventory.Isdeleted == false) &&
                (SelectedMaterial == null || inventory.Material.Materialid == SelectedMaterial.Materialid) &&
                (SelectedSupplier == null || inventory.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                (inventory.Expirationdate >= DateTime.Now) &&
                ((inventory.Expirationdate - DateTime.Now).TotalDays <= 15);
            }
            else if (IsFilterExpired)
            {
                consumedFilter = consumedMaterial =>
                    (consumedMaterial.Isdeleted == false) &&
                    (SelectedMaterial == null || consumedMaterial.Materialsupplier.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || consumedMaterial.Materialsupplier.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                    (consumedMaterial.Materialsupplier.Expirationdate < DateTime.Now);

                inventoryFilter = inventory =>
                    (inventory.Isdeleted == false) &&
                    (SelectedMaterial == null || inventory.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || inventory.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                    (inventory.Expirationdate < DateTime.Now);
            }
            else
            {
                consumedFilter = consumedMaterial =>
                    (consumedMaterial.Isdeleted == false) &&
                    (SelectedMaterial == null || consumedMaterial.Materialsupplier.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || consumedMaterial.Materialsupplier.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                    (FilterManufacturedate == null || consumedMaterial.Materialsupplier.Manufacturedate == FilterManufacturedate) &&
                    (FilterExpirationdate == null || consumedMaterial.Materialsupplier.Expirationdate == FilterExpirationdate);

                inventoryFilter = inventory =>
                    (inventory.Isdeleted == false) &&
                    (SelectedMaterial == null || inventory.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || inventory.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                    (FilterManufacturedate == null || inventory.Manufacturedate == FilterManufacturedate) &&
                    (FilterExpirationdate == null || inventory.Expirationdate == FilterExpirationdate);
            }
            var dbListConsumedMaterial = await _consumedMaterialServices.GetSearchPaginateListConsumedMaterial(consumedFilter, consumedPageIndex, consumedPageSize);
            var dbListInventory = await _materialSupplierServices.GetSearchPaginateListMaterialsupplier(inventoryFilter, inventoryPageIndex, inventoryPageSize);
            ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbListInventory.Item1)];
            ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbListConsumedMaterial.Item1)];
            consumedTotalPages = (dbListConsumedMaterial.Item2 + consumedPageSize - 1) / consumedPageSize;
            inventoryTotalPages = (dbListInventory.Item2 + inventoryPageSize - 1) / inventoryPageSize;
            OnPropertyChanged(nameof(ConsumedPageUI));
            OnPropertyChanged(nameof(InventoryPageUI));
        }

        [RelayCommand]
        private void FilterExpiring()
        {
            if (IsFilterExpiring)
            {
                IsFilterExpiring = false;
            }
            else
            {
                IsFilterExpiring = true;
                IsFilterExpired = false;
            }
            _ = LoadInventory();
        }

        [RelayCommand]
        private void FilterExpired()
        {
            if (IsFilterExpired)
            {
                IsFilterExpired = false;
            }
            else
            {
                IsFilterExpired = true;
                IsFilterExpiring = false;
            }
            _ = LoadInventory();
        }

        #region Material
        [RelayCommand]
        private void OpenMaterialView()
        {
            IsOpenMaterialView = true;
        }

        [RelayCommand]
        private void CloseMaterialView()
        {
            IsOpenMaterialView = false;
            ListMaterialDTO = MaterialVM.ListMaterial.ToList();
        }

        #endregion Material

        #region ConsumedMaterial

        //[RelayCommand]
        //private async Task SubmitConsumedMaterial()
        //{
        //    bool ok = false;
        //    try
        //    {
        //        if (!IsValidData())
        //            throw new InvalidOperationException("Số lượng trong kho nhỏ hơn yêu cầu");
        //        if (IsAdding)
        //        {
        //            var addConsumedMaterial = await _consumedMaterialServices.AddConsumedmaterial(_mapper.Map<Consumedmaterial>(ModifyConsumedMaterial));
        //            if (addConsumedMaterial != null)
        //            {
        //                var res = ListConsumedMaterialDTO.FirstOrDefault(x => x.Consumedmaterialid == addConsumedMaterial.Consumedmaterialid);
        //                if (res != null)
        //                {
        //                    _mapper.Map(addConsumedMaterial, res);
        //                }
        //                else
        //                {
        //                    ListConsumedMaterialDTO.Add(_mapper.Map<ConsumedMaterialDTO>(addConsumedMaterial));
        //                }
        //                MyMessageBox.ShowDialog("Thêm chi tiết sử dụng vật liệu cấp thành công");
        //                ok = true;
        //            }
        //            else
        //            {
        //                MyMessageBox.Show("Thêm chi tiết sử dụng vật liệu thất bại");
        //            }
        //        }
        //        if (IsUpdating)
        //        {
        //            var res = await _consumedMaterialServices.UpdateConsumedMaterialById(ModifyConsumedMaterial.Consumedmaterialid, _mapper.Map<Consumedmaterial>(ModifyConsumedMaterial));
        //            if (res != null)
        //            {
        //                var updateConsumedMaterialDTO = ListConsumedMaterialDTO.FirstOrDefault(x => x.Consumedmaterialid == res.Consumedmaterialid);
        //                if (updateConsumedMaterialDTO != null)
        //                {
        //                    _mapper.Map(res, updateConsumedMaterialDTO);
        //                    MyMessageBox.ShowDialog("Sửa chi tiết sử dụng vật liệu thành công");
        //                    ok = true;
        //                }
        //            }
        //            else
        //            {
        //                MyMessageBox.ShowDialog("Sửa nhà cung cấp thất bại");
        //            }
        //        }
        //        if (ok)
        //        {
        //            var ms = await _materialSupplierServices.GetMaterialsupplierById(ModifyConsumedMaterial.Materialsupplierid);
        //            var updateMaterialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == ModifyConsumedMaterial.Materialsupplierid);
        //            _mapper.Map(ms, updateMaterialSupplier);
        //            ListInventoryDTO = [.. ListInventoryDTO];
        //            ListConsumedMaterialDTO = [.. ListConsumedMaterialDTO];
        //            ClearValueOfFrom();
        //            IsOpenModifyConsumed = false;
        //            FilterInventory();
        //        }
        //    }
        //    catch (InvalidOperationException ioe)
        //    {
        //        MyMessageBox.ShowDialog(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
        //    }
        //}

        //private bool IsValidData()
        //{
        //    var materialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == ModifyConsumedMaterial.Materialsupplierid);
        //    if (IsAdding)
        //    {
        //        return materialSupplier?.TotalQuantity - ModifyConsumedMaterial.Quantity >= 0;
        //    }
        //    if (IsUpdating)
        //    {
        //        return materialSupplier?.TotalQuantity - (ModifyConsumedMaterial.Quantity - currentQuantity) >= 0;
        //    }
        //    return false;
        //}

        //public void ClearValueOfFrom()
        //{
        //    IsAdding = false;
        //    IsUpdating = false;
        //    ModifyConsumedMaterial = new();
        //    currentQuantity = 0;
        //}

        //[RelayCommand]
        //private void OpenModifyConsumed_Add(MaterialSupplierDTO materialSupplier)
        //{
        //    ModifyConsumedMaterial.Materialsupplierid = materialSupplier.Materialsupplierid;

        //    IsOpenModifyConsumed = true;
        //    IsAdding = true;
        //}

        //[RelayCommand]
        //private void OpenModifyConsumed_Update(ConsumedMaterialDTO consumedMaterialDTO)
        //{
        //    SelectedMaterial = null;
        //    currentQuantity = consumedMaterialDTO.Quantity;
        //    ModifyConsumedMaterial = consumedMaterialDTO.Clone();
        //    IsOpenModifyConsumed = true;
        //    IsUpdating = true;
        //}

        //[RelayCommand]
        //private async Task DeleteConsumedMaterial(ConsumedMaterialDTO consumedMaterial)
        //{
        //    try
        //    {
        //        string messageBox = MyMessageBox.ShowDialog("Bạn có muốn xoá chi tiết sử dụng vật liệu này không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
        //        if (messageBox.Equals("1"))
        //        {
        //            bool isDeleted = await _consumedMaterialServices.DeleteConsumedmaterial(consumedMaterial.Consumedmaterialid);
        //            if (isDeleted)
        //            {
        //                var ms = await _materialSupplierServices.GetMaterialsupplierById(consumedMaterial.Materialsupplierid);
        //                var updateMaterialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == consumedMaterial.Materialsupplierid);
        //                _mapper.Map(ms, updateMaterialSupplier);
        //                ListInventoryDTO = [.. ListInventoryDTO];

        //                ListConsumedMaterialDTO.Remove(consumedMaterial);
        //                MyMessageBox.ShowDialog("Xoá chi tiết sử dụng vật liệu thanh công");
        //            }
        //            else
        //            {
        //                MyMessageBox.ShowDialog("Xoá chi tiết sử dụng vật liệu thất bại");
        //            }
        //        }
        //    }
        //    catch (InvalidOperationException ioe)
        //    {
        //        MyMessageBox.ShowDialog(ioe.Message);
        //    }
        //    FilterInventory();
        //}

        //[RelayCommand]
        //private void CloseModifyConsumed()
        //{
        //    IsOpenModifyConsumed = false;
        //    ClearValueOfFrom();
        //}

        #endregion ConsumedMaterial

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Phan Trang ConsumedMaterial
        private int consumedPageIndex = 1;

        private int consumedPageSize = 12;
        private int consumedTotalPages = 0;

        public string ConsumedPageUI => $"{consumedPageIndex}/{consumedTotalPages}";

        [RelayCommand]
        private async Task ConsumedFirstPage()
        {
            consumedPageIndex = 1;

            await LoadData();
        }

        [RelayCommand]
        private async Task ConsumedNextPage()
        {
            if (consumedPageIndex == consumedTotalPages)
            {
                return;
            }
            consumedPageIndex += 1;
            await LoadData();
        }

        [RelayCommand]
        private async Task ConsumedPreviousPage()
        {
            if (consumedPageIndex == 1)
            {
                return;
            }
            consumedPageIndex -= 1;
            await LoadData();
        }

        [RelayCommand]
        private async Task ConsumedLastPage()
        {
            consumedPageIndex = consumedTotalPages;
            await LoadData();
        }
        #endregion

        #region Phan trang Inventory
        private int inventoryPageIndex = 1;

        private int inventoryPageSize = 12;
        private int inventoryTotalPages = 0;

        public string InventoryPageUI => $"{inventoryPageIndex}/{inventoryTotalPages}";

        [RelayCommand]
        private async Task InventoryFirstPage()
        {
            inventoryPageIndex = 1;

            await LoadData();
        }

        [RelayCommand]
        private async Task InventoryNextPage()
        {
            if (inventoryPageIndex == inventoryTotalPages)
            {
                return;
            }
            inventoryPageIndex += 1;
            await LoadData();
        }

        [RelayCommand]
        private async Task InventoryPreviousPage()
        {
            if (inventoryPageIndex == 1)
            {
                return;
            }
            inventoryPageIndex -= 1;
            await LoadData();
        }

        [RelayCommand]
        private async Task InventoryLastPage()
        {
            inventoryPageIndex = inventoryTotalPages;
            await LoadData();
        }
        #endregion
    }
}