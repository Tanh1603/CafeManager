using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CafeManager.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private IServiceProvider _provider;

        [ObservableProperty]
        private ObservableObject _currentViewModel;

        public MainViewModel(IServiceProvider provider)
        {
            _provider = provider;

            CurrentViewModel = _provider.GetRequiredService<MainAdminViewModel>();
        }

        [RelayCommand]
        private void ChangeCurrentViewModel(string model)
        {
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
    }
}