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

namespace CafeManager.WPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AppUserServices _appUserServices;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _resetUser;

        [ObservableProperty]
        private string _resetEmail;

        [ObservableProperty]
        private string _vertificationCode;

        [ObservableProperty]
        private bool _isOpenResetPassWord;

        [ObservableProperty]
        private bool _isRememberAccount;

        public LoginViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = _provider.GetRequiredService<NavigationStore>();
            _appUserServices = provider.GetRequiredService<AppUserServices>();

            if (Properties.Settings.Default.RememberAccount)
            {
                Username = Properties.Settings.Default.UserName;
                Password = _provider.GetRequiredService<EncryptionHelper>().DecryptAES(Properties.Settings.Default.PassWord);
                IsRememberAccount = Properties.Settings.Default.RememberAccount;
            }
        }

        [RelayCommand]
        private async Task Login()
        {
            try
            {
                (Appuser? appuser, bool isSuccessLogin, int? role) = await _appUserServices.Login(Username, Password);
                if (isSuccessLogin && role != null && appuser != null)
                {
                    MessageBoxResult dialogResult = MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        _provider.GetRequiredService<AccountStore>().SetAccount(appuser);
                        _navigationStore.Navigation = role == 1 ? _provider.GetRequiredService<MainAdminViewModel>() : _provider.GetRequiredService<MainUserViewModel>();
                    }
                    if (IsRememberAccount)
                    {
                        Properties.Settings.Default.UserName = Username;
                        Properties.Settings.Default.PassWord = _provider.GetRequiredService<EncryptionHelper>().EncryptAES(Password);
                        Properties.Settings.Default.RememberAccount = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.RememberAccount = false;
                        Properties.Settings.Default.Save();
                    }
                }
                else if (isSuccessLogin == false && role == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại");
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại");
                }
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private void NavigateToRegister()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<RegisterViewModel>();
        }

        [RelayCommand]
        private void CloseOpenResetPassWord()
        {
            IsOpenResetPassWord = false;
            Username = string.Empty;
            Password = string.Empty;
        }

        [RelayCommand]
        private async Task SubmitResetPassWord()
        {
            try
            {
                if (await _appUserServices.HasAppUser(ResetUser, ResetEmail) == false)
                {
                    MessageBox.Show("Tài khoản không tồn tại");
                    return;
                }
                _appUserServices.SendResetPassWord(ResetEmail, ResetUser);
                bool isSuccessResetPassWord = await _appUserServices.ResetPassWord(ResetUser);
                if (isSuccessResetPassWord)
                {
                    MessageBox.Show($"Mật khẩu tài khoản {ResetUser} đã đổi thành công");
                }
                else
                {
                    MessageBox.Show($"Mật khẩu tài khoản {ResetUser} đổi thất bại");
                }
                IsOpenResetPassWord = false;
                ResetUser = string.Empty;
                ResetEmail = string.Empty;
            }
            catch (InvalidOperationException ioe)
            {
                MessageBox.Show(ioe.Message);
            }
        }

        [RelayCommand]
        private void OpenResetPassWord()
        {
            IsOpenResetPassWord = true;
        }
    }
}