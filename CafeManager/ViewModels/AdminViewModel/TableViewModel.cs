using CafeManager.Core.Data;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class TableViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly CoffeTableServices _coffeTableServices;

        [ObservableProperty]
        private ObservableCollection<Coffeetable> _listTable = new();

        public TableViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _ = LoadData();
        }

        private async Task LoadData()
        {
            List<Coffeetable> list = new(await _coffeTableServices.GetListCoffeTable());
            for (int i = 0; i < list.Count; i++)
            {
                ListTable.Add(new Coffeetable()
                {
                    Tablename = $"Bàn {i + 1}",
                    Statustable = list[i].Statustable,
                    Notes = list[i].Notes,
                    Coffeetableid = list[i].Coffeetableid,
                    Invoices = list[i].Invoices,
                });
            }
        }

        [RelayCommand]
        private async Task AddTable()
        {
            try
            {
                var res = await _coffeTableServices.AddCoffeTable(new Coffeetable());
                if (res != null)
                {
                    MyMessageBox.Show("Thêm bàn thành công");
                    ListTable.Add(new Coffeetable()
                    {
                        Tablename = $"Bàn {ListTable.Count + 1}"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}