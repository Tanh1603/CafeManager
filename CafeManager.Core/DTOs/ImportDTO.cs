using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class ImportDTO : BaseDTO
    {
        [ObservableProperty]
        private int _importid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Người giao hàng không được trống")]
        private string _deliveryperson;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        [RegularExpression(@"^0[0-9]{9,10}$",
        ErrorMessage = "SĐT không hợp lệ.")]
        private string _phone;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _shippingcompany;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private DateTime _receiveddate;

        [ObservableProperty]
        private int _staffid;

        [ObservableProperty]
        private int _supplierid;

        [ObservableProperty]
        private SupplierDTO _supplier;

        [ObservableProperty]
        private StaffDTO _staff;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalPrice))]
        private ObservableCollection<ImportDetailDTO> _importdetails = [];

        public decimal TotalPrice => Importdetails?.Where(x => x.Isdeleted == false)
            .Sum(x => x.Quantity * x.Materialsupplier.Price) ?? 0;

        public ImportDTO Clone()
        {
            return new ImportDTO
            {
                Id = Id,
                Importid = Importid,
                Deliveryperson = Deliveryperson,
                Phone = Phone,
                Shippingcompany = Shippingcompany,
                Receiveddate = Receiveddate,
                Staffid = Staffid,
                Supplierid = Supplierid,
                Isdeleted = this.Isdeleted,

                Importdetails = Importdetails,
                Staff = Staff,
                Supplier = Supplier,
            };
        }
    }
}