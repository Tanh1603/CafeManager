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
using CafeManager.Core.Services;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainUserViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;
        private CancellationTokenSource? _cts = default;

        [ObservableProperty]
        private ObservableObject? _currentVM;

        [ObservableProperty]
        private AppUserDTO _userAccount = new();

        [ObservableProperty]
        private bool _IsLeftDrawerOpen;

        private string currentVM = string.Empty;


        [ObservableProperty]
        private SettingAccountViewModel _openSettingAccountVM;
        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _accountStore = provider.GetRequiredService<AccountStore>();
            //CurrentVM = _provider.GetRequiredService<OrderViewModel>();
            //ChangeCurrentViewModelCommand.Execute("OrderFood");
            ChangeCurrentViewModel("OrderFood");
            LoadAccount();
            currentVM = "OrderFood";
            OpenSettingAccountVM = _provider.GetRequiredService<SettingAccountViewModel>();
            OpenSettingAccountVM.Close += OpenSettingAccountVM_Close;
            _accountStore.ChangeAccount += _accountStore_ChangeAccount;
        }

        private void OpenSettingAccountVM_Close()
        {
            IsOpenSetting = false;
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

            if (_cts != default)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
            _cts = default;

            try
            {
                _cts?.Token.ThrowIfCancellationRequested();
                CurrentVM = choice switch
                {
                    "OrderFood" => _provider.GetRequiredService<OrderViewModel>(),
                    "Setting" => _provider.GetRequiredService<SettingAccountViewModel>(),
                    _ => throw new InvalidOperationException("Lỗi")
                };

                _cts = new();

                if (CurrentVM is IDataViewModel dataVM)
                {
                    Task.Run(async () =>
                    {
                        await dataVM.LoadData(_cts.Token);
                    }, _cts.Token);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                currentVM = choice;
                IsLeftDrawerOpen = false;
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

        [ObservableProperty]
        private bool _isOpenSetting = false;


        [RelayCommand]
        private void OpenSetting()
        {
            IsOpenSetting = true;

        }


        public void Dispose()
        {
            if (_accountStore != null)
            {
                _accountStore.ChangeAccount -= _accountStore_ChangeAccount;
            }
            if (_cts != default)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = default;
            }
        }
    }
}