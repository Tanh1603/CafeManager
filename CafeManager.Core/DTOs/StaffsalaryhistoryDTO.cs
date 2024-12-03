namespace CafeManager.Core.DTOs
{
    public class StaffsalaryhistoryDTO : BaseDTO
    {
        private int _staffsalaryhistoryid;
        private int _staffid;
        private decimal _salary;
        private bool _isdeleted;

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

        private DateOnly _effectivedate = DateOnly.FromDateTime(DateTime.Now);

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