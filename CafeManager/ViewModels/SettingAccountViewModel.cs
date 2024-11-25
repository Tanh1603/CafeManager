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

namespace CafeManager.WPF.ViewModels
{
    public partial class SettingAccountViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly AccountStore _accountStore;
        private readonly AppUserServices _appUserServices;
        private readonly EncryptionHelper _encryptionHelper;
        private readonly FileDialogService _fileDialogService;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private AppUserDTO _account = new();

        [ObservableProperty]
        private string _oldPassword = string.Empty;

        [ObservableProperty]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private bool _isOpenChangePassWord = false;

        public SettingAccountViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _accountStore = provider.GetRequiredService<AccountStore>();
            _appUserServices = provider.GetRequiredService<AppUserServices>();
            _encryptionHelper = provider.GetRequiredService<EncryptionHelper>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            _mapper = provider.GetRequiredService<IMapper>();
            LoadData();
        }

        private void LoadData()
        {
            if (_accountStore.Account != null)
            {
                Account = _accountStore.Account.Clone();
            }
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
                if (_accountStore.Account != null)
                {
                    Appuser? appuser = await _appUserServices.GetAppUserByUserName(_accountStore.Account.Username);
                    if (appuser == null)
                    {
                        return;
                    }
                    appuser.Username = Account.Username;
                    appuser.Displayname = Account.Displayname;
                    appuser.Email = Account.Email;
                    appuser.Avatar = ConvertImageServices.BitmapImageToBase64(Account.Avatar);
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
                if (_accountStore.Account != null)
                {
                    if (Account.Username != _accountStore.Account.Username
                || Account.Displayname != _accountStore.Account.Displayname
                || Account.Email != _accountStore.Account.Email
                || ConvertImageServices.BitmapImageToBase64(Account.Avatar) != ConvertImageServices.BitmapImageToBase64(_accountStore.Account.Avatar)
                )
                    {
                        MyMessageBox.ShowDialog("Vui lòng cập nhật tài khoản trước khi thay đổi mật khẩu");
                        return;
                    }
                    if (OldPassword.Equals(NewPassword))
                    {
                        MyMessageBox.ShowDialog("Mật khẩu mới và cũ trùng nhau");
                        return;
                    }
                    if (string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(OldPassword))
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
                        string oldHash = _encryptionHelper.DecryptAES(appuser.Password);
                        if (!oldHash.Equals(OldPassword))
                        {
                            MyMessageBox.ShowDialog("Mật khẩu cũ không khớp");
                            return;
                        }
                        appuser.Password = _encryptionHelper.EncryptAES(NewPassword);
                        var res = await _appUserServices.UpdateAppUser(appuser);
                        if (res != null)
                        {
                            Properties.Settings.Default.UserName = string.Empty;
                            Properties.Settings.Default.PassWord = string.Empty;
                            Properties.Settings.Default.RememberAccount = false;
                            Properties.Settings.Default.Save();
                            MyMessageBox.ShowDialog("Mật khẩu đổi thành công");
                            IsOpenChangePassWord = false;
                        }
                    }
                    OldPassword = string.Empty;
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
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
        }
    }
}