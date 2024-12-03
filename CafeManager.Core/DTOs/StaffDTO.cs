using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class StaffDTO : BaseDTO
    {
        private int _staffid;
        private string _staffname;
        private string _phone;
        private DateOnly _birthday = DateOnly.FromDateTime(DateTime.Now);
        private string _address;
        private DateOnly _startworkingdate = DateOnly.FromDateTime(DateTime.Now);
        private bool _sex;
        private DateOnly? _endworkingdate = null;
        private string _role;
        private bool _isdeleted;

        public int Staffid
        {
            get => _staffid;
            set
            {
                if (_staffid != value)
                {
                    _staffid = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Staffname
        {
            get => _staffname;
            set
            {
                if (_staffname != value)
                {
                    _staffname = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Sex
        {
            get => _sex;
            set
            {
                if (_sex != value)
                {
                    _sex = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly Birthday
        {
            get => _birthday;
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly Startworkingdate
        {
            get => _startworkingdate;
            set
            {
                if (_startworkingdate != value)
                {
                    _startworkingdate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly? Endworkingdate
        {
            get => _endworkingdate;
            set
            {
                if (_endworkingdate != value)
                {
                    _endworkingdate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Isdeleted
        {
            get => _isdeleted;
            set
            {
                if (_isdeleted != value)
                {
                    _isdeleted = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<StaffsalaryhistoryDTO> _staffsalaryhistories = [];

        public ObservableCollection<StaffsalaryhistoryDTO> Staffsalaryhistories
        {
            get => _staffsalaryhistories;
            set
            {
                if (_staffsalaryhistories != value)
                {
                    _staffsalaryhistories = value;
                    OnPropertyChanged();
                }
            }
        }

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