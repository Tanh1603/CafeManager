using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class StaffViewModel : ObservableObject, IDisposable, IDataViewModel
    {
        private readonly StaffServices _staffServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private DateOnly? _endWorkingDate = new DateOnly?(DateOnly.FromDateTime(DateTime.Now));

        [ObservableProperty]
        private DateOnly _startWorkingDate = DateOnly.FromDateTime(DateTime.Now);

        [ObservableProperty]
        private bool _isOpenDeleteStaffView = false;

        [ObservableProperty]
        private bool _isOpenModifyStaffView = false;

        [ObservableProperty]
        private ModifyStaffViewModel _modifyStaffVM;

        public ObservableCollection<StaffDTO> ListExistedStaff => [.. _filterListStaff?.Where(x => x.Isdeleted == false) ?? []];

        public ObservableCollection<StaffDTO> ListDeletedStaff => [.. _filterListStaff?.Where(x => x.Isdeleted != false) ?? []];

        private List<StaffDTO> AllStaff = [];
        private List<StaffDTO> _filterListStaff = [];
        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText; set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    FilterStaff();
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public StaffViewModel(IServiceScope scope)
        {
            var provider = scope.ServiceProvider;
            _staffServices = provider.GetRequiredService<StaffServices>();
            ModifyStaffVM = provider.GetRequiredService<ModifyStaffViewModel>();
            ModifyStaffVM.StaffChanged += ModifyStaffVM_StaffChanged;
            _mapper = provider.GetRequiredService<IMapper>();
        }

        public async Task LoadData(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var dbListStaff = await _staffServices.GetListStaff(token);
                AllStaff = new List<StaffDTO>(_mapper.Map<List<StaffDTO>>(dbListStaff));
                _filterListStaff = [.. AllStaff];
                OnPropertyChanged(nameof(ListExistedStaff));
                OnPropertyChanged(nameof(ListDeletedStaff));
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadData của StaffViewModel bị hủy");
            }
        }

        private void FilterStaff()
        {
            var filter = AllStaff.Where(x => string.IsNullOrWhiteSpace(SearchText)
            || x.Staffname.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            || x.Phone.Contains(SearchText) || x.Address.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            || x.Role.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();

            _filterListStaff = [.. filter];
            OnPropertyChanged(nameof(ListExistedStaff));
            OnPropertyChanged(nameof(ListDeletedStaff));
        }

        [RelayCommand]
        private void OpenModifyStaff(StaffDTO staffDTO)
        {
            IsOpenModifyStaffView = true;
            ModifyStaffVM.IsAdding = true;
            ModifyStaffVM.IsEnable = true;
        }

        [RelayCommand]
        private void CloseModifyStaff()
        {
            IsOpenModifyStaffView = false;
            ModifyStaffVM.ClearValueOfViewModel();
        }

        [RelayCommand]
        private void OpenUpdateStaff(StaffDTO staffDTO)
        {
            ModifyStaffVM.RecieveStaff(staffDTO.Clone());
            IsOpenModifyStaffView = true;
            ModifyStaffVM.IsUpdating = true;
            ModifyStaffVM.IsEnable = true;
        }

        [RelayCommand]
        private void OpenInfoStaffView(StaffDTO staffDTO)
        {
            ModifyStaffVM.RecieveStaff(staffDTO.Clone());
            IsOpenModifyStaffView = true;
            ModifyStaffVM.IsEnable = false;
        }

        private StaffDTO TempDeleteStaff = new();

        [RelayCommand]
        private void OpenDeleteStaffView(StaffDTO staffDTO)
        {
            IsOpenDeleteStaffView = true;
            StartWorkingDate = staffDTO.Startworkingdate;
            TempDeleteStaff = staffDTO;
        }

        [RelayCommand]
        private async Task SubmitDeleteStaff()
        {
            try
            {
                if (TempDeleteStaff.Startworkingdate >= EndWorkingDate)
                {
                    MyMessageBox.Show("Ngày nghỉ việc phải lớn hơn ngày vào làm");
                    return;
                }
                bool isSuccessDelete = await _staffServices.DeleteStaff(TempDeleteStaff.Staffid, EndWorkingDate);
                if (isSuccessDelete)
                {
                    var res = AllStaff.FirstOrDefault(x => x.Staffid == TempDeleteStaff.Staffid);
                    if (res != null)
                    {
                        res.Isdeleted = true;
                        res.Endworkingdate = EndWorkingDate;
                        MyMessageBox.Show("Xóa nhân viên thành công (Nhân viên nghỉ việc)");
                        FilterStaff();
                    }
                }
                else
                {
                    MyMessageBox.Show("Xóa nhân viên thất bại (Nhân viên nghỉ việc)");
                }
                IsOpenDeleteStaffView = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void ModifyStaffVM_StaffChanged(StaffDTO staff)
        {
            try
            {
                if (ModifyStaffVM.IsAdding)
                {
                    Staff? addStaff =
                        await _staffServices.CreateStaff(_mapper.Map<Staff>(staff));
                    if (addStaff != null)
                    {
                        AllStaff.Add(_mapper.Map<StaffDTO>(addStaff));
                        MyMessageBox.Show("Thêm nhân viên thành công");
                        IsOpenModifyStaffView = false;
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm nhân viên thất bại công");
                    }
                }
                if (ModifyStaffVM.IsUpdating)
                {
                    var res = await _staffServices.UpdateStaff(_mapper.Map<Staff>(staff));
                    if (res != null)
                    {
                        var updateDTO = AllStaff.FirstOrDefault(x => x.Staffid == staff.Staffid);
                        _mapper.Map(res, updateDTO);
                        MyMessageBox.ShowDialog("Sửa nhân viên thành công");
                    }
                    else
                    {
                        MyMessageBox.ShowDialog("Sửa nhân viên thất bại");
                    }
                }
                IsOpenModifyStaffView = false;
                ModifyStaffVM.ClearValueOfViewModel();
                FilterStaff();
            }
            catch (InvalidOperationException ioe)
            {
                MyMessageBox.ShowDialog(ioe.Message);
            }
        }

        [RelayCommand]
        private void CloseDeleteStaffView()
        {
            IsOpenDeleteStaffView = false;
            EndWorkingDate = null;
        }

        public void Dispose()
        {
            ModifyStaffVM.StaffChanged -= ModifyStaffVM_StaffChanged;
            GC.SuppressFinalize(this);
        }
    }
}