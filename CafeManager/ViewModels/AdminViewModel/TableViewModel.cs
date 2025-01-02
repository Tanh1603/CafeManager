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

        [ObservableProperty]
        private bool _isLoading;

        public ObservableCollection<int> TypeTable { get; } = [2, 4, 6, 8, 10];

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listTable = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSubmit))]
        [NotifyCanExecuteChangedFor(nameof(ModifyTableCommand))]
        private CoffeetableDTO _table;

        private bool isAdding = false;

        private bool isUpdating = false;

        [ObservableProperty]
        private bool _isOpenModifyTable;

        public TableViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            Table = new CoffeetableDTO();
            Table.ErrorsChanged += Table_ErrorsChanged;
        }

        private void Table_ErrorsChanged(object? sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanSubmit));
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var coffeeTables = await _coffeTableServices.GetListCoffeTable(token);
                ListTable = [.. _mapper.Map<List<CoffeetableDTO>>(coffeeTables)];
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsLoading = false;
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
                IsLoading = true;
                res.Isdeleted = true;
                var delete = await _coffeTableServices.DeleteCoffeeTable(dTO.Coffeetableid);
                if (delete)
                {
                    IsLoading = false;
                    MyMessageBox.ShowDialog("Ẩn bàn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
        }

        [RelayCommand]
        private async Task ShowTable(CoffeetableDTO dTO)
        {
            var res = ListTable.FirstOrDefault(x => x.Coffeetableid == dTO.Coffeetableid);
            if (res != null)
            {
                IsLoading = true;
                res.Isdeleted = false;
                var show = await _coffeTableServices.UpdateCoffeeTable(_mapper.Map<Coffeetable>(res));
                if (show != null)
                {
                    IsLoading = false;
                    MyMessageBox.ShowDialog("Hiện bàn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
        }

        public bool CanSubmit => !Table.HasErrors;

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task ModifyTable()
        {
            try
            {
                IsLoading = true;
                if (Table.Seatingcapacity <= 0)
                {
                    MyMessageBox.ShowDialog("Vui lòng chọn số chổ ngồi", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
                    return;
                }
                if (isAdding)
                {
                    var tmp = _mapper.Map<Coffeetable>(Table);
                    var addTable = await _coffeTableServices.AddCoffeTable(tmp);
                    if (addTable != null)
                    {
                        IsLoading = false;
                        MyMessageBox.ShowDialog("Thêm bàn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
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
                            IsLoading = false;
                            _mapper.Map(updateTable, tmp);
                            MyMessageBox.ShowDialog("Sửa bàn thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
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
            finally
            {
                IsLoading = false;
            }
        }
    }
}