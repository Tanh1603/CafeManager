using CafeManager.Infrastructure.Models;
using CafeManager.WPF.HostBuilders;
using CafeManager.WPF.MessageBox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        public static IHostBuilder CreateHostBuilder(string[]? arg = null)
        {
            return Host.CreateDefaultBuilder(arg).AddConfiguration()
                                                .AddDbContext()
                                                .AddServiceRepository()
                                                .AddViewModels();
        }

        private void InitialLoading()
        {
            using var dbContext = _host.Services.GetRequiredService<IDbContextFactory<CafeManagerContext>>().CreateDbContext();
            dbContext.Database.EnsureCreated();
            var db = dbContext.Appusers.FirstOrDefault();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                _host.Start();
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Current.MainWindow = _host.Services.GetRequiredService<WaitWindow>();
                Current.MainWindow.ShowInTaskbar = false;
                Current.MainWindow.Show();

                await Task.Run(InitialLoading);

                Current.MainWindow.Close();
                Current.MainWindow = _host.Services.GetRequiredService<MainWindow>();
                Current.MainWindow.Show();
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                string res = MyMessageBox.ShowDialog(ex.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                if (res == "1")
                {
                    Current.Shutdown();
                }
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}