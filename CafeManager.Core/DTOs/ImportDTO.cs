using CafeManager.Core.Data;
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

        private SupplierDTO _supplierDTO;
        private StaffDTO _staffDTO;
        private ObservableCollection<ImportDetailDTO> _listImportDetailDTO = [];

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

        public SupplierDTO SupplierDTO
        {
            get => _supplierDTO;
            set
            {
                _supplierDTO = value;
                OnPropertyChanged();
            }
        }

        public StaffDTO StaffDTO
        {
            get => _staffDTO;
            set
            {
                _staffDTO = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImportDetailDTO> ListImportDetailDTO
        {
            get => _listImportDetailDTO;
            set
            {
                if (_listImportDetailDTO != value)
                {
                    _listImportDetailDTO = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TotalPriceOfImport));
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

                ListImportDetailDTO = this.ListImportDetailDTO,
                StaffDTO = this.StaffDTO,
                SupplierDTO = this.SupplierDTO,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal TotalPriceOfImport => CaculateTotalPrice();

        public decimal CaculateTotalPrice()
        {
            //return ListImportDetailDTO?.Sum(x =>
            //{
            //    decimal Price = x.MaterialDTO.MaterialsuppliersDTO.?.Price ?? 0;
            //    decimal quantity = x.Quantity ?? 0;

            //    return Price * quantity;
            //}) ?? 0;

            return 0;
        }
    }
}