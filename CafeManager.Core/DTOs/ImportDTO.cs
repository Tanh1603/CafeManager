using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.DTOs
{
    public class ImportDTO : INotifyPropertyChanged
    {
        private int _importid;
        public int Importid
        {
            get => _importid;
            set
            {
                _importid = value;
                OnPropertyChanged();
            }
        }


        private Supplier? _importSupplier;
        public Supplier? ImportSupplier
        {
            get => _importSupplier;
            set
            {
                _importSupplier = value;
                OnPropertyChanged();
            }
        }

        private string? _deliveryperson;
        public string? Deliveryperson
        {
            get => _deliveryperson;
            set
            {
                _deliveryperson = value;
                OnPropertyChanged();
            }
        }

        private string? _phone;
        public string? Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        private string? _shippingcompany;
        public string? Shippingcompany
        {
            get => _shippingcompany;
            set
            {
                _shippingcompany = value;
                OnPropertyChanged();
            }
        }

        private DateTime _receiveddate;
        public DateTime Receiveddate
        {
            get => _receiveddate;
            set
            {
                _receiveddate = value;
                OnPropertyChanged();
            }
        }

        private Staff _receivedStaff;
        public Staff ReceivedStaff
        {
            get => _receivedStaff;
            set
            {
                _receivedStaff = value;
                OnPropertyChanged();
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


        private ObservableCollection<ImportDetailDTO> _listImportDetailDTO = new ObservableCollection<ImportDetailDTO>();

        public ObservableCollection<ImportDetailDTO> ListImportDetailDTO
        {
            get => _listImportDetailDTO;
            set
            {
                if (_listImportDetailDTO != value)
                {
                    _listImportDetailDTO = value;
                    OnPropertyChanged();
                }
            }
        }

        public ImportDTO Clone()
        {
            return new ImportDTO
            {
                Importid = this.Importid,
                ImportSupplier = this.ImportSupplier,
                Deliveryperson = this.Deliveryperson,
                Phone = this.Phone,
                Shippingcompany = this.Shippingcompany,
                Receiveddate = this.Receiveddate,
                ReceivedStaff = this.ReceivedStaff,
                Isdeleted = this.Isdeleted,
                ListImportDetailDTO = new ObservableCollection<ImportDetailDTO>(this.ListImportDetailDTO.Select(history => history.Clone()))
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public decimal? CaculateTotalPrice()
        {
            return ListImportDetailDTO?.Sum(x =>
            {
                decimal Price = x.ModifyMaterialDetail?.Price ?? 0;
                decimal quantity = x.Quantity ?? 0;

                return Price * quantity;
            }) ?? 0;
        }
    }
}
