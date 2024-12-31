using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using CafeManager.WPF.MessageBox;
using System.Windows.Media.Imaging;
using CafeManager.Core.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Drawing.Text;
using CafeManager.Core.DTOs;
using AutoMapper;

#nullable disable

namespace CafeManager.WPF.ViewModels
{
    public partial class RegisterViewModel : ObservableValidator, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AppUserServices _appUserServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanRegisterExcute))]
        [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
        private AppUserDTO _registerAccount = new();

        [ObservableProperty]
        private bool _isOpenVerificationEmail;

        [ObservableProperty]
        private string _vertificationCode;

        public RegisterViewModel(IServiceScope scope)
        {
            _provider = scope.ServiceProvider;
            _navigationStore = _provider.GetRequiredService<NavigationStore>();
            _appUserServices = _provider.GetRequiredService<AppUserServices>();
            _mapper = _provider.GetRequiredService<IMapper>();
            RegisterAccount.NotifyDataErrors += RegisterAccount_NotifyDataErrors;
        }

        private void RegisterAccount_NotifyDataErrors()
        {
            OnPropertyChanged(nameof(CanRegisterExcute));
        }

        [RelayCommand(CanExecute = nameof(CanRegisterExcute))]
        private void Register()
        {
            try
            {
                Task.Run(async () => await _appUserServices.SendVerificationEmail(RegisterAccount.Email, RegisterAccount.Username));
                IsOpenVerificationEmail = true;
                MyMessageBox.Show("Kiểm tra tài khoản email", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        public bool CanRegisterExcute => !RegisterAccount.HasErrors;

        [RelayCommand]
        private async Task SubmitRegister()
        {
            try
            {
                IsLoading = true;
                if (VertificationCode == _appUserServices.VerificationCode)
                {
                    Appuser appuser = _mapper.Map<Appuser>(RegisterAccount);
                    Appuser res = await _appUserServices.Register(appuser);
                    IsLoading = false;
                    if (res == null)
                    {
                        MyMessageBox.Show("Đăng kí tài khoản thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                    MyMessageBox.Show("Đăng kí tài khoản thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
                }
                else
                {
                    IsLoading = false;
                    MyMessageBox.Show("Mã xác nhận nhập sai vui lòng nhập lại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
        }

        [RelayCommand]
        private void CloseOpenVerificationEmail()
        {
            IsOpenVerificationEmail = false;
            VertificationCode = string.Empty;
        }

        public void Dispose()
        {
            RegisterAccount.NotifyDataErrors -= RegisterAccount_NotifyDataErrors;
        }
    }
}