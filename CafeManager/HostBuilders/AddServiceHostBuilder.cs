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
                services.AddScoped<FoodServices>();
                services.AddScoped<FoodCategoryServices>();
                services.AddScoped<CoffeTableServices>();
                services.AddScoped<InvoiceServices>();
                services.AddScoped<StaffServices>();

                services.AddScoped<ImportServices>();
                services.AddScoped<ImportDetailServices>();

                services.AddScoped<FileDialogService>();

                services.AddScoped<MaterialSupplierServices>();
                services.AddScoped<AppUserServices>();
                services.AddScoped<ConsumedMaterialServices>();

                services.AddSingleton<EncryptionHelper>(provider => new EncryptionHelper(provider));
                services.AddAutoMapper(typeof(MappingProfile));
            });
            return hostBuilder;
        }
    }
}