using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using CafeManager.WPF.HostBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
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
            return Host.CreateDefaultBuilder(arg).AddConfiguration().AddDbContext().AddServiceRepository();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _host.Start();
            CafeManagerContextFactory cafeManagerContextFactory = _host.Services.GetRequiredService<CafeManagerContextFactory>();
            //Task.Run(() =>
            //{
            //    List<Foodcategory> test = new List<Foodcategory>(_host.Services.GetRequiredService<IRepository<Foodcategory>>().SearchAndSortAsync(
            //            x => true, x => x.Foodcategoryid, false, 2, 3
            //        ).GetAwaiter().GetResult());
            //    foreach (Foodcategory user in test)
            //    {
            //        Console.WriteLine(user.Displayname);
            //    }
            //}).Wait();
            Window window = new MainWindow();
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