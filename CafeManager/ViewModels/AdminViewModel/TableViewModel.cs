using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class TableViewModel : ObservableObject, IDataViewModel
    {
        private readonly CoffeTableServices _coffeTableServices;
        private readonly IMapper _mapper;

        public ObservableCollection<int> TypeTable { get; } = [2, 4, 6, 8, 10];

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listTable = [];

        [ObservableProperty]
        private CoffeetableDTO _table = new();

        private bool isAdding = false;

        private bool isUpdating = false;

        [ObservableProperty]
        private bool _isOpenModifyTable;

        public TableViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _mapper = provider.GetRequiredService<IMapper>();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var coffeeTables = await _coffeTableServices.GetListCoffeTable(token);
                ListTable = [.. _mapper.Map<List<CoffeetableDTO>>(coffeeTables)];
            }
            catch (OperationCanceledException)
            {
            }
        }

        [RelayCommand]
        private void OpenAddTable()
        {
            IsOpenModifyTable = true;
            isAdding = true;
        }

        [RelayCommand]
        private void OpenUpdateTable(CoffeetableDTO dTO)
        {
            IsOpenModifyTable = true;
            isUpdating = true;
            Table = dTO.Clone();
        }

        [RelayCommand]
        private void CloseModifyTable()
        {
            IsOpenModifyTable = false;
            Table = new();
            isUpdating = false;
            isAdding = false;
        }

        [RelayCommand]
        private async Task DeleteTable(CoffeetableDTO dTO)
        {
            var res = ListTable.FirstOrDefault(x => x.Coffeetableid == dTO.Coffeetableid);
            if (res != null)
            {
                res.Isdeleted = true;
                var delete = await _coffeTableServices.DeleteCoffeeTable(dTO.Coffeetableid);
                if (delete)
                {
                    MyMessageBox.ShowDialog("Ẩn bàn thành công");
                }
            }
        }

        [RelayCommand]
        private async Task ShowTable(CoffeetableDTO dTO)
        {
            var res = ListTable.FirstOrDefault(x => x.Coffeetableid == dTO.Coffeetableid);
            if (res != null)
            {
                res.Isdeleted = false;
                var show = await _coffeTableServices.UpdateCoffeeTable(_mapper.Map<Coffeetable>(res));
                if (show != null)
                {
                    MyMessageBox.ShowDialog("Hiện bàn thành công");
                }
            }
        }

        [RelayCommand]
        private async Task ModifyTable()
        {
            try
            {
                if (isAdding)
                {
                    var tmp = _mapper.Map<Coffeetable>(Table);
                    var addTable = await _coffeTableServices.AddCoffeTable(tmp);
                    if (addTable != null)
                    {
                        MyMessageBox.ShowDialog("Thêm bàn thành công");
                        ListTable.Add(_mapper.Map<CoffeetableDTO>(addTable));
                    }
                }
                if (isUpdating)
                {
                    var updateTable = await _coffeTableServices.UpdateCoffeeTable(_mapper.Map<Coffeetable>(Table));
                    if (updateTable != null)
                    {
                        var tmp = ListTable.FirstOrDefault(x => x.Coffeetableid == Table.Coffeetableid);
                        if (tmp != null)
                        {
                            _mapper.Map(updateTable, tmp);
                            MyMessageBox.ShowDialog("Sửa bàn thành công");
                        }
                    }
                }

                Table = new();
                isUpdating = false;
                isAdding = false;
                IsOpenModifyTable = false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}