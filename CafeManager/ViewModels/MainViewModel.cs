using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels.AdminViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;

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
            CurrentViewModel = provider.GetRequiredService<MainAdminViewModel>();
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

        #region handleDatePicker

        [RelayCommand]
        public void ClearDatePicker(DatePicker datePicker)
        {
            if (datePicker != null)
            {
                datePicker.SelectedDate = null;

                var textBox = FindChild<DatePickerTextBox>(datePicker);
                if (textBox != null)
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                    return tChild;

                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        #endregion handleDatePicker

        #region handleComboBox

        [RelayCommand]
        public void ClearComboBox(ComboBox comboBox)

        {
            if (comboBox != null)
            {
                comboBox.SelectedItem = null; // Đặt SelectedItem về null
                comboBox.Text = string.Empty; // Xóa nội dung Text
            }
        }

        #endregion handleComboBox

        public void Dispose()
        {
            _navigationStore.NavigationStoreChanged -= _navigationStore_NavigationStoreChanged;
        }
    }
}