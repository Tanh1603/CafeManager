using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
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

namespace CafeManager.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private IServiceProvider _provider;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        public MainViewModel(IServiceProvider provider)
        {
            _provider = provider;

            CurrentViewModel = _provider.GetRequiredService<TestImportViewModel>();
        }

        [RelayCommand]
        private void TabSelectionChanged(string str)
        {
            switch (str)
            {
                case "Nhập kho":
                    CurrentViewModel = _provider.GetRequiredService<TestImportViewModel>();
                    break;

                case "Tồn kho":
                    CurrentViewModel = _provider.GetRequiredService<TestInventoryViewModel>();
                    break;

                case "Nhà cung cấp":
                    CurrentViewModel = _provider.GetRequiredService<TestSupplierViewModel>();
                    break;

                default:
                    break;
            }
        }
    }
}