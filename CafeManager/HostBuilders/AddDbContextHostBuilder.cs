using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
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
    public static class AddDbContextHostBuilder
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                string? connectionString = context.Configuration.GetConnectionString("postgreSql");
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseNpgsql(connectionString);
                //services.AddDbContext<CafeManagerContext>(configureDbContext);
                //services.AddSingleton<CafeManagerContextFactory>(new CafeManagerContextFactory(configureDbContext));

                services.AddDbContextFactory<CafeManagerContext>(configureDbContext);

                services.AddScoped<IUnitOfWork, UnitOfWork>(
                                provider => new UnitOfWork(provider.GetRequiredService<IDbContextFactory<CafeManagerContext>>())
                    );
            });
            return hostBuilder;
        }
    }
}