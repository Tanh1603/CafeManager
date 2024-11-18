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
    public partial class MainUserViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        private ObservableObject _currentVM;

        [ObservableProperty]
        private BitmapImage? _imageAccount = new();

        [ObservableProperty]
        private string _displayname = string.Empty;

        [ObservableProperty]
        private string _role = string.Empty;

        [ObservableProperty]
        private bool _IsLeftDrawerOpen;

        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _accountStore = provider.GetRequiredService<AccountStore>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            CurrentVM = _provider.GetRequiredService<OrderViewModel>();

            ImageAccount = _fileDialogService.Base64ToBitmapImage(_accountStore.Account.Avatar);
            Displayname = _accountStore.Account.Displayname;
            Role = _accountStore.Account.Role == 1 ? "Admin" : "User";
        }

        [RelayCommand]
        private void ChangeCurrentViewModel(string choice)
        {
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
    }
}