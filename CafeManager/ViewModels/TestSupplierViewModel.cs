using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class TestSupplierViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;

        [ObservableProperty]
        private ObservableCollection<Supplier>? _allSupplierList;

        public TestSupplierViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            Task.Run(() => LoadData()).Wait();
        }

        private async Task LoadData()
        {
            AllSupplierList = new ObservableCollection<Supplier>(
                await _materialSupplierServices.GetSupplierList()
                );
            return;
        }
    }
}