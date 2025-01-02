using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace CafeManager.WPF.HostBuilders
{
    public static class AddDbContextHostBuilder
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, context.Configuration.GetConnectionString("SqlLite"));
                //string connectionString = $"Data Source={path}";

                string? connectionString = context.Configuration.GetConnectionString("SqlLite");
                Action<DbContextOptionsBuilder> configureDbContext = o => o.UseSqlite(connectionString);
                services.AddDbContextFactory<CafeManagerContext>(configureDbContext);
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            });
            return hostBuilder;
        }
    }
}