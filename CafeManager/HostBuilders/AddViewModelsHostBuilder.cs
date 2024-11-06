using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels;
using CafeManager.WPF.ViewModels.AddViewModel;
using CafeManager.WPF.ViewModels.AdminViewModel;
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
                services.AddScoped<MainAdminViewModel>(provider => new MainAdminViewModel(provider));

                services.AddScoped<AppUserViewModel>(provider => new AppUserViewModel(provider));
                services.AddScoped<FoodViewModel>(provider => new FoodViewModel(provider));
                services.AddScoped<HomeViewModel>(provider => new HomeViewModel(provider));
                services.AddScoped<ImportViewModel>(provider => new ImportViewModel(provider));
                services.AddScoped<InventoryViewModel>(provider => new InventoryViewModel(provider));
                services.AddScoped<InvoiceViewModel>(provider => new InvoiceViewModel(provider));
                services.AddScoped<StaffViewModel>(provider => new StaffViewModel(provider));
                services.AddScoped<SupplierViewModel>(provider => new SupplierViewModel(provider));
                services.AddScoped<TableViewModel>(provider => new TableViewModel(provider));

                services.AddScoped<LoginViewModel>(provider => new LoginViewModel(provider));
                services.AddScoped<RegisterViewModel>(provider => new RegisterViewModel(provider));

                services.AddScoped<AddSuppierViewModel>(provider => new AddSuppierViewModel(provider));
                services.AddScoped<AddUpdateFoodViewModel>(provider => new AddUpdateFoodViewModel(provider));

                services.AddSingleton<NavigationStore>(provider => new NavigationStore(provider));
                services.AddSingleton<MainWindow>(provider => new MainWindow()
                {
                    DataContext = provider.GetRequiredService<MainViewModel>()
                });
            });
            return hostBuilder;
        }
    }
}