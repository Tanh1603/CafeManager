using CafeManager.Core.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class CoffeetableDTO : BaseDTO
    {
        [ObservableProperty]
        private int coffeetableid;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TableName))]
        private int _tablenumber;

        [ObservableProperty]
        private int _seatingcapacity;

        [ObservableProperty]
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