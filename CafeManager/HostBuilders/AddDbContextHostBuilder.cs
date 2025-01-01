using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
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
                //string? connectionString = context.Configuration.GetConnectionString("SqlLite");
                //Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlite(connectionString);

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CafeManagerApp.db");
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlite($"Data Source={path}");

                services.AddDbContextFactory<CafeManagerContext>(configureDbContext);
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            });
            return hostBuilder;
        }
    }
}