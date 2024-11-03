using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class TestDb : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly InvoiceServices _invoiceServices;
        private readonly ImportServices _importServices;
        private readonly ImportDetailServices _importDetailServices;
        private readonly MaterialSupplierServices _materialSupplierServices;

        public TestDb(IServiceProvider provider)
        {
            _provider = provider;
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();
            _importServices = provider.GetRequiredService<ImportServices>();
            _importDetailServices = provider.GetRequiredService<ImportDetailServices>();
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();

            Task.Run(() => UseDb());
        }

        private async Task UseDb()
        {
        }
    }
}