using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class UpdateAppUserViewModel : ObservableObject
    {
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        private AppUserDTO _account = new();

        [ObservableProperty]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private bool _isOpenChangePassWord = false;

        public event Action<AppUserDTO>? ModifyAppUserChanged;

        public UpdateAppUserViewModel(IServiceScope scope)
        {
            _fileDialogService = scope.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<FileDialogService>();
        }

        public void ReceiveAccount(AppUserDTO account)
        {
            Account = account.Clone();
        }

        [RelayCommand]
        private void OpenChangePassWord()
        {
            IsOpenChangePassWord = true;
        }

        [RelayCommand]
        private void UpdateAccount()
        {
            ModifyAppUserChanged?.Invoke(Account.Clone());
        }

        [RelayCommand]
        private void OpenUploadImage()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _fileDialogService.OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                Account.Avatar = new BitmapImage(new Uri(filePath));
                OnPropertyChanged(nameof(Account));
            }
        }
    }
}