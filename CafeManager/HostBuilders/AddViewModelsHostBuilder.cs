using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels;
using CafeManager.WPF.ViewModels.AddViewModel;
using CafeManager.WPF.ViewModels.AdminViewModel;
using CafeManager.WPF.ViewModels.UserViewModel;
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
                services.AddTransient<MainViewModel>();
                services.AddTransient<MainAdminViewModel>();
                services.AddTransient<MainUserViewModel>();

                services.AddTransient<AppUserViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<FoodViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<FoodCategoryViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<HomeViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<MaterialViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<ImportViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<InventoryViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<InvoiceViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<StaffViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<SupplierViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<TableViewModel>(provider => new(provider.CreateScope()));

                services.AddTransient<LoginViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<RegisterViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<SettingAccountViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<OrderViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<DistributionMaterialViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<RequestMaterialViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<SelectInventoryViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<IncidentTableViewModel>(provider => new(provider.CreateScope()));

                services.AddTransient<AddSuppierViewModel>();
                services.AddTransient<AddMaterialViewModel>();
                services.AddTransient<ModifyFoodViewModel>();
                services.AddTransient<AddImportViewModel>();
                services.AddTransient<ModifyStaffViewModel>();
                services.AddTransient<ModifyInvoiceViewModel>();
                services.AddTransient<UpdateAppUserViewModel>(provider => new(provider.CreateScope()));

                services.AddSingleton<NavigationStore>();
                services.AddSingleton<AccountStore>();
                services.AddSingleton<WaitWindow>();
                services.AddSingleton<MainWindow>(provider => new()
                {
                    DataContext = provider.GetRequiredService<MainViewModel>()
                });
            });
            return hostBuilder;
        }
    }
}