using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels.UserViewModel
{
    public partial class IncidentTableViewModel : ObservableObject, IDataViewModel
    {
        private readonly CoffeTableServices _coffeetableServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isOpenDialog;

        [ObservableProperty]
        private ObservableCollection<CoffeetableDTO> _listCoffeeTable = [];

        [ObservableProperty]
        private CoffeetableDTO _modifyCoffeTable = new();

        public IncidentTableViewModel(IServiceScope serviceScope)
        {
            var provider = serviceScope.ServiceProvider;
            _coffeetableServices = provider.GetRequiredService<CoffeTableServices>();
            _mapper = provider.GetRequiredService<IMapper>();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                var dbListCoffeeTable = (await _coffeetableServices.GetListCoffeTable(token)).Where(x => x.Isdeleted == false);
                ListCoffeeTable = [.. _mapper.Map<List<CoffeetableDTO>>(dbListCoffeeTable)];
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("LoadData của IncidentViewModel bị hủy");
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
        private void OpenDiaglogHost(CoffeetableDTO dTO)
        {
            IsOpenDialog = true;
            ModifyCoffeTable = _mapper.Map<CoffeetableDTO>(dTO);
        }

        [RelayCommand]
        private void CloseDiaglogHost()
        {
            IsOpenDialog = false;
        }

        [RelayCommand]
        private async Task Submit()
        {
            try
            {
                IsLoading = true;
                var update = await _coffeetableServices.UpdateCoffeeTable(_mapper.Map<Coffeetable>(ModifyCoffeTable));
                if (update != null)
                {
                    _mapper.Map(update, ListCoffeeTable.FirstOrDefault(x => x.Coffeetableid == ModifyCoffeTable.Coffeetableid));
                    IsLoading = false;
                    MyMessageBox.Show("Báo cáo sự cố thành công");
                    IsOpenDialog = false;
                }
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
    }
}