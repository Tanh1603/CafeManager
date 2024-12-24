using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class StaffDTO : BaseDTO
    {
        [ObservableProperty]
        private int _staffid;

        [ObservableProperty]
        private string _staffname;

        [ObservableProperty]
        private string _phone;

        [ObservableProperty]
        private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Now);

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private DateOnly _startworkingdate = DateOnly.FromDateTime(DateTime.Now);

        [ObservableProperty]
        private bool _sex;

        [ObservableProperty]
        private DateOnly? _endworkingdate = null;

        [ObservableProperty]
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