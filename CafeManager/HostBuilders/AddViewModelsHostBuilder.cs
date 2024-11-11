using CafeManager.WPF.Services;
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
                services.AddTransient<MainViewModel>(provider => new MainViewModel(provider));
                services.AddTransient<MainAdminViewModel>(provider => new MainAdminViewModel(provider));
                services.AddTransient<MainUserViewModel>(provider => new MainUserViewModel(provider));

                services.AddTransient<AppUserViewModel>(provider => new AppUserViewModel(provider));
                services.AddTransient<FoodViewModel>(provider => new FoodViewModel(provider));
                services.AddTransient<HomeViewModel>(provider => new HomeViewModel(provider));
                services.AddTransient<ImportViewModel>(provider => new ImportViewModel(provider));
                services.AddTransient<InventoryViewModel>(provider => new InventoryViewModel(provider));
                services.AddTransient<InvoiceViewModel>(provider => new InvoiceViewModel(provider));
                services.AddScoped<StaffViewModel>(provider => new StaffViewModel(provider));
                services.AddTransient<SupplierViewModel>(provider => new SupplierViewModel(provider));
                services.AddTransient<TableViewModel>(provider => new TableViewModel(provider));

                services.AddTransient<LoginViewModel>(provider => new LoginViewModel(provider));
                services.AddTransient<RegisterViewModel>(provider => new RegisterViewModel(provider));

                services.AddTransient<AddSuppierViewModel>(provider => new AddSuppierViewModel(provider));
                services.AddTransient<AddMaterialViewModel>(provider => new AddMaterialViewModel(provider));
                services.AddTransient<AddUpdateFoodViewModel>(provider => new AddUpdateFoodViewModel(provider));
                services.AddTransient<AddImportViewModel>(provider => new AddImportViewModel(provider));

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