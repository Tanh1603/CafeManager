using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class TableViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listTable = [];

        public TableViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            Task.Run(LoadData);
        }

        private async Task LoadData()
        {
            var coffeeTables = await _coffeTableServices.GetListCoffeTable();
            ListTable = [.. _mapper.Map<List<CoffeetableDTO>>(coffeeTables)];
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
                    ListTable.Add(_mapper.Map<CoffeetableDTO>(res));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}