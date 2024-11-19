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
        private ObservableCollection<object> _listTable = new();

        public TableViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var coffeeTables = await _coffeTableServices.GetListCoffeTable();
            foreach (var x in coffeeTables)
            {
                ListTable.Add(new
                {
                    Tablename = $"Bàn {x.Tablenumber}",
                    Statustable = x.Statustable,
                    Notes = x.Notes,
                    Coffeetableid = x.Coffeetableid,
                    Invoices = x.Invoices
                });
            }
        }

        [RelayCommand]
        private async Task AddTable()
        {
            try
            {
                var res = await _coffeTableServices.AddCoffeTable(new Coffeetable()
                {
                    Tablenumber = ListTable.Count + 1,
                });
                if (res != null)
                {
                    MyMessageBox.ShowDialog("Thêm bàn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    ListTable.Add(new
                    {
                        Tablename = $"Bàn {ListTable.Count + 1}",
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