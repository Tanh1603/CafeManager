using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class StaffsalaryhistoryDTO : INotifyPropertyChanged
    {
        private int _staffsalaryhistoryid;

        public int Staffsalaryhistoryid
        {
            get => _staffsalaryhistoryid;
            set
            {
                if (_staffsalaryhistoryid != value)
                {
                    _staffsalaryhistoryid = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private decimal _salary;

        public decimal Salary
        {
            get => _salary;
            set
            {
                if (_salary != value)
                {
                    _salary = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateOnly _effectivedate;

        public DateOnly Effectivedate
        {
            get => _effectivedate;
            set
            {
                if (_effectivedate != value)
                {
                    _effectivedate = value;
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

        public StaffsalaryhistoryDTO Clone()
        {
            return new StaffsalaryhistoryDTO
            {
                Staffsalaryhistoryid = this.Staffsalaryhistoryid,
                Staffid = this.Staffid,
                Salary = this.Salary,
                Effectivedate = this.Effectivedate
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}