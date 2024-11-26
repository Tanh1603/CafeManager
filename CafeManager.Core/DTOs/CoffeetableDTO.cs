using CafeManager.Core.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class CoffeetableDTO : BaseDTO
    {
        private int coffeetableid;

        private int _tablenumber;

        private int _seatingcapacity;

        private string _statustable;

        private string _notes;

        private bool _isdeleted;

        public int Coffeetableid
        {
            get => coffeetableid;
            set
            {
                coffeetableid = value;
                OnPropertyChanged();
            }
        }

        public int Seatingcapacity
        {
            get => _seatingcapacity;
            set
            {
                _seatingcapacity = value;
                OnPropertyChanged();
            }
        }

        public string Statustable
        {
            get => _statustable; set
            {
                _statustable = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        public bool Isdeleted
        {
            get => _isdeleted;
            set
            {
                _isdeleted = value;
                OnPropertyChanged();
            }
        }

        public int Tablenumber
        {
            get => _tablenumber;
            set
            {
                _tablenumber = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TableName));
            }
        }

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