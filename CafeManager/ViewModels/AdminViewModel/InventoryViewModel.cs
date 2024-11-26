using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
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
        private bool _isPopupOpen = false;

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private ConsumedMaterialDTO _modifyConsumedMaterial = new();

        private decimal _currentQuantity;

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listInventoryDTO = [];

        public InventoryViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            //Task.Run(LoadData);
            _ = LoadData();
        }

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

        private async Task LoadData()
        {
            var dbMaterialSupplier = await _materialSupplierServices.GetListMaterialSupplier();
            var dbConsumedMaterial = await _consumedMaterialServices.GetListConsumedMaterial();

            ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbConsumedMaterial)];
            ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbMaterialSupplier)];

            var list = _mapper.Map<List<Consumedmaterial>>(ListConsumedMaterialDTO);
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
        private void ClosePopup()
        {
            IsPopupOpen = false;
            ClearValueOfPopupBox();
        }
    }
}