using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class CoffeetableDTO : BaseDTO
    {
        [ObservableProperty]
        private int coffeetableid;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số phải lớn hơn 0")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số bàn phải là số nguyên")]
        [NotifyPropertyChangedFor(nameof(TableName))]
        private int _tablenumber;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private int _seatingcapacity;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Không được trống")]
        private string _statustable;

        [ObservableProperty]
        private string _notes;

        public string TableName => $"Bàn {Tablenumber}";

        public CoffeetableDTO Clone()
        {
            return new CoffeetableDTO()
            {
                Id = Id,
                Coffeetableid = Coffeetableid,
                Tablenumber = Tablenumber,
                Seatingcapacity = Seatingcapacity,
                Statustable = Statustable,
                Notes = Notes,
                Isdeleted = Isdeleted,
            };
        }
    }
}