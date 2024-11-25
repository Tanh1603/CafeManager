using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
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
        private readonly IMapper _mapper;

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
            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            var dbMaterialSupplier = await _materialSupplierServices.GetListMaterialSupplier();
            var dbConsumedMaterial = await _consumedMaterialServices.GetListConsumedMaterial();

            ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbConsumedMaterial)];
            ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbMaterialSupplier)];

            var list = _mapper.Map<List<Consumedmaterial>>(ListConsumedMaterialDTO);
        }
    }
}