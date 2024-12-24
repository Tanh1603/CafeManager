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
        private readonly AccountStore _accountStore;
        private readonly AppUserServices _appUserServices;
        private readonly EncryptionHelper _encryptionHelper;
        private readonly FileDialogService _fileDialogService;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private AppUserDTO _account = new();

        public event Action? Close;

        [ObservableProperty]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private bool _isOpenChangePassWord = false;

        public UpdateAppUserViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _accountStore = provider.GetRequiredService<AccountStore>();
            _appUserServices = provider.GetRequiredService<AppUserServices>();
            _encryptionHelper = provider.GetRequiredService<EncryptionHelper>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            _mapper = provider.GetRequiredService<IMapper>();
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
        private async Task UpdateAccount()
        {
            try
            {
                if (Account != null)
                {
                    Appuser? appuser = await _appUserServices.GetAppUserByUserName(Account.Username);
                    if (appuser == null)
                    {
                        return;
                    }
                    appuser.Username = Account.Username;
                    appuser.Displayname = Account.Displayname;
                    appuser.Email = Account.Email;
                    appuser.Avatar = ConvertImageServices.BitmapImageToByteArray(Account.Avatar);
                    Appuser? res = await _appUserServices.UpdateAppUser(appuser);
                    if (res != null)
                    {
                        MyMessageBox.ShowDialog("Cập nhật tài khoản thành công");
                        Properties.Settings.Default.UserName = string.Empty;
                        Properties.Settings.Default.PassWord = string.Empty;
                        Properties.Settings.Default.RememberAccount = false;
                        Properties.Settings.Default.Save();
                        _accountStore.SetAccount(_mapper.Map<AppUserDTO>(res));
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private async Task SubmitChangePassWord()
        {
            try
            {
                if (Account != null)
                {
                    if (string.IsNullOrEmpty(NewPassword))
                    {
                        MyMessageBox.ShowDialog("Mật khẩu không được để trống");
                        return;
                    }

                    if (!ConfirmPassword.Equals(NewPassword))
                    {
                        MyMessageBox.ShowDialog("Mật khẩu mới và xác nhận mật khẩu mới không trùng");
                        return;
                    }

                    Appuser? appuser = await _appUserServices.GetAppUserByUserName(Account.Username);
                    if (appuser != null)
                    {
                        appuser.Password = _encryptionHelper.EncryptAES(NewPassword);
                        var res = await _appUserServices.UpdateAppUser(appuser);
                        if (res != null)
                        {
                            MyMessageBox.ShowDialog("Mật khẩu đổi thành công");
                            IsOpenChangePassWord = false;
                        }
                    }
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message);
            }
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

        [RelayCommand]
        private void CloseChangePassWord()
        {
            IsOpenChangePassWord = false;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            Close?.Invoke();
        }
    }
}