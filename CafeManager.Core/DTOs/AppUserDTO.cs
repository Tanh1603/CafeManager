using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class AppUserDTO : BaseDTO
    {
        [ObservableProperty]
        private int _appuserid;

        private string _username;

        [Required(ErrorMessage = "Tài khoản không được trống")]
        [CustomValidation(typeof(AppUserDTO), nameof(ValidateUserName))]
        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value, true);
                NotifyDataErrors?.Invoke();
            }
        }

        public static ValidationResult ValidateUserName(string userName, ValidationContext context)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]");

            if (string.IsNullOrEmpty(userName) || regex.IsMatch(userName))
            {
                return ValidationResult.Success!;
            }
            return new ValidationResult("Tài khoản người dùng chỉ được chứa chữ cái");
        }

        private string _password;

        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự")]
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value, true);
                NotifyDataErrors?.Invoke();
            }
        }

        private string _displayname;

        [MinLength(10, ErrorMessage = "Tên hiển thị tối thiểu 10 ký tự")]
        public string Displayname
        {
            get => _displayname;
            set
            {
                SetProperty(ref _displayname, value, true);
                NotifyDataErrors?.Invoke();
            }
        }

        private string _email;

        [Required(ErrorMessage = "Email không được để trống.")]
        [CustomValidation(typeof(AppUserDTO), nameof(ValidateEmail))]
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value, true);
                NotifyDataErrors?.Invoke();
            }
        }

        public static ValidationResult ValidateEmail(string email, ValidationContext context)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!string.IsNullOrEmpty(email) && regex.IsMatch(email))
            {
                return ValidationResult.Success!;
            }
            return new ValidationResult("Email không hợp lệ. Vui lòng nhập đúng định dạng.");
        }

        [ObservableProperty]
        private string _role;

        [ObservableProperty]
        private BitmapImage? _avatar;

        public event Action NotifyDataErrors;

        public AppUserDTO Clone()
        {
            return new AppUserDTO
            {
                Id = Id,
                Appuserid = Appuserid,
                Username = Username,
                Displayname = Displayname,
                Email = Email,
                Role = Role,
                Password = Password,
                Avatar = Avatar,
                Isdeleted = Isdeleted
            };
        }
    }
}