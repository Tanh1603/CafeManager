using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using CafeManager.Infrastructure.Repositories;
using CafeManager.WPF.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                services.AddScoped<ImportServices>(provider => new ImportServices(provider));
                services.AddScoped<ImportDetailServices>(provider => new ImportDetailServices(provider));

                services.AddScoped<FileDialogService>();

                services.AddScoped<MaterialSupplierServices>(provider => new MaterialSupplierServices(provider));
                services.AddScoped<AppUserServices>(provider => new AppUserServices(provider));
            });
            return hostBuilder;
        }
    }
}