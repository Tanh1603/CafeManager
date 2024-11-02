using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class TestInventoryViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;

        [ObservableProperty]
        private ObservableCollection<MaterialDetailDTO> _allInventoryList = new();

        [ObservableProperty]
        private ObservableCollection<MaterialDetailDTO> _allUsedMaterialList = new();

        public TestInventoryViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadUsedMaterialData();
            await LoadInventoryData();
        }

        private async Task LoadInventoryData()
        {
            AllInventoryList = new ObservableCollection<MaterialDetailDTO>(await _materialSupplierServices.GetInventoryList() ?? Enumerable.Empty<MaterialDetailDTO>());
        }

        private async Task LoadUsedMaterialData()
        {
            AllUsedMaterialList = new ObservableCollection<MaterialDetailDTO>(
                await _materialSupplierServices.GetConsumedMaterialList() ?? Enumerable.Empty<MaterialDetailDTO>()
                );
        }
    }
}