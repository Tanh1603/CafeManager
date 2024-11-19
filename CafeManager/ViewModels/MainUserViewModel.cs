using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Assets.UsersControls;
using CafeManager.WPF.ViewModels.UserViewModel;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainUserViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;

        [ObservableProperty]
        private ObservableObject _currentVM;

        [ObservableProperty]
        private AppUserDTO _userAccount = new();

        [ObservableProperty]
        private bool _IsLeftDrawerOpen;

        private string currentVM = string.Empty;

        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _accountStore = provider.GetRequiredService<AccountStore>();
            CurrentVM = _provider.GetRequiredService<OrderViewModel>();
            LoadAccount();
            currentVM = "OrderFood";
            _accountStore.ChangeAccount += _accountStore_ChangeAccount;
        }

        private void _accountStore_ChangeAccount()
        {
            LoadAccount();
        }

        private void LoadAccount()
        {
            if (_accountStore.Account != null)
            {
                UserAccount = _accountStore.Account;
            }
        }

        [RelayCommand]
        private void ChangeCurrentViewModel(string choice)
        {
            if (currentVM.Equals(choice)) return;

            switch (choice)
            {
                case "OrderFood":
                    CurrentVM = _provider.GetRequiredService<OrderViewModel>();
                    IsLeftDrawerOpen = false;
                    break;

                case "Setting":
                    CurrentVM = _provider.GetRequiredService<SettingAccountViewModel>();
                    IsLeftDrawerOpen = false;
                    break;

                default:
                    break;
            }
            currentVM = choice;
        }

        [RelayCommand]
        private void OpenMenu()
        {
            IsLeftDrawerOpen = true;
        }

        [RelayCommand]
        private void SignOut()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }

        public void Dispose()
        {
            if (_accountStore != null)
            {
                _accountStore.ChangeAccount -= _accountStore_ChangeAccount;
            }
        }
    }
}