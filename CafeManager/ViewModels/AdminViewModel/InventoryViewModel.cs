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

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isOpenAddMaterial = false;

        [ObservableProperty]
        private bool _isPopupOpen = false;

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private bool _isOpenPopupMVM = false;

        [ObservableProperty]
        private bool _isGetMaterial = true;
        public bool IsDeletedMaterial => !IsGetMaterial;

        partial void OnIsGetMaterialChanged(bool value)
        {
            OnPropertyChanged(nameof(IsDeletedMaterial)); // Báo rằng IsDeletedMaterial cũng đã thay đổi
        }

        [ObservableProperty]
        private ConsumedMaterialDTO _modifyConsumedMaterial = new();

        private decimal _currentQuantity;

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialDTO> _listDeletedMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listInventoryDTO = [];

        [ObservableProperty]
        private AddMaterialViewModel _modifyMaterialVM;

        [ObservableProperty]
        private MaterialDTO _selectedMaterial;

        public InventoryViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();

            ModifyMaterialVM = _provider.GetRequiredService<AddMaterialViewModel>();
            ModifyMaterialVM.ModifyMaterialChanged += ModifyMaterialVM_ModifyMaterialChanged;
            ModifyMaterialVM.Close += ModifyMaterialVM_Close;

            _mapper = provider.GetRequiredService<IMapper>();
            //Task.Run(LoadData);
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var dbMaterialSupplier = (await _materialSupplierServices.GetListMaterialSupplier()).ToList().Where(x => x.Isdeleted == false);
            var dbConsumedMaterial = (await _consumedMaterialServices.GetListConsumedMaterial()).ToList().Where(x => x.Isdeleted == false);
            var dbMaterial = await _materialSupplierServices.GetListMaterial();

            var listExistedMaterial = dbMaterial.ToList().Where(x => x.Isdeleted == false);
            var listDeletedMaterial = dbMaterial.ToList().Where(x => x.Isdeleted == true);

            ListMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(listExistedMaterial)];
            ListDeletedMaterialDTO = [.. _mapper.Map<List<MaterialDTO>>(listDeletedMaterial)];
            ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbConsumedMaterial)];
            ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbMaterialSupplier)];

            var list = _mapper.Map<List<Consumedmaterial>>(ListConsumedMaterialDTO);
        }

        #region Material
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
                            MyMessageBox.ShowDialog("Sửa nhà cung cấp thành công");
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
            IsOpenAddMaterial = true;
            ModifyMaterialVM.IsAdding = true;
            ModifyMaterialVM.ModifyMaterial = new();
        }

        [RelayCommand]
        private void OpenUpdateMaterial()
        {
            if (SelectedMaterial == null)
            {
                MyMessageBox.ShowDialog("Hãy chọn vật tư", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
                return;
            }    
            IsOpenAddMaterial = true;
            ModifyMaterialVM.IsUpdating = true;
            ModifyMaterialVM.ReceiveMaterialDTO(SelectedMaterial);
        }

        [RelayCommand]
        private async Task DeleteMaterial()
        {
            try
            {
                if (SelectedMaterial == null)
                    throw new InvalidOperationException("Hãy chọn vật tư");

                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn ẩn vật tư không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    bool isDeleted = await _materialSupplierServices.DeleteMaterial(SelectedMaterial.Materialid);
                    if (isDeleted)
                    {
                        ListMaterialDTO.Remove(SelectedMaterial);
                        MyMessageBox.ShowDialog("Ẩn vật tư thanh công");
                        SelectedMaterial = new();
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
                if(SelectedMaterial == null)
                    throw new InvalidOperationException("Hãy chọn vật tư");

                string messageBox = MyMessageBox.ShowDialog("Bạn có muốn hiển thị vật tư không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (messageBox.Equals("1"))
                {
                    SelectedMaterial.Isdeleted = false;
                    var res = await _materialSupplierServices.UpdateMaterialById(SelectedMaterial.Materialid, _mapper.Map<Material>(SelectedMaterial));
                    if (res != null)
                    {
                        ListMaterialDTO.Add(SelectedMaterial);
                        ListDeletedMaterialDTO.Remove(SelectedMaterial);
                        MyMessageBox.ShowDialog("Hiển thị vật tư thanh công");
                        SelectedMaterial = new();
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Hiển thị vật tư thất bại");
                    }
                }
                ClearValueOfPopupBox();
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
        }
        #endregion

        [RelayCommand]
        private async void SubmitConsumedMaterial()
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
                        MyMessageBox.ShowDialog("Thêm chi tiết sử dụng vật tư cấp thành công");
                        IsPopupOpen = false;
                        ok = true;
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm chi tiết sử dụng vật tư thất bại");
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
                            MyMessageBox.ShowDialog("Sửa nhà cung cấp thành công");
                            IsPopupOpen = false;
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
                    var updateMaterialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == ModifyConsumedMaterial.Materialsupplierid);
                    if (updateMaterialSupplier != null)
                    {
                        updateMaterialSupplier.TotalQuantity -= ModifyConsumedMaterial.Quantity;
                    }
                    ClearValueOfPopupBox();
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
                return materialSupplier.TotalQuantity - ModifyConsumedMaterial.Quantity > 0;
            }    
            if(IsUpdating)
            {
                return materialSupplier.TotalQuantity - (ModifyConsumedMaterial.Quantity - _currentQuantity) > 0;
            }    
            return false;
        }


        public void ClearValueOfPopupBox()
        {
            IsAdding = false;
            IsUpdating = false;
            ModifyConsumedMaterial = new();
            _currentQuantity = 0;
        }

        [RelayCommand]
        private void OpenPopup_Add(MaterialSupplierDTO materialSupplier)
        {
            ModifyConsumedMaterial.Materialsupplierid = materialSupplier.Materialsupplierid;

            IsPopupOpen = true;
            IsAdding = true;
        }
        [RelayCommand]
        private void OpenPopup_Update(ConsumedMaterialDTO consumedMaterialDTO)
        {
            _currentQuantity = consumedMaterialDTO.Quantity;
            ModifyConsumedMaterial = consumedMaterialDTO.Clone();
            IsPopupOpen = true;
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
                        var updateMaterialSupplier = ListInventoryDTO.FirstOrDefault(x => x.Materialsupplierid == consumedMaterial.Materialsupplierid);
                        if (updateMaterialSupplier != null)
                        {
                            updateMaterialSupplier.TotalQuantity += consumedMaterial.Quantity;
                        }
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
        }

        [RelayCommand]
        private void OpenPopupMaterialView()
        {
            IsOpenPopupMVM = true;
        }

        [RelayCommand]
        private void ClosePopup()
        {
            IsPopupOpen = false;
            ClearValueOfPopupBox();
        }

        public void Dispose()
        {
            ModifyMaterialVM.ModifyMaterialChanged -= ModifyMaterialVM_ModifyMaterialChanged;
            ModifyMaterialVM.Close -= ModifyMaterialVM_Close;
        }
    }
}