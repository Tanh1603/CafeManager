using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CafeManager.Core.DTOs
{
    public partial class StaffsalaryhistoryDTO : BaseDTO
    {
        [ObservableProperty]
        private int _staffsalaryhistoryid;

        [ObservableProperty]
        private int _staffid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Lương không được trống")]
        private decimal _salary;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Ngày có hiêu lực không được trống")]
        private DateOnly _effectivedate = DateOnly.FromDateTime(DateTime.Now);

        public StaffsalaryhistoryDTO Clone()
        {
            return new StaffsalaryhistoryDTO
            {
                Id = Id,
                Staffsalaryhistoryid = Staffsalaryhistoryid,
                Staffid = Staffid,
                Salary = Salary,
                Effectivedate = Effectivedate
            };
        }
    }
}