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
using CafeManager.WPF.MessageBox;
using CafeManager.Core.Services;
using AutoMapper;
using CafeManager.Core.DTOs;

namespace CafeManager.WPF.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AppUserServices _appUserServices;
        private readonly IMapper _mapper;

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
            _mapper = provider.GetRequiredService<IMapper>();
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
                    string dialogResult = MyMessageBox.ShowDialog("Đăng nhập thành công", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Information);
                    if (dialogResult == "1")
                    {
                        _provider.GetRequiredService<AccountStore>().SetAccount(_mapper.Map<AppUserDTO>(appuser));
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
                    MyMessageBox.ShowDialog("Tài khoản không tồn tại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                }
                else
                {
                    MyMessageBox.ShowDialog("Đăng nhập thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
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
                    MyMessageBox.ShowDialog("Tài khoản không tồn tại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    return;
                }
                _appUserServices.SendResetPassWord(ResetEmail, ResetUser);
                bool isSuccessResetPassWord = await _appUserServices.ResetPassWord(ResetUser);
                if (isSuccessResetPassWord)
                {
                    MyMessageBox.ShowDialog($"Mật khẩu tài khoản {ResetUser} đã đổi thành công");
                }
                else
                {
                    MyMessageBox.ShowDialog($"Mật khẩu tài khoản {ResetUser} đổi thất bại");
                }
                IsOpenResetPassWord = false;
                ResetUser = string.Empty;
                ResetEmail = string.Empty;
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        [RelayCommand]
        private void OpenResetPassWord()
        {
            IsOpenResetPassWord = true;
        }
    }
}