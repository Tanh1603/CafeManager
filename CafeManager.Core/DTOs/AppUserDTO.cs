using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

#nullable disable

namespace CafeManager.Core.DTOs
{
    public partial class AppUserDTO : BaseDTO
    {
        [ObservableProperty]
        private int _appuserid;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _displayname;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _role;

        [ObservableProperty]
        private BitmapImage? _avatar;

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
                Password = Password,
                Avatar = Avatar,
                Isdeleted = Isdeleted
            };
        }
    }
}