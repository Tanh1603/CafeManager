﻿using CafeManager.Core.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CafeManager.Core.DTOs
{
    public class CoffeetableDTO : INotifyPropertyChanged
    {
        private int coffeetableid;

        private int _tablenumber;

        private int? _seatingcapacity;

        private string _statustable;

        private string _notes;

        private bool? _isdeleted;

        public int Coffeetableid
        {
            get => coffeetableid;
            set
            {
                coffeetableid = value;
                OnPropertyChanged();
            }
        }

        public int? Seatingcapacity
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

        public bool? Isdeleted
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
                Coffeetableid = this.Coffeetableid,
                Tablenumber = this.Tablenumber,
                Seatingcapacity = this.Seatingcapacity,
                Statustable = this.Statustable,
                Notes = this.Notes,
                Isdeleted = this.Isdeleted,
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}