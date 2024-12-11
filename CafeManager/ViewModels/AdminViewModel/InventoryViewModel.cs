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
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InventoryViewModel : ObservableObject, IDataViewModel, IDisposable
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;

        #region Material Declare

        [ObservableProperty]
        private bool _isOpenAddMaterial = false;

        [ObservableProperty]
        private bool _isPopupOpen = false;

        [ObservableProperty]
        private bool _isOpenModifyConsumed = false;

        [ObservableProperty]
        private bool _isGetMaterial = true;

        public bool IsDeletedMaterial => !IsGetMaterial;

        [ObservableProperty]
        private List<SupplierDTO> _ListSupplierDTO = [];

        [ObservableProperty]
        private List<MaterialDTO> _listMaterialDTO = [];

        [ObservableProperty]
        private List<MaterialDTO> _listDeletedMaterialDTO = [];

        [ObservableProperty]
        private MaterialDTO? _deletedMaterial;

        #endregion Material Declare

        #region Inventory Delcare

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private ConsumedMaterialDTO _modifyConsumedMaterial = new();

        private decimal currentQuantity;

        [ObservableProperty]
        private AddMaterialViewModel _modifyMaterialVM;

        private ObservableCollection<ConsumedMaterialDTO> ListConsumedMaterialDTO = [];
        public ObservableCollection<ConsumedMaterialDTO> CurrentListConsumedMaterial => [.. _filterListConsumedMaterial];

        private ObservableCollection<MaterialSupplierDTO> ListInventoryDTO = [];
        public ObservableCollection<MaterialSupplierDTO> CurrentListInventory => [.. _filterListInventory];

        private List<ConsumedMaterialDTO> _filterListConsumedMaterial = [];
        private List<MaterialSupplierDTO> _filterListInventory = [];

        #endregion Inventory Delcare

        #region Filter Declare

        [ObservableProperty]
        private bool _isFillterAll = true;

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
                    FilterInventory();
                    OnPropertyChanged(nameof(SelectedSupplier));
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
                    FilterInventory();
                    OnPropertyChanged(nameof(SelectedMaterial));
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
                    FilterInventory();
                    OnPropertyChanged(nameof(FilterManufacturedate));
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
                    FilterInventory();
                    OnPropertyChanged(nameof(FilterExpirationdate));
                }
            }
        }

        #endregion Filter Declare

        public InventoryViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();

            ModifyMaterialVM = provider.GetRequiredService<AddMaterialViewModel>();
            ModifyMaterialVM.ModifyMaterialChanged += ModifyMaterialVM_ModifyMaterialChanged;
            ModifyMaterialVM.Close += ModifyMaterialVM_Close;

            _mapper = provider.GetRequiredService<IMapper>();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var dbMaterialSupplier = (await _materialSupplierServices.GetListMaterialSupplier(token)).Where(x => x.Isdeleted == false);
                var dbConsumedMaterial = (await _consumedMaterialServices.GetListConsumedMaterial(token)).Where(x => x.Isdeleted == false);
                var dbMaterial = await _materialSupplierServices.GetListMaterial(token);
                var dbSupplier = (await _materialSupplierServices.GetListSupplier(token)).Where(x => x.Isdeleted == false);

                var listExistedMaterial = dbMaterial.Where(x => x.Isdeleted == false); // List vật liệu đang có
                var listDeletedMaterial = dbMaterial.Where(x => x.Isdeleted == true); // List vật liệu đã ẩn

                ListMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(listExistedMaterial)];
                ListDeletedMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(listDeletedMaterial)];
                ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbConsumedMaterial)];
                ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbMaterialSupplier)];
                ListSupplierDTO = [.. _mapper.Map<List<SupplierDTO>>(dbSupplier)];

                await CheckTotalQuantity();

                _filterListConsumedMaterial = [.. ListConsumedMaterialDTO];
                OnPropertyChanged(nameof(CurrentListConsumedMaterial));
                _filterListInventory = [.. ListInventoryDTO];
                OnPropertyChanged(nameof(CurrentListInventory));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của ImportViewModel bị hủy");
            }
        }

        private async Task CheckTotalQuantity()
        {
            var itemsToRemove = new List<MaterialSupplierDTO>();

            foreach (var item in ListInventoryDTO)
            {
                if (item.TotalQuantity == 0)
                {
                    var materialsupplier = await _materialSupplierServices.GetMaterialsupplierById(item.Materialsupplierid);
                    item.Isdeleted = true;
                    itemsToRemove.Add(item);
                }
            }

            foreach (var item in itemsToRemove)
            {
                ListInventoryDTO.Remove(item);
            }
        }

        private void FilterInventory()
        {
            var filterConsumed = ListConsumedMaterialDTO
                .Where(x =>
                    (SelectedMaterial == null || x.Materialsupplier.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || x.Materialsupplier.Supplier.Supplierid == SelectedSupplier.Supplierid))
                .ToList();

            var filterInventory = ListInventoryDTO
                .Where(x =>
                    (SelectedMaterial == null || x.Material.Materialid == SelectedMaterial.Materialid) &&
                    (SelectedSupplier == null || x.Supplier.Supplierid == SelectedSupplier.Supplierid))
                .ToList();

            if (IsFillterAll)
            {
                filterConsumed = filterConsumed.Where(x =>
                    (FilterManufacturedate == null || x.Materialsupplier.Manufacturedate == FilterManufacturedate) &&
                    (FilterExpirationdate == null || x.Materialsupplier.Expirationdate == FilterExpirationdate))
                    .ToList();
                filterInventory = filterInventory.Where(x =>
                    (FilterManufacturedate == null || x.Manufacturedate == FilterManufacturedate) &&
                    (FilterExpirationdate == null || x.Expirationdate == FilterExpirationdate))
                    .ToList();
            }
            else if (IsFilterExpiring)
            {
                filterConsumed = filterConsumed.Where(x =>
                    (x.Materialsupplier.Expirationdate >= DateTime.Now) &&
                   (x.Materialsupplier.Expirationdate - DateTime.Now).TotalDays <= 15)
                    .ToList();
                filterInventory = filterInventory.Where(x =>
                    (x.Expirationdate >= DateTime.Now) &&
                    (x.Expirationdate - DateTime.Now).TotalDays <= 15)
                    .ToList();
            }
            else
            {
                filterConsumed = filterConsumed.Where(x =>
                    x.Materialsupplier.Expirationdate < DateTime.Now)
                    .ToList();
                filterInventory = filterInventory.Where(x =>
                    x.Expirationdate < DateTime.Now)
                    .ToList();
            }

            _filterListConsumedMaterial = [.. filterConsumed];
            OnPropertyChanged(nameof(CurrentListConsumedMaterial));
            _filterListInventory = [.. filterInventory];
            OnPropertyChanged(nameof(CurrentListInventory));
        }

        [RelayCommand]
        private void FilterAll()
        {
            IsFillterAll = true;
            IsFilterExpiring = IsFilterExpired = false;
            FilterInventory();
        }

        [RelayCommand]
        private void FilterExpiring()
        {
            IsFilterExpiring = true;
            IsFillterAll = IsFilterExpired = false;
            FilterInventory();
        }

        [RelayCommand]
        private void FilterExpired()
        {
            IsFilterExpired = true;
            IsFilterExpiring = IsFillterAll = false;
            FilterInventory();
        }

        #region Material

        [RelayCommand]
        private void OpenPopupMaterialView()
        {
            IsPopupOpen = true;
        }

        private async void ModifyMaterialVM_ModifyMaterialChanged(MaterialDTO obj)
        {
            try
            {
                if (ModifyMaterialVM.IsAdding)
                {
                    var addMaterial = await _materialSupplierServices.AddMaterial(_mapper.Map<Material>(obj));
                    if (addMaterial != null)
                    {
                        ListMaterialDTO.Add(_mapper.Map<MaterialDTO>(addMaterial));
                        MyMessageBox.ShowDialog("Thêm vật tư cấp thành công");
                        ModifyMaterialVM.ClearValueOfFrom();
                        IsOpenAddMaterial = false;
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm vật tư cấp thất bại");
                    }
                }
                if (ModifyMaterialVM.IsUpdating)
                {
                    var res = await _materialSupplierServices.UpdateMaterialById(obj.Materialid, _mapper.Map<Material>(obj));
                    if (res != null)
                    {
                        var updateSupplierDTO = ListMaterialDTO.FirstOrDefault(x => x.Materialid == res.Materialid);
                        if (updateSupplierDTO != null)
                        {
                            _mapper.Map(res, updateSupplierDTO);
                            MyMessageBox.ShowDialog("Sửa vật tư cấp thành công");
                            ModifyMaterialVM.ClearValueOfFrom();
                            IsOpenAddMaterial = false;
                        }
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa nhà cung cấp thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        private void ModifyMaterialVM_Close()
        {
            IsOpenAddMaterial = false;
            ModifyMaterialVM.ClearValueOfFrom();
        }

        [RelayCommand]
        private void OpenAddMaterial()
        {
            IsPopupOpen = false;
            IsOpenAddMaterial = true;
            ModifyMaterialVM.IsAdding = true;
            ModifyMaterialVM.ModifyMaterial = new();
        }

        [RelayCommand]
        private void OpenUpdateMaterial()
        {
            if (SelectedMaterial == null || SelectedMaterial.Materialid == -1)
            {
                MyMessageBox.ShowDialog("Hãy chọn vật tư", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
                return;
            }
            IsPopupOpen = false;
            IsOpenAddMaterial = true;
            ModifyMaterialVM.IsUpdating = true;
            ModifyMaterialVM.ReceiveMaterialDTO(SelectedMaterial);
        }

        [RelayCommand]
        private async Task DeleteMaterial()
        {
            IsPopupOpen = false;
            try
            {
                if (SelectedMaterial == null || SelectedMaterial.Materialid == -1)
                    throw new InvalidOperationException("Hãy chọn vật tư");

                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn ẩn vật tư không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    bool isDeleted = await _materialSupplierServices.DeleteMaterial(SelectedMaterial.Materialid);
                    if (isDeleted)
                    {
                        ListDeletedMaterialDTO.Add(SelectedMaterial);
                        ListMaterialDTO.Remove(SelectedMaterial);
                        MyMessageBox.ShowDialog("Ẩn vật tư thanh công");
                        SelectedMaterial = null;
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Ẩn vật tư cấp thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task RestoreMaterial()
        {
            try
            {
                if (DeletedMaterial == null)
                    throw new InvalidOperationException("Hãy chọn vật tư");

                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn hiển thị vật tư không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    DeletedMaterial.Isdeleted = false;
                    var res = await _materialSupplierServices.UpdateMaterialById(DeletedMaterial.Materialid, _mapper.Map<Material>(DeletedMaterial));
                    if (res != null)
                    {
                        ListMaterialDTO.Add(DeletedMaterial);
                        ListDeletedMaterialDTO.Remove(DeletedMaterial);
                        MyMessageBox.ShowDialog("Hiển thị vật tư thanh công");
                        DeletedMaterial = null;
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Hiển thị vật tư thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private void ChangeRoleMaterial()
        {
            IsGetMaterial = !IsGetMaterial;
            OnPropertyChanged(nameof(IsDeletedMaterial));
        }

        #endregion Material

        #region ConsumedMaterial

        [RelayCommand]
        private async Task SubmitConsumedMaterial()
        {
            bool ok = false;
            try
            {
                if (!IsValidData())
                    throw new InvalidOperationException("Số lượng trong kho nhỏ hơn yêu cầu");
                if (IsAdding)
                {
                    var addConsumedMaterial = await _consumedMaterialServices.AddConsumedmaterial(_mapper.Map<Consumedmaterial>(ModifyConsumedMaterial));
                    if (addConsumedMaterial != null)
                    {
                        var res = ListConsumedMaterialDTO.FirstOrDefault(x => x.Consumedmaterialid == addConsumedMaterial.Consumedmaterialid);
                        if (res != null)
                        {
                            _mapper.Map(addConsumedMaterial, res);
                        }
                        else
                        {
                            ListConsumedMaterialDTO.Add(_mapper.Map<ConsumedMaterialDTO>(addConsumedMaterial));
                        }
                        MyMessageBox.ShowDialog("Thêm chi tiết sử dụng vật liệu cấp thành công");
                        ok = true;
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm chi tiết sử dụng vật liệu thất bại");
                    }
                }
                if (IsUpdating)
                {
                    var res = await _consumedMaterialServices.UpdateConsumedMaterialById(ModifyConsumedMaterial.Consumedmaterialid, _mapper.Map<Consumedmaterial>(ModifyConsumedMaterial));
                    if (res != null)
                    {
                        var updateConsumedMaterialDTO = ListConsumedMaterialDTO.FirstOrDefault(x => x.Consumedmaterialid == res.Consumedmaterialid);
                        if (updateConsumedMaterialDTO != null)
                        {
                            _mapper.Map(res, updateConsumedMaterialDTO);
                            MyMessageBox.ShowDialog("Sửa chi tiết sử dụng vật liệu thành công");
                            ok = true;
                        }
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa nhà cung cấp thất bại");
                    }
                }
                if (ok)
                {
                    var ms = await _materialSupplierServices.GetMaterialsupplierById(ModifyConsumedMaterial.Materialsupplierid);
                    var updateMaterialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == ModifyConsumedMaterial.Materialsupplierid);
                    _mapper.Map(ms, updateMaterialSupplier);
                    ListInventoryDTO = [.. ListInventoryDTO];
                    ListConsumedMaterialDTO = [.. ListConsumedMaterialDTO];
                    ClearValueOfFrom();
                    IsOpenModifyConsumed = false;
                    FilterInventory();
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        private bool IsValidData()
        {
            var materialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == ModifyConsumedMaterial.Materialsupplierid);
            if (IsAdding)
            {
                return materialSupplier?.TotalQuantity - ModifyConsumedMaterial.Quantity >= 0;
            }
            if (IsUpdating)
            {
                return materialSupplier?.TotalQuantity - (ModifyConsumedMaterial.Quantity - currentQuantity) >= 0;
            }
            return false;
        }

        public void ClearValueOfFrom()
        {
            IsAdding = false;
            IsUpdating = false;
            ModifyConsumedMaterial = new();
            currentQuantity = 0;
        }

        [RelayCommand]
        private void OpenModifyConsumed_Add(MaterialSupplierDTO materialSupplier)
        {
            ModifyConsumedMaterial.Materialsupplierid = materialSupplier.Materialsupplierid;

            IsOpenModifyConsumed = true;
            IsAdding = true;
        }

        [RelayCommand]
        private void OpenModifyConsumed_Update(ConsumedMaterialDTO consumedMaterialDTO)
        {
            SelectedMaterial = null;
            currentQuantity = consumedMaterialDTO.Quantity;
            ModifyConsumedMaterial = consumedMaterialDTO.Clone();
            IsOpenModifyConsumed = true;
            IsUpdating = true;
        }

        [RelayCommand]
        private async Task DeleteConsumedMaterial(ConsumedMaterialDTO consumedMaterial)
        {
            try
            {
                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn xoá chi tiết sử dụng vật liệu này không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    bool isDeleted = await _consumedMaterialServices.DeleteConsumedmaterial(consumedMaterial.Consumedmaterialid);
                    if (isDeleted)
                    {
                        var ms = await _materialSupplierServices.GetMaterialsupplierById(consumedMaterial.Materialsupplierid);
                        var updateMaterialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == consumedMaterial.Materialsupplierid);
                        _mapper.Map(ms, updateMaterialSupplier);
                        ListInventoryDTO = [.. ListInventoryDTO];

                        ListConsumedMaterialDTO.Remove(consumedMaterial);
                        MyMessageBox.ShowDialog("Xoá chi tiết sử dụng vật liệu thanh công");
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Xoá chi tiết sử dụng vật liệu thất bại");
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
            FilterInventory();
        }

        [RelayCommand]
        private void CloseModifyConsumed()
        {
            IsOpenModifyConsumed = false;
            ClearValueOfFrom();
        }

        #endregion ConsumedMaterial

        public void Dispose()
        {
            ModifyMaterialVM.ModifyMaterialChanged -= ModifyMaterialVM_ModifyMaterialChanged;
            ModifyMaterialVM.Close -= ModifyMaterialVM_Close;
        }
    }
}