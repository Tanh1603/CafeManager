using CafeManager.WPF.Stores;
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
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;

        public RegisterViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = _provider.GetRequiredService<NavigationStore>();
        }

        [RelayCommand]
        private void Register()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }
    }
}