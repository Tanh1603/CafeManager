using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels
{
    public partial class SettingAccountViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly AccountStore _accountStore;
        private readonly AppUserServices _appUserServices;
        private readonly FileDialogService _fileDialogService;
        private readonly EncryptionHelper _encryptionHelper;

        [ObservableProperty]
        private string _displayname;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _role;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private BitmapImage? _image;

        [ObservableProperty]
        private string _oldpassword;

        [ObservableProperty]
        private string _newpassword;

        [ObservableProperty]
        private bool _isOpenChangePassWord;

        public SettingAccountViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _accountStore = provider.GetRequiredService<AccountStore>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
            _appUserServices = provider.GetRequiredService<AppUserServices>();
            _encryptionHelper = provider.GetRequiredService<EncryptionHelper>();

            LoadData();
        }

        private void LoadData()
        {
            Username = _accountStore.Account?.Username ?? string.Empty;
            Displayname = _accountStore.Account?.Displayname ?? string.Empty;
            Email = _accountStore?.Account?.Email ?? string.Empty;
            Role = _accountStore?.Account?.Role == 1 ? "Admin" : "User";
            Image = _fileDialogService.Base64ToBitmapImage(_accountStore?.Account?.Avatar ?? string.Empty) ?? null;
            Password = _encryptionHelper.DecryptAES(_accountStore?.Account?.Password ?? string.Empty);
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
                Appuser? appuser = await _appUserServices.GetAppUserByUserName(_accountStore.Account.Username);
                if (appuser == null)
                {
                    return;
                }
                appuser.Displayname = Displayname ?? string.Empty;
                appuser.Username = Username;
                appuser.Email = Email;
                appuser.Avatar = _fileDialogService.ConvertBitmapImageToBase64(Image);

                var res = await _appUserServices.UpdateAppUser(appuser);
                if (res != null)
                {
                    //MessageBox.Show("Cập nhật tài khoản thành công");
                    Properties.Settings.Default.UserName = string.Empty;
                    Properties.Settings.Default.PassWord = string.Empty;
                    Properties.Settings.Default.RememberAccount = false;
                    Properties.Settings.Default.Save();
                    _accountStore.SetAccount(res);
                }
                else
                {
                    //MessageBox.Show("Lỗi");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [RelayCommand]
        private async Task SubmitChangePassWord()
        {
            try
            {
                if (Oldpassword.Equals(Newpassword))
                {
                    //MessageBox.Show("Mật khẩu mới và cũn trùng nhau");
                    return;
                }
                Appuser? appuser = await _appUserServices.GetAppUserByUserName(_accountStore.Account.Username);
                if (appuser != null)
                {
                    string oldHash = _encryptionHelper.DecryptAES(appuser.Password);
                    if (!oldHash.Equals(Oldpassword))
                    {
                        //MessageBox.Show("Mật khẩu cũ không khớp");
                        return;
                    }
                    appuser.Password = _encryptionHelper.EncryptAES(Newpassword);
                    var res = await _appUserServices.UpdateAppUser(appuser);
                    if (res != null)
                    {
                        _accountStore.SetAccount(res);
                        Properties.Settings.Default.UserName = string.Empty;
                        Properties.Settings.Default.PassWord = string.Empty;
                        Properties.Settings.Default.RememberAccount = false;
                        Properties.Settings.Default.Save();

                        //MessageBox.Show("Mật khẩu đổi thành công");
                        IsOpenChangePassWord = false;
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {
                //MessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private void CloseChangePassWord()
        {
            IsOpenChangePassWord = false;
            Oldpassword = string.Empty;
            Newpassword = string.Empty;
        }
    }
}