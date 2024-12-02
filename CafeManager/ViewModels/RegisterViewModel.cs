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

#nullable disable

namespace CafeManager.WPF.ViewModels
{
    
    public partial class RegisterViewModel : ObservableValidator
    {
        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;
        private readonly AppUserServices _appUserServices;
        private readonly FileDialogService _fileDialogService;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Tên người dùng không được để trống.")]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateUserName))]
        private string _username;

        public static ValidationResult ValidateUserName(string userName, ValidationContext context)
        {
            // Quy tắc: Chỉ cho phép ký tự chữ và số, 3-20 ký tự
            var regex = new Regex(@"^[a-zA-Z0-9]{3,20}$");

            if (string.IsNullOrEmpty(userName) || regex.IsMatch(userName))
            {
                return ValidationResult.Success!;
            }
            return new ValidationResult("Tên người dùng chỉ được chứa chữ cái, số và dài từ 3 đến 20 ký tự.");
        }


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Tên hiển thị không được để trống.")]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateDisplayName))]
        private string _displayname;

        public static ValidationResult ValidateDisplayName(string displayName, ValidationContext context)
        {
           
            var regex = new Regex(@"^[a-zA-Z0-9\s\-]{3,50}$");

            if (string.IsNullOrEmpty(displayName) || regex.IsMatch(displayName))
            {
                return ValidationResult.Success!;
            }
            return new ValidationResult("Tên hiển thị chỉ được chứa chữ cái, số, khoảng trắng, dấu gạch ngang và dài từ 3 đến 50 ký tự.");
        }
        


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidatePassword))]
        private string _password;


        public static ValidationResult ValidatePassword(string password, ValidationContext context)
        {
            
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$");

            if (string.IsNullOrEmpty(password) || regex.IsMatch(password))
            {
                return ValidationResult.Success!;
            }
            return new ValidationResult("Mật khẩu phải chứa ít nhất 1 chữ hoa, 1 chữ thường, 1 số, 1 ký tự đặc biệt và có độ dài từ 8-20 ký tự.");
        }


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Email không được để trống.")]
        [CustomValidation(typeof(RegisterViewModel), nameof(ValidateEmail))]
        private string _email;


        public static ValidationResult ValidateEmail(string email, ValidationContext context)
        {
            // Biểu thức chính quy để xác thực email
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (string.IsNullOrEmpty(email) || regex.IsMatch(email))
            {
                return ValidationResult.Success!;
            }
            return new ValidationResult("Email không hợp lệ. Vui lòng nhập đúng định dạng.");
        }

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
                    Appuser appuser = new()
                    {
                        Username = this.Username,
                        Displayname = this.Displayname,
                        Password = this.Password,
                        Email = this.Email,
                        Role = (this.Role ?? string.Empty).Equals("Admin") ? 1 : 0,
                        Avatar = ConvertImageServices.BitmapImageToBase64(this.Avata)
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