using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AdminViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class RequestMaterialViewModel : ObservableObject, IDisposable
    {
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _consumedMaterialDTOs = [];

        public event Action OpenSelectInventoryView;

        public event Action<List<ConsumedMaterialDTO>> Submit;

        public RequestMaterialViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();
            _mapper = provider.GetRequiredService<IMapper>();
        }

        public void ClearValue()
        {
            ConsumedMaterialDTOs.Clear();
        }

        [RelayCommand]
        private void OpenSelectInventory()
        {
            OpenSelectInventoryView?.Invoke();
        }

        public void AddConsumedMaterial(MaterialSupplierDTO inventory)
        {
            var res = ConsumedMaterialDTOs.FirstOrDefault(x => x.Materialsupplierid == inventory.Materialsupplierid);
            if (res != null)
            {
                MyMessageBox.ShowDialog("Bạn đã chọn vật liệu này!", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            else
            {
                ConsumedMaterialDTO addConsumedMaterial = new()
                {
                    Materialsupplierid = inventory.Materialsupplierid,
                    Quantity = 0,
                    Materialsupplier = inventory,
                    Usagedate = DateOnly.FromDateTime(DateTime.Now)
                };
                ConsumedMaterialDTOs.Add(addConsumedMaterial);
            }
        }

        [RelayCommand]
        private void RemoveSelectedInventory(ConsumedMaterialDTO consuming)
        {
            try
            {
                var res = MyMessageBox.ShowDialog("Bạn có muốn huỷ bỏ yêu cầu này không?", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Question);
                if (res.Equals("1"))
                {
                    ConsumedMaterialDTOs.Remove(consuming);
                    MyMessageBox.ShowDialog("Huỷ bỏ yêu cầu thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.ShowDialog(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        [RelayCommand]
        private void SubmitRequestMaterial()
        {
            try
            {
                CheckValidData();
                Submit?.Invoke(ConsumedMaterialDTOs.ToList());
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.ShowDialog(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        private void CheckValidData()
        {
            foreach (var consumed in ConsumedMaterialDTOs)
            {
                if (consumed.Materialsupplier.TotalQuantity < consumed.Quantity)
                    throw new InvalidOperationException($"Lỗi: {consumed.Materialsupplier.Material.Materialname} không đủ số lượng");
                else if (consumed.Quantity == 0)
                    throw new InvalidOperationException($"Bạn chưa nhập số lượng của {consumed.Materialsupplier.Material.Materialname}");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}