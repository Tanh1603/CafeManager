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
                services.AddScoped<MainViewModel>(provider => new MainViewModel(provider));
                services.AddScoped<SupplierViewModel>(provider => new SupplierViewModel(provider));
                services.AddScoped<MainAdminViewModel>(provider => new MainAdminViewModel(provider));
                services.AddScoped<FoodListViewModel>(provider => new FoodListViewModel(provider));
                services.AddScoped<HomeViewModel>(provider => new HomeViewModel(provider));

                services.AddSingleton<MainWindow>(provider => new MainWindow()
                {
                    DataContext = provider.GetRequiredService<MainViewModel>()
                });
            });
            return hostBuilder;
        }
    }
}