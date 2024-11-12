using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels;
using CafeManager.WPF.ViewModels.AddViewModel;
using CafeManager.WPF.ViewModels.AdminViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainAdminViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AccountStore _accountStore;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        [ObservableProperty]
        private string _displayname;

        [ObservableProperty]
        private string _role;

        [ObservableProperty]
        private BitmapImage? _imageAccount;

        public MainAdminViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = _provider.GetRequiredService<NavigationStore>();
            _accountStore = provider.GetRequiredService<AccountStore>();
            CurrentViewModel = _provider.GetRequiredService<HomeViewModel>();

            LoadAccount();
            _accountStore.ChangeAccount += _accountStore_ChangeAccount;
        }

        private void _accountStore_ChangeAccount()
        {
            LoadAccount();
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
                    CurrentViewModel = _provider.GetRequiredService<FoodViewModel>();
                    break;

                case "Invoice":
                    CurrentViewModel = _provider.GetRequiredService<InvoiceViewModel>();
                    break;

                case "Table":
                    CurrentViewModel = _provider.GetRequiredService<TableViewModel>();
                    break;

                case "Import":
                    CurrentViewModel = _provider.GetRequiredService<ImportViewModel>();
                    break;

                case "Inventory":
                    CurrentViewModel = _provider.GetRequiredService<InventoryViewModel>();
                    break;

                case "Supplier":
                    CurrentViewModel = _provider.GetRequiredService<SupplierViewModel>();
                    break;

                case "Staff":
                    CurrentViewModel = _provider.GetRequiredService<StaffViewModel>();
                    break;

                case "AppUser":
                    CurrentViewModel = _provider.GetRequiredService<AppUserViewModel>();
                    break;

                case "Setting":
                    CurrentViewModel = _provider.GetRequiredService<SettingAccountViewModel>();
                    break;

                default:
                    break;
            }
        }

        [RelayCommand]
        private void SignOut()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
            _accountStore.ClearAccount();
        }

        private void LoadAccount()
        {
            Displayname = _accountStore.Account?.Displayname ?? "No name";
            Role = (_accountStore.Account?.Role == 1) ? "Admin" : "User";
            ImageAccount =
                _provider.GetRequiredService<FileDialogService>().Base64ToBitmapImage(_accountStore.Account?.Avatar ?? string.Empty);
        }
    }
}