using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels.AdminViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        private readonly NavigationStore _navigationStore;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        public MainViewModel(IServiceProvider provider)
        {
            _navigationStore = provider.GetRequiredService<NavigationStore>();
            _navigationStore.Navigation = CurrentViewModel;

            //CurrentViewModel = provider.GetRequiredService<LoginViewModel>();
            //CurrentViewModel = provider.GetRequiredService<MainAdminViewModel>();
            CurrentViewModel = provider.GetRequiredService<MainUserViewModel>();
            _navigationStore.NavigationStoreChanged += _navigationStore_NavigationStoreChanged;
        }

        private void _navigationStore_NavigationStoreChanged()
        {
            CurrentViewModel = _navigationStore.Navigation;
        }

        #region command handle window

        [RelayCommand]
        private void MiniMizeWindown()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        [RelayCommand]
        private void MaximizeWindown()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Normal)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        [RelayCommand]
        private void CloseWindown()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void DragMove()
        {
            Application.Current.MainWindow.DragMove();
        }

        #endregion command handle window

        public void Dispose()
        {
            _navigationStore.NavigationStoreChanged -= _navigationStore_NavigationStoreChanged;
        }
    }
}