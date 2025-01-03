using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class SupplierDTO : BaseDTO
    {
        [ObservableProperty]
        private int _supplierid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Nhà cung cấp không được trống")]
        private string _suppliername;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Người đại diện được trống")]
        private string _representativesupplier;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Số điện thoại được trống")]
        [RegularExpression(@"^0[0-9]{9,10}$",
        ErrorMessage = "SĐT không hợp lệ.")]
        private string _phone;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Email được trống")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$",
        ErrorMessage = "Email không đúng định dạng")]
        private string _email;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Địa chỉ được trống")]
        private string _address;

        [ObservableProperty]
        private string _notes;

        [ObservableProperty]
        private ObservableCollection<ImportDTO> _imports = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _materialsuppliers = [];

        public SupplierDTO Clone()
        {
            return new SupplierDTO
            {
                Id = Id,
                Supplierid = Supplierid,
                Suppliername = Suppliername,
                Representativesupplier = Representativesupplier,
                Phone = Phone,
                Email = Email,
                Address = Address,
                Notes = Notes,
                Isdeleted = Isdeleted,

                Imports = Imports,
                Materialsuppliers = Materialsuppliers
            };
        }
    }
}