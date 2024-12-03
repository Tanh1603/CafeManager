﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public class AppUserDTO : BaseDTO
    {
        private int _appUserId;
        private string _username;
        private string _displayName;
        private string _email;
        private string _role;
        private BitmapImage _avatar;
        private bool _isDeleted;

        public int Appuserid
        {
            get => _appUserId;
            set
            {
                if (_appUserId != value)
                {
                    _appUserId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Displayname
        {
            get => _displayName;
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                if (_role != value)
                {
                    _role = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage Avatar
        {
            get => _avatar;
            set
            {
                if (_avatar != value)
                {
                    _avatar = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Isdeleted
        {
            get => _isDeleted;
            set
            {
                if (_isDeleted != value)
                {
                    _isDeleted = value;
                    OnPropertyChanged();
                }
            }
        }

        public AppUserDTO Clone()
        {
            return new AppUserDTO
            {
                Id = Id,
                Appuserid = Appuserid,
                Username = Username,
                Displayname = Displayname,
                Email = Email,
                Role = Role,
                Avatar = Avatar,
                Isdeleted = Isdeleted
            };
        }
    }
}