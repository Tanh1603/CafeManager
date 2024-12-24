using CommunityToolkit.Mvvm.ComponentModel;

namespace CafeManager.Core.DTOs
{
    public partial class StaffsalaryhistoryDTO : BaseDTO
    {
        [ObservableProperty]
        private int _staffsalaryhistoryid;

        [ObservableProperty]
        private int _staffid;

        [ObservableProperty]
        private decimal _salary;

        [ObservableProperty]
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