using CafeManager.Core.Data;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using CafeManager.WPF.MessageBox;
using System.Windows.Media.Imaging;

namespace CafeManager.WPF.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AppUserServices _appUserServices;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _displayname;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _role;

        [ObservableProperty]
        private BitmapImage _avata;

        [ObservableProperty]
        private bool _isOpenVerificationEmail;

        [ObservableProperty]
        private string _vertificationCode;

        public RegisterViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _navigationStore = _provider.GetRequiredService<NavigationStore>();
            _appUserServices = _provider.GetRequiredService<AppUserServices>();
            _fileDialogService = provider.GetRequiredService<FileDialogService>();
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                _appUserServices.SendVerificationEmail(Email, Username);
                IsOpenVerificationEmail = true;
                MyMessageBox.Show("Kiểm tra tài khoản email", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        [RelayCommand]
        private async Task SubmitRegister()
        {
            try
            {
                if (VertificationCode == _appUserServices.VerificationCode)
                {
                    Appuser appuser = new Appuser()
                    {
                        Username = this.Username,
                        Displayname = this.Displayname,
                        Password = this.Password,
                        Email = this.Email,
                        Role = (this.Role ?? string.Empty).Equals("Admin") ? 1 : 0,
                        Avatar = _fileDialogService.ConvertBitmapImageToBase64(this.Avata)
                    };
                    Appuser res = await _appUserServices.Register(appuser);
                    if (res == null)
                    {
                        MyMessageBox.Show("Đăng kí tài khoản thất bại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                    }
                    MyMessageBox.Show("Đăng kí tài khoản thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                    _navigationStore.Navigation = _provider.GetRequiredService<LoginViewModel>();
                }
                else
                {
                    MyMessageBox.Show("Mã xác nhận nhập sai vui lòng nhập lại", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Error);
                }
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.Show(ioe.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
        }

        [RelayCommand]
        private void SetAvata()
        {
            string filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|All files (*.*)|*.*";
            var filePath = _fileDialogService.OpenFileDialog(filter);

            if (!string.IsNullOrEmpty(filePath))
            {
                Avata = new BitmapImage(new Uri(filePath));
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
    }
}