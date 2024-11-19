using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class StaffDTO : INotifyPropertyChanged
    {
        private int _staffid;

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

        private string _staffname;

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

        private string _phone;

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

        private bool? _sex;

        public bool? Sex
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

        private DateOnly _birthday;

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

        private string _address;

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

        private DateOnly _startworkingdate;

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

        private DateOnly? _endworkingdate;

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

        private string _role;

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

        private bool? _isdeleted;

        public bool? Isdeleted
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

        private ObservableCollection<StaffsalaryhistoryDTO> _staffsalaryhistories = new();

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
                Staffid = this.Staffid,
                Staffname = this.Staffname,
                Phone = this.Phone,
                Sex = this.Sex,
                Birthday = this.Birthday,
                Address = this.Address,
                Startworkingdate = this.Startworkingdate,
                Endworkingdate = this.Endworkingdate,
                Role = this.Role,
                Isdeleted = this.Isdeleted,
                // Sao chép ObservableCollection Staffsalaryhistories (sao chép sâu)
                Staffsalaryhistories = new ObservableCollection<StaffsalaryhistoryDTO>(this.Staffsalaryhistories.Select(history => history.Clone()))
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}