using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using CafeManager.WPF.HostBuilders;
using CafeManager.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace CafeManager.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] arg = null)
        {
            return Host.CreateDefaultBuilder(arg).AddConfiguration()
                                                .AddDbContext()
                                                .AddServiceRepository()
                                                .AddViewModels();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _host.Start();
            Window window = new MainWindow()
            {
                DataContext = _host.Services.GetRequiredService<MainViewModel>()
            };
            window.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}