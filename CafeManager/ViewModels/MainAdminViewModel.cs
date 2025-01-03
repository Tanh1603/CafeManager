using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels.AdminViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Web;
using System.Windows;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainAdminViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;
        private CancellationTokenSource? _cts = default;

        [ObservableProperty]
        private ObservableObject? _currentViewModel;

        [ObservableProperty]
        private SettingAccountViewModel? _openSettingAccountVM;

        [ObservableProperty]
        private AppUserDTO _adminAccount = new();

        private string currentVM = string.Empty;

        [ObservableProperty]
        private bool _IsLeftDrawerOpen;

        public MainAdminViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = _provider.GetRequiredService<NavigationStore>();
            _accountStore = provider.GetRequiredService<AccountStore>();
            ChangeCurrentViewModel("Home");
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

        [RelayCommand]
        private void ChangeCurrentViewModel(string viewModel)
        {
            if (viewModel.Equals(currentVM)) return;

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            try
            {
                _cts.Token.ThrowIfCancellationRequested();
                Task.Run(async () =>
               {
                   CurrentViewModel = viewModel switch
                   {
                       "Home" => _provider.GetRequiredService<HomeViewModel>(),
                       "Food" => _provider.GetRequiredService<FoodViewModel>(),
                       "Invoice" => _provider.GetRequiredService<InvoiceViewModel>(),
                       "Table" => _provider.GetRequiredService<TableViewModel>(),
                       "Import" => _provider.GetRequiredService<ImportViewModel>(),
                       "Inventory" => _provider.GetRequiredService<InventoryViewModel>(),
                       "Supplier" => _provider.GetRequiredService<SupplierViewModel>(),
                       "Staff" => _provider.GetRequiredService<StaffViewModel>(),
                       "AppUser" => _provider.GetRequiredService<AppUserViewModel>(),
                       "Setting" => _provider.GetRequiredService<SettingAccountViewModel>(),
                       _ => throw new Exception("Lỗi")
                   };
                   currentVM = viewModel;
                   if (CurrentViewModel is IDataViewModel dataViewModel)
                   {
                       await dataViewModel.LoadData(_cts.Token);
                   }
               }, _cts.Token);
            }
            catch (OperationCanceledException oe)
            {
                Debug.WriteLine(oe.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsLeftDrawerOpen = false;
            }
        }

        [RelayCommand]
        private void SignOut()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = default;
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
            _accountStore.ClearAccount();
            Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void LoadAccount()
        {
            if (_accountStore.Account != null)
            {
                AdminAccount = _accountStore.Account;
            }
        }

        [RelayCommand]
        private void OpenMenu()
        {
            IsLeftDrawerOpen = true;
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
            }
            if (OpenSettingAccountVM != null)
            {
                OpenSettingAccountVM.Close -= OpenSettingAccountVM_Close;
            }
        }

        [ObservableProperty]
        private string _selectedOption = "Home";

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
    }
}