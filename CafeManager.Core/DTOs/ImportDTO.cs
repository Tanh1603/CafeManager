using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class ImportDTO : INotifyPropertyChanged
    {
        private int _importid;
        private string _deliveryperson;
        private string _phone;
        private string _shippingcompany;
        private DateTime _receiveddate;
        private int _staffid;
        private int _supplierid;
        private bool? _isdeleted;

        private SupplierDTO _supplier;
        private StaffDTO _staff;
        private ObservableCollection<ImportDetailDTO> _importdetails = [];

        public int Importid
        {
            get => _importid;
            set
            {
                _importid = value;
                OnPropertyChanged();
            }
        }

        public string Deliveryperson
        {
            get => _deliveryperson;
            set
            {
                _deliveryperson = value;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        public string Shippingcompany
        {
            get => _shippingcompany;
            set
            {
                _shippingcompany = value;
                OnPropertyChanged();
            }
        }

        public DateTime Receiveddate
        {
            get => _receiveddate;
            set
            {
                _receiveddate = value;
                OnPropertyChanged();
            }
        }

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

        public int Staffid
        {
            get => _staffid;
            set
            {
                _staffid = value;
                OnPropertyChanged();
            }
        }

        public int Supplierid
        {
            get => _supplierid;
            set
            {
                _supplierid = value;
                OnPropertyChanged();
            }
        }

        public SupplierDTO Supplier
        {
            get => _supplier;
            set
            {
                if (_supplier != value)
                {
                    _supplier = value;
                    OnPropertyChanged();
                }
            }
        }

        public StaffDTO Staff
        {
            get => _staff;
            set
            {
                if (_staff != value)
                {
                    _staff = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ImportDetailDTO> Importdetails
        {
            get => _importdetails;
            set
            {
                if (_importdetails != value)
                {
                    _importdetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public ImportDTO Clone()
        {
            return new ImportDTO
            {
                Importid = this.Importid,
                Deliveryperson = this.Deliveryperson,
                Phone = this.Phone,
                Shippingcompany = this.Shippingcompany,
                Receiveddate = this.Receiveddate,
                Staffid = this.Staffid,
                Supplierid = this.Supplierid,
                Isdeleted = this.Isdeleted,

                Importdetails = this.Importdetails,
                Staff = this.Staff,
                Supplier = this.Supplier,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}