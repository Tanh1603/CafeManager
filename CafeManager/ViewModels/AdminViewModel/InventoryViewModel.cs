using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;

        //[ObservableProperty]
        //private ObservableCollection<MaterialSupplierDTO> _listMaterialSupplierDTO = [];

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listInventoryDTO = [];

        public InventoryViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();

            //Task.Run(LoadData);
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var dbMaterialSupplier = await _materialSupplierServices.GetListMaterialSupplier();
            var dbConsumedMaterial = await _consumedMaterialServices.GetListConsumedMaterial();

            ListConsumedMaterialDTO = [.. dbConsumedMaterial.Select(x => x.ToDTO(true))];
            ListInventoryDTO = [.. dbMaterialSupplier.Select(x => x.ToDTO(true))];
        }
    }
}