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

namespace CafeManager.WPF.ViewModels
{
    public partial class MainUserViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly FoodServices _foodServices;
        private readonly CoffeTableServices _coffeTableServices;
        private readonly FoodCategoryServices _foodCategoryServices;
        private readonly FileDialogService _fileDialogService;
        private readonly InvoiceServices _invoiceServices;

        private decimal _totalPrice = 0;

        [ObservableProperty]
        private ObservableObject _orderVM;

        public MainUserViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _coffeTableServices = provider.GetRequiredService<CoffeTableServices>();
            _foodServices = provider.GetRequiredService<FoodServices>();
            _foodCategoryServices = provider.GetRequiredService<FoodCategoryServices>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            _invoiceServices = provider.GetRequiredService<InvoiceServices>();

            OrderVM = provider.GetRequiredService<OrderViewModel>();
        }

        [RelayCommand]
        private void SignOut()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }
    }
}