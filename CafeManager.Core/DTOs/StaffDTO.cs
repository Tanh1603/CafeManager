using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class StaffDTO : BaseDTO
    {
        [ObservableProperty]
        private int _staffid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Tên nhân viên không được trống")]
        private string _staffname;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [CustomValidation(typeof(StaffDTO), nameof(ValidatePhone))]
        private string _phone;

        public static ValidationResult ValidatePhone(string phone, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return new ValidationResult("Số điện thoại không được để trống");
            }

            if (!string.IsNullOrEmpty(phone.ToString()) && phone.ToString().All(char.IsLetter))
            {
                return new ValidationResult("Số điện thoại chỉ được chứa chữ cái");
            }

            return ValidationResult.Success;
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Sinh nhật không được trống")]
        private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Now);

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Địa chỉ không được trống")]
        private string _address;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Ngày vào làm không được trống")]
        [CustomValidation(typeof(StaffDTO), nameof(ValidateStartDate))]
        private DateOnly _startworkingdate = DateOnly.FromDateTime(DateTime.Now);

        public static ValidationResult ValidateStartDate(object startDate, ValidationContext context)
        {
            var obj = context.ObjectInstance as StaffDTO;
            foreach (var item in obj.Staffsalaryhistories)
            {
                if (item.Isdeleted == false && item.Effectivedate < obj.Startworkingdate) return new("Không thể nhỏ hơn ngày hiệu lực lương bắt đầu");
            }
            return ValidationResult.Success;
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Giới tính không được trống")]
        private bool _sex;

        [ObservableProperty]
        private DateOnly? _endworkingdate = null;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Vai trò không được trống")]
        private string _role;

        [ObservableProperty]
        private ObservableCollection<StaffsalaryhistoryDTO> _staffsalaryhistories = [];

        public StaffDTO Clone()
        {
            return new StaffDTO
            {
                Id = Id,
                Staffid = Staffid,
                Staffname = Staffname,
                Phone = Phone,
                Sex = Sex,
                Birthday = Birthday,
                Address = Address,
                Startworkingdate = Startworkingdate,
                Endworkingdate = Endworkingdate,
                Role = Role,
                Isdeleted = Isdeleted,
                Staffsalaryhistories = Staffsalaryhistories
            };
        }
    }
}