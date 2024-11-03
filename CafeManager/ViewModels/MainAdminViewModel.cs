using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainAdminViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        public MainAdminViewModel(IServiceProvider provider)
        {
            _provider = provider;
            CurrentViewModel = _provider.GetRequiredService<HomeViewModel>();
        }

        [RelayCommand]
        private void ChangeCurrentViewModel(string viewModel)
        {
            switch (viewModel)
            {
                case "Home":
                    CurrentViewModel = _provider.GetRequiredService<HomeViewModel>();
                    break;

                case "Food":
                    CurrentViewModel = _provider.GetRequiredService<FoodListViewModel>();
                    break;

                case "Supplier":
                    CurrentViewModel = _provider.GetRequiredService<SupplierViewModel>();
                    break;

                default:
                    break;
            }
        }
    }
}