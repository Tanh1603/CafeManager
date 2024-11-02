using CafeManager.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilder
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<MainViewModel>(provider => new MainViewModel(provider));
                services.AddTransient<TestViewModel>(provider => new TestViewModel(provider));
                services.AddTransient<TestDb>(provider => new TestDb(provider));

                services.AddTransient<TestImportViewModel>(provider => new TestImportViewModel(provider));
                services.AddTransient<TestInventoryViewModel>(provider => new TestInventoryViewModel(provider));
                services.AddTransient<TestSupplierViewModel>(provider => new TestSupplierViewModel(provider));
            });
            return hostBuilder;
        }
    }
}