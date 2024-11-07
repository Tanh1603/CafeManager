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
    public partial class MainUserViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;

        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
        }

        [RelayCommand]
        private void SignOut()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }
    }
}