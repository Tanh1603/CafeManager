using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels.UserViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Windows;

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
        private SettingAccountViewModel? _openSettingAccountVM;

        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _accountStore = provider.GetRequiredService<AccountStore>();
            ChangeCurrentViewModel("OrderFood");
            _accountStore.ChangeAccount += _accountStore_ChangeAccount;
            LoadAccount();
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

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            try
            {
                _cts.Token.ThrowIfCancellationRequested();
                Task.Run(async () =>
                {
                    _cts.Token.ThrowIfCancellationRequested();
                    CurrentVM = choice switch
                    {
                        "OrderFood" => _provider.GetRequiredService<OrderViewModel>(),
                        "Setting" => _provider.GetRequiredService<SettingAccountViewModel>(),
                        "DistributionMaterial" => _provider.GetRequiredService<DistributionMaterialViewModel>(),
                        "IncidentTable" => _provider.GetRequiredService<IncidentTableViewModel>(),
                        _ => throw new Exception("Lỗi")
                    };
                    currentVM = choice;
                    if (CurrentVM is IDataViewModel dataVM)
                    {
                        await dataVM.LoadData(_cts.Token);
                    }
                }, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.Print("Tác vụ thay đổi ViewModel đã bị hủy.");
            }
            catch (Exception ex)
            {
                Debug.Print($"Lỗi khi thay đổi ViewModel: {ex.Message}");
            }
            finally
            {
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
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = default;
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        [ObservableProperty]
        private bool _isOpenSetting = false;

        [RelayCommand]
        private void OpenSetting()
        {
            if (OpenSettingAccountVM != null)
            {
                OpenSettingAccountVM.Close -= OpenSettingAccountVM_Close;
            }
            OpenSettingAccountVM = _provider.GetRequiredService<SettingAccountViewModel>();
            OpenSettingAccountVM.Close += OpenSettingAccountVM_Close;
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
            if (OpenSettingAccountVM != null)
            {
                OpenSettingAccountVM = _provider.GetRequiredService<SettingAccountViewModel>();
                OpenSettingAccountVM.Close -= OpenSettingAccountVM_Close;
            }
        }
    }
}