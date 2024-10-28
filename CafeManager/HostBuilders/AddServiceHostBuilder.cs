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
                services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                services.AddScoped<IUnitOfWork, UnitOfWork>(
                        provider => new UnitOfWork(provider.GetRequiredService<CafeManagerContextFactory>().CreateDbContext())
                    );

                services.AddTransient<FoodServices>(provider => new FoodServices(provider));
            });
            return hostBuilder;
        }
    }
}