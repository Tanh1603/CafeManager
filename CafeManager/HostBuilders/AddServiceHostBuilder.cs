using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CafeManager.WPF.HostBuilders
{
    public static class AddServiceHostBuilder
    {
        public static IHostBuilder AddServiceRepository(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddScoped<FoodServices>(provider => new FoodServices(provider));
                services.AddScoped<FoodCategoryServices>(provider => new FoodCategoryServices(provider));
                services.AddScoped<FoodServices>(provider => new FoodServices(provider));
                services.AddScoped<CoffeTableServices>(provider => new CoffeTableServices(provider));
                services.AddScoped<InvoiceServices>(provider => new InvoiceServices(provider));
                services.AddScoped<StaffServices>(provider => new StaffServices(provider));

                services.AddScoped<ImportServices>(provider => new ImportServices(provider));
                services.AddScoped<ImportDetailServices>(provider => new ImportDetailServices(provider));

                services.AddScoped<FileDialogService>();

                services.AddScoped<MaterialSupplierServices>(provider => new MaterialSupplierServices(provider));
                services.AddScoped<AppUserServices>(provider => new AppUserServices(provider));
                services.AddScoped<ConsumedMaterialServices>(provider => new ConsumedMaterialServices(provider));

                services.AddSingleton<EncryptionHelper>(provider => new EncryptionHelper(provider));
                services.AddAutoMapper(typeof(MappingProfile));
            });
            return hostBuilder;
        }
    }
}