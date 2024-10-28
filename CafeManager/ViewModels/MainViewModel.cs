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
        private ObservableObject _curentViewModel;

        public MainViewModel(IServiceProvider provider)
        {
            _provider = provider;
            CurentViewModel = _provider.GetRequiredService<FoodViewModel>();
        }
    }
}