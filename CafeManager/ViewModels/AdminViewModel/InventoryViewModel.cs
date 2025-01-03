﻿using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CafeManager.WPF.Views.AddUpdateView;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
    public partial class InventoryViewModel : ObservableObject, IDataViewModel
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;
        private CancellationToken _token = default;
        private int _selectedTabIndex;
        private readonly int PageSize = 15;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        public bool _pageVisibility = false;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                switch (value)
                {
                    case 0:
                        consumedPageIndex = 1;
                        _ = LoadConsumedMaterial(_token);
                        break;

                    case 1:
                        inventoryPageIndex = 1;
                        _ = LoadInventory(_token);
                        break;

                    case 2:
                        usedUpPageIndex = 1;
                        _ = LoadUsedUpeMaterial(_token);
                        break;

                    case 3:
                        expiredPageIndex = 1;
                        _ = LoadExpiredMaterial(_token);
                        break;

                    default:
                        break;
                }
                ConsumedPage = (value == 0);
                InventoryPage = (value == 1);
                UsedUpPage = (value == 2);
                ExpiredPage = (value == 3);
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));

                OnPropertyChanged(nameof(ConsumedPage));
                OnPropertyChanged(nameof(InventoryPage));
                OnPropertyChanged(nameof(UsedUpPage));
                OnPropertyChanged(nameof(ExpiredPage));
            }
        }

        [ObservableProperty]
        private bool _consumedPage = true;

        [ObservableProperty]
        private bool _inventoryPage = false;

        [ObservableProperty]
        private bool _expiredPage = false;

        [ObservableProperty]
        private bool _usedUpPage = false;

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

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listInventoryDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listExpriedDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listUsedUp = [];

        #endregion Inventory Delcare

        // ===================== Filter Declare =====================

        #region Filter Declare

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
                    _ = LoadTableControl();
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
                    _ = LoadTableControl();
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
                    _ = LoadTableControl();
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
                    _ = LoadTableControl();
                }
            }
        }

        private DateTime? _selectedUsageDate;

        public DateTime? SelectedUsageDate
        {
            get => _selectedUsageDate; set
            {
                if (value != _selectedUsageDate)
                {
                    _selectedUsageDate = value;
                    OnPropertyChanged();
                    _ = LoadTableControl();
                }
            }
        }

        #endregion Filter Declare

        #region Hàm filter

        private Expression<Func<Consumedmaterial, bool>> ConsumedFilter => consum =>
            (consum.Isdeleted == false) &&
            (SelectedMaterial == null || consum.Materialsupplier.Materialid == SelectedMaterial.Materialid) &&
            (SelectedSupplier == null || consum.Materialsupplier.Supplierid == SelectedSupplier.Supplierid) &&
            (FilterManufacturedate == null || consum.Materialsupplier.Manufacturedate == FilterManufacturedate) &&
            (FilterExpirationdate == null || consum.Materialsupplier.Expirationdate == FilterExpirationdate) &&
            (SelectedUsageDate == null || (consum.Usagedate.Day == SelectedUsageDate.Value.Day && consum.Usagedate.Month == SelectedUsageDate.Value.Month && consum.Usagedate.Year == SelectedUsageDate.Value.Year));

        private Expression<Func<Materialsupplier, bool>> InventoryFilter => inventory =>
                        (inventory.Isdeleted == false) &&
                        (SelectedMaterial == null || inventory.Materialid == SelectedMaterial.Materialid) &&
                        (SelectedSupplier == null || inventory.Supplierid == SelectedSupplier.Supplierid) &&
                        (FilterManufacturedate == null || inventory.Manufacturedate == FilterManufacturedate) &&
                        (FilterExpirationdate == null || inventory.Expirationdate == FilterExpirationdate) &&
                        (inventory.Expirationdate >= DateTime.Now) &&
                        (inventory.Importdetails.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) - inventory.Consumedmaterials.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) > 0);

        private Expression<Func<Materialsupplier, bool>> UsedUpFilter => used =>
                (used.Isdeleted == false) &&
                (SelectedMaterial == null || used.Materialid == SelectedMaterial.Materialid) &&
                (SelectedSupplier == null || used.Supplierid == SelectedSupplier.Supplierid) &&
                (FilterManufacturedate == null || used.Manufacturedate == FilterManufacturedate) &&
                (FilterExpirationdate == null || used.Expirationdate == FilterExpirationdate) &&
                (used.Importdetails.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) - used.Consumedmaterials.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) == 0);

        private Expression<Func<Materialsupplier, bool>> ExpiredFilter => expired =>
                (expired.Isdeleted == false) &&
                (SelectedMaterial == null || expired.Materialid == SelectedMaterial.Materialid) &&
                (SelectedSupplier == null || expired.Supplierid == SelectedSupplier.Supplierid) &&
                (FilterManufacturedate == null || expired.Manufacturedate == FilterManufacturedate) &&
                (FilterExpirationdate == null || expired.Expirationdate == FilterExpirationdate) &&
                (expired.Expirationdate < DateTime.Now);

        private Expression<Func<Materialsupplier, bool>> inventoryFilter => inventory =>
                (inventory.Isdeleted == false) &&
                (SelectedMaterial == null || inventory.Material.Materialid == SelectedMaterial.Materialid) &&
                (SelectedSupplier == null || inventory.Supplier.Supplierid == SelectedSupplier.Supplierid) &&
                (FilterManufacturedate == null || inventory.Manufacturedate == FilterManufacturedate) &&
                (inventory.Importdetails.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) - inventory.Consumedmaterials.Where(x => x.Isdeleted == false).Sum(x => x.Quantity) > 0) &&
                (inventory.Expirationdate <= DateTime.Now.AddDays(15));

        private async Task LoadTableControl()
        {
            if (ConsumedPage)
            {
                consumedPageIndex = 1;
                await LoadConsumedMaterial(_token);
            }
            if (InventoryPage)
            {
                inventoryPageIndex = 1;
                await LoadInventory(_token);
            }
            if (UsedUpPage)
            {
                usedUpPageIndex = 1;
                await LoadUsedUpeMaterial(_token);
            }
            if (ExpiredPage)
            {
                expiredPageIndex = 1;
                await LoadExpiredMaterial(_token);
            }
        }

        #endregion Hàm filter

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
                _token = token;
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbMaterial = await _materialSupplierServices.GetListMaterial(token);
                var dbSupplier = (await _materialSupplierServices.GetListSupplier(token)).Where(x => x.Isdeleted == false);

                var listExistedMaterial = dbMaterial.Where(x => x.Isdeleted == false); // List vật liệu đang có
                var listDeletedMaterial = dbMaterial.Where(x => x.Isdeleted == true); // List vật liệu đã ẩn

                ListMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(listExistedMaterial)];
                MaterialVM.ListMaterial = [.. _mapper.Map<List<MaterialDTO>>(listExistedMaterial)];
                MaterialVM.ListDeletedMaterial = [.. _mapper.Map<List<MaterialDTO>>(listDeletedMaterial)];
                ListSupplierDTO = [.. _mapper.Map<List<SupplierDTO>>(dbSupplier)];

                SelectedTabIndex = 0;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của ImportViewModel bị hủy");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadInventory(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListInventory = await _materialSupplierServices.GetSearchPaginateListMaterialsupplier(InventoryFilter, inventoryPageIndex, PageSize, token);
                ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbListInventory.Item1)];
                InventoryTotalPages = (dbListInventory.Item2 + PageSize - 1) / PageSize;
                if (InventoryTotalPages > 0)
                {
                    PageVisibility = true;
                    OnPropertyChanged(nameof(InventoryPageUI));
                }
                else
                {
                    PageVisibility = false;
                }
                IsLoading = false;
            }
            catch (OperationCanceledException oe)
            {
                Debug.WriteLine(oe.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadConsumedMaterial(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListConsumed = await _consumedMaterialServices.GetSearchPaginateListConsumedMaterial(ConsumedFilter, consumedPageIndex, PageSize, token);
                ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbListConsumed.Item1)];
                ConsumedTotalPages = (dbListConsumed.Item2 + PageSize - 1) / PageSize;
                if (ConsumedTotalPages > 0)
                {
                    PageVisibility = true;
                    OnPropertyChanged(nameof(ConsumedPageUI));
                }
                else
                {
                    PageVisibility = false;
                }
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadUsedUpeMaterial(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListUsedUp = await _materialSupplierServices.GetSearchPaginateListMaterialsupplier(UsedUpFilter, usedUpPageIndex, PageSize, token);
                ListUsedUp = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbListUsedUp.Item1)];
                usedUpTotalPages = (dbListUsedUp.Item2 + PageSize - 1) / PageSize;
                if (usedUpTotalPages > 0)
                {
                    PageVisibility = true;
                    OnPropertyChanged(nameof(UsedUpPageUI));
                }
                else
                {
                    PageVisibility = false;
                }
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadExpiredMaterial(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListExpired = await _materialSupplierServices.GetSearchPaginateListMaterialsupplier(ExpiredFilter, expiredPageIndex, PageSize, token);
                ListExpriedDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbListExpired.Item1)];
                expiredTotalPages = (dbListExpired.Item2 + PageSize - 1) / PageSize;
                if (expiredTotalPages > 0)
                {
                    PageVisibility = true;
                    OnPropertyChanged(nameof(ExpiredPageUI));
                }
                else
                {
                    PageVisibility = false;
                }
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            finally { IsLoading = false; }
        }

        [RelayCommand]
        private async Task FilterExpiring()
        {
            IsLoading = true;
            var dbListInventory = await _materialSupplierServices.GetSearchPaginateListMaterialsupplier(inventoryFilter, inventoryPageIndex, PageSize);
            ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbListInventory.Item1)];
            InventoryTotalPages = (dbListInventory.Item2 + PageSize - 1) / PageSize;
            if (InventoryTotalPages > 0)
            {
                PageVisibility = true;
                OnPropertyChanged(nameof(InventoryPageUI));
            }
            else
            {
                PageVisibility = false;
            }
            IsLoading = false;
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

        #region Phan Trang ConsumedMaterial

        private int consumedPageIndex = 1;

        [ObservableProperty]
        private int consumedTotalPages = 0;

        public string ConsumedPageUI => $"{consumedPageIndex}/{ConsumedTotalPages}";

        [RelayCommand]
        private async Task ConsumedFirstPage()
        {
            consumedPageIndex = 1;

            await LoadConsumedMaterial(_token);
        }

        [RelayCommand]
        private async Task ConsumedNextPage()
        {
            if (consumedPageIndex == ConsumedTotalPages)
            {
                return;
            }
            consumedPageIndex += 1;
            await LoadConsumedMaterial(_token);
        }

        [RelayCommand]
        private async Task ConsumedPreviousPage()
        {
            if (consumedPageIndex == 1)
            {
                return;
            }
            consumedPageIndex -= 1;
            await LoadConsumedMaterial(_token);
        }

        [RelayCommand]
        private async Task ConsumedLastPage()
        {
            consumedPageIndex = ConsumedTotalPages;
            await LoadConsumedMaterial(_token);
        }

        #endregion Phan Trang ConsumedMaterial

        #region Phan trang Inventory

        private int inventoryPageIndex = 1;

        [ObservableProperty]
        private int inventoryTotalPages = 0;

        public string InventoryPageUI => $"{inventoryPageIndex}/{InventoryTotalPages}";

        [RelayCommand]
        private async Task InventoryFirstPage()
        {
            inventoryPageIndex = 1;

            await LoadInventory(_token);
        }

        [RelayCommand]
        private async Task InventoryNextPage()
        {
            if (inventoryPageIndex == InventoryTotalPages)
            {
                return;
            }
            inventoryPageIndex += 1;
            await LoadInventory(_token);
        }

        [RelayCommand]
        private async Task InventoryPreviousPage()
        {
            if (inventoryPageIndex == 1)
            {
                return;
            }
            inventoryPageIndex -= 1;
            await LoadInventory(_token);
        }

        [RelayCommand]
        private async Task InventoryLastPage()
        {
            inventoryPageIndex = InventoryTotalPages;
            await LoadInventory(_token);
        }

        #endregion Phan trang Inventory

        #region Phan trang Used Up

        private int usedUpPageIndex = 1;
        private int usedUpTotalPages = 0;

        public string UsedUpPageUI => $"{usedUpPageIndex}/{usedUpTotalPages}";

        [RelayCommand]
        private async Task UsedUpFirstPage()
        {
            usedUpPageIndex = 1;

            await LoadUsedUpeMaterial(_token);
        }

        [RelayCommand]
        private async Task UsedUpNextPage()
        {
            if (usedUpPageIndex == usedUpTotalPages)
            {
                return;
            }
            usedUpPageIndex += 1;
            await LoadUsedUpeMaterial(_token);
        }

        [RelayCommand]
        private async Task UsedUpPreviousPage()
        {
            if (usedUpPageIndex == 1)
            {
                return;
            }
            usedUpPageIndex -= 1;
            await LoadUsedUpeMaterial(_token);
        }

        [RelayCommand]
        private async Task UsedUpLastPage()
        {
            usedUpPageIndex = usedUpTotalPages;
            await LoadUsedUpeMaterial(_token);
        }

        #endregion Phan trang Used Up

        #region Phan trang expired

        private int expiredPageIndex = 1;
        private int expiredTotalPages = 0;

        public string ExpiredPageUI => $"{expiredPageIndex}/{expiredTotalPages}";

        [RelayCommand]
        private async Task ExpiredFirstPage()
        {
            expiredPageIndex = 1;

            await LoadExpiredMaterial(_token);
        }

        [RelayCommand]
        private async Task ExpiredNextPage()
        {
            if (expiredPageIndex == expiredTotalPages)
            {
                return;
            }
            expiredPageIndex += 1;
            await LoadExpiredMaterial(_token);
        }

        [RelayCommand]
        private async Task ExpiredPreviousPage()
        {
            if (expiredPageIndex == 1)
            {
                return;
            }
            expiredPageIndex -= 1;
            await LoadExpiredMaterial(_token);
        }

        [RelayCommand]
        private async Task ExpiredLastPage()
        {
            expiredPageIndex = expiredTotalPages;
            await LoadExpiredMaterial(_token);
        }

        #endregion Phan trang expired
    }
}