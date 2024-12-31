using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.Stores;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class AppUserViewModel : ObservableObject, IDataViewModel
    {
        private readonly AppUserServices _appUserServices;
        private readonly EncryptionHelper _encryptionHelper;
        private CancellationToken _token = default;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<AppUserDTO> _listAppUserDTO = [];

        [ObservableProperty]
        private bool _isOpenUpdateAppUser;

        [ObservableProperty]
        private UpdateAppUserViewModel _updateAppUserVM;

        public AppUserViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _appUserServices = provider.GetRequiredService<AppUserServices>();
            _updateAppUserVM = provider.GetRequiredService<UpdateAppUserViewModel>();
            _encryptionHelper = provider.GetRequiredService<EncryptionHelper>();
            _mapper = provider.GetRequiredService<IMapper>();

            UpdateAppUserVM.ModifyAppUserChanged += UpdateAppUserVM_ModifyAppUserChanged;
            IsOpenUpdateAppUser = false;
        }

        public async Task LoadData(CancellationToken token = default)
        {
            if (token == default)
            {
                _token = token;
            }
            try
            {
                token.ThrowIfCancellationRequested();
                IsLoading = true;
                Expression<Func<Appuser, bool>> filter = appuser =>
                    (appuser.Isdeleted == false) &&
                    (appuser.Role == 0);
                var dbListAppuser = await _appUserServices.GetSearchPaginateListAppuser(filter, pageIndex, pageSize);
                ListAppUserDTO = [.. _mapper.Map<List<AppUserDTO>>(dbListAppuser.Item1)];
                DecryptPassword();
                totalPages = (dbListAppuser.Item2 + pageSize - 1) / pageSize;
                OnPropertyChanged(nameof(PageUI));
                IsLoading = false;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của Appuser bị hủy");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void UpdateAppUserVM_ModifyAppUserChanged(AppUserDTO obj)
        {
            try
            {
                IsLoading = true;
                obj.Password = _encryptionHelper.EncryptAES(obj.Password);
                Appuser? update = await _appUserServices.UpdateAppUser(_mapper.Map<Appuser>(obj));
                if (update != null)
                {
                    _mapper.Map(update, ListAppUserDTO.FirstOrDefault(x => x.Appuserid == obj.Appuserid));
                    DecryptPassword();
                    IsOpenUpdateAppUser = false;
                    IsLoading = false;
                    MyMessageBox.Show("Sửa tài khoản thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DecryptPassword()
        {
            foreach (var item in ListAppUserDTO)
            {
                item.Password = _encryptionHelper.DecryptAES(item.Password ?? string.Empty);
            }
        }

        [RelayCommand]
        private void OpenUpdateAppUser(AppUserDTO appUser)
        {
            UpdateAppUserVM.ReceiveAccount(appUser.Clone());
            IsOpenUpdateAppUser = true;
        }

        [RelayCommand]
        private void CloseUserControl()
        {
            IsOpenUpdateAppUser = false;
        }

        [RelayCommand]
        private async Task DeleteAppUser(AppUserDTO appUser)
        {
            try
            {
                var res = MyMessageBox.ShowDialog("Bạn có muốn xóa tài khoản này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
                if (res.Equals("1"))
                {
                    //IsLoading = true;
                    var isDeleted = await _appUserServices.DeleteAppUser(appUser.Appuserid);
                    if (isDeleted)
                    {
                        ListAppUserDTO.Remove(appUser);
                    }
                    MyMessageBox.Show("Xoá tài khoản thành công", MyMessageBox.Buttons.OK, MyMessageBox.Icons.Information);
                }
            }
            catch (InvalidOperationException ivd)
            {
                MyMessageBox.Show(ivd.Message, MyMessageBox.Buttons.OK, MyMessageBox.Icons.Warning);
            }
            //await LoadData();
            //IsLoading = false;
        }

        public void Dispose()
        {
            UpdateAppUserVM.ModifyAppUserChanged -= UpdateAppUserVM_ModifyAppUserChanged;
            GC.SuppressFinalize(this);
        }

        #region Phan trang

        private int pageIndex = 1;

        private int pageSize = 10;
        private int totalPages = 0;

        public string PageUI => $"{pageIndex}/{totalPages}";

        [RelayCommand]
        private async Task FirstPage()
        {
            pageIndex = 1;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task NextPage()
        {
            if (pageIndex == totalPages)
            {
                return;
            }
            pageIndex += 1;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task PreviousPage()
        {
            if (pageIndex == 1)
            {
                return;
            }
            pageIndex -= 1;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task LastPage()
        {
            pageIndex = totalPages;
            IsLoading = true;
            await LoadData();
            IsLoading = false;
        }

        #endregion Phan trang
    }
}