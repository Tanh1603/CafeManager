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

                services.AddTransient<AppUserViewModel>(provider => new AppUserViewModel(provider.CreateScope()));
                services.AddTransient<FoodViewModel>(provider => new FoodViewModel(provider.CreateScope()));
                services.AddTransient<FoodCategoryViewModel>(provider => new FoodCategoryViewModel(provider.CreateScope()));
                services.AddTransient<HomeViewModel>();
                services.AddTransient<MaterialViewModel>(provider => new MaterialViewModel(provider.CreateScope()));
                services.AddTransient<ImportViewModel>(provider => new ImportViewModel(provider.CreateScope()));
                services.AddTransient<InventoryViewModel>(provider => new InventoryViewModel(provider.CreateScope()));
                services.AddTransient<InvoiceViewModel>(provider => new InvoiceViewModel(provider.CreateScope()));
                services.AddTransient<StaffViewModel>(provider => new StaffViewModel(provider.CreateScope()));
                services.AddTransient<SupplierViewModel>(provider => new SupplierViewModel(provider.CreateScope()));
                services.AddTransient<TableViewModel>(provider => new TableViewModel(provider.CreateScope()));
                

                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();
                services.AddTransient<SettingAccountViewModel>(provider => new(provider.CreateScope()));
                services.AddTransient<OrderViewModel>(provider => new OrderViewModel(provider.CreateScope()));
                services.AddTransient<DistributionMaterialViewModel>(provider => new DistributionMaterialViewModel(provider.CreateScope()));
                services.AddTransient<RequestMaterialViewModel>(provider => new RequestMaterialViewModel(provider.CreateScope()));
                services.AddTransient<SelectInventoryViewModel>(provider => new SelectInventoryViewModel(provider.CreateScope()));

                services.AddTransient<AddSuppierViewModel>();
                services.AddTransient<AddMaterialViewModel>();
                services.AddTransient<ModifyFoodViewModel>();
                services.AddTransient<AddImportViewModel>();
                services.AddTransient<ModifyStaffViewModel>();
                services.AddTransient<ModifyInvoiceViewModel>();
                services.AddTransient<UpdateAppUserViewModel>(provider => new(provider.CreateScope()));

                services.AddSingleton<NavigationStore>();
                services.AddSingleton<AccountStore>();
                services.AddSingleton<MainWindow>(provider => new MainWindow()
                {
                    DataContext = provider.GetRequiredService<MainViewModel>()
                });
            });
            return hostBuilder;
        }
    }
}