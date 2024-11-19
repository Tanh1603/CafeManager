using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CafeManager.WPF.ViewModels.AddViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class StaffViewModel : ObservableObject, IDisposable
    {
        private readonly IServiceProvider _provider;
        private readonly StaffServices _staffServices;

        [ObservableProperty]
        private DateOnly? _endWorkingDate = new DateOnly?(DateOnly.FromDateTime(DateTime.Now));

        [ObservableProperty]
        private DateOnly _startWorkingDate = DateOnly.FromDateTime(DateTime.Now);

        [ObservableProperty]
        private bool _isOpenDeleteStaffView = false;

        [ObservableProperty]
        private bool _isOpenModifyStaffView;

        [ObservableProperty]
        private ModifyStaffViewModel _modifyStaffVM;

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaff = [];

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaffDeleted = [];

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _currentListStaff = [];

        private int _statusStaffIndex = 0;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenModifyStaffCommand))]
        private bool _isListStaffDeleted = false;

        public int StatusStaffIndex
        {
            get => _statusStaffIndex;
            set
            {
                _statusStaffIndex = value;
                if (value == 0)
                {
                    CurrentListStaff = [.. ListStaff];
                    IsListStaffDeleted = false;
                }
                else
                {
                    CurrentListStaff = [.. ListStaffDeleted];
                    IsListStaffDeleted = true;
                }
            }
        }

        public StaffViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _staffServices = provider.GetRequiredService<StaffServices>();
            ModifyStaffVM = provider.GetRequiredService<ModifyStaffViewModel>();
            ModifyStaffVM.StaffChanged += ModifyStaffVM_StaffChanged;
            IsOpenModifyStaffView = false;
            Task.Run(LoadData);
        }

        private async void ModifyStaffVM_StaffChanged(StaffDTO staff)
        {
            try
            {
                if (ModifyStaffVM.IsAdding)
                {
                    Staff? addStaff = await _staffServices.CreateStaff(StaffMapper.ToEntity(staff));
                    if (addStaff != null)
                    {
                        ListStaff.Add(StaffMapper.ToDTO(addStaff));
                        IsOpenModifyStaffView = false;
                        MyMessageBox.Show("Thêm nhân viên thành công");
                    }
                    else
                    {
                        MyMessageBox.Show("Thêm nhân viên thất bại công");
                    }
                }
                if (ModifyStaffVM.IsUpdating)
                {
                    var updateStaffDTO = ListStaff.FirstOrDefault(x => x.Staffid == staff.Staffid);
                    if (updateStaffDTO != null)
                    {
                        updateStaffDTO.Staffname = staff.Staffname;
                        updateStaffDTO.Phone = staff.Phone;
                        updateStaffDTO.Sex = staff.Sex;
                        updateStaffDTO.Birthday = staff.Birthday;
                        updateStaffDTO.Address = staff.Address;
                        updateStaffDTO.Startworkingdate = staff.Startworkingdate;
                        updateStaffDTO.Endworkingdate = staff.Endworkingdate;
                        updateStaffDTO.Role = staff.Role;
                        updateStaffDTO.Staffsalaryhistories = staff.Staffsalaryhistories;

                        Staff? updateStaff = await _staffServices.GetStaffById(updateStaffDTO.Staffid);
                        List<Staffsalaryhistory>? updateStaffSalaryHitory = [];
                        if (updateStaff != null)
                        {
                            updateStaff.Staffname = staff.Staffname;
                            updateStaff.Phone = staff.Phone;
                            updateStaff.Sex = staff.Sex;
                            updateStaff.Birthday = staff.Birthday;
                            updateStaff.Address = staff.Address;
                            updateStaff.Startworkingdate = staff.Startworkingdate;
                            updateStaff.Endworkingdate = staff.Endworkingdate;
                            updateStaff.Role = staff.Role;

                            updateStaffSalaryHitory
                                = staff.Staffsalaryhistories.Select(x => StaffSalaryHistoryMapper.ToEntity(x)).ToList();

                            await _staffServices.UpdateStaff(updateStaff, updateStaffSalaryHitory);
                        }
                        MyMessageBox.Show("Sửa thông nhân viên thành công");
                    }
                    else
                    {
                        MyMessageBox.Show("Sửa thông nhân viên thất bại");
                    }
                }
                IsOpenModifyStaffView = false;
                CurrentListStaff = [.. ListStaff];
                ModifyStaffVM.ClearValueOfViewModel();
            }
            catch (Exception)
            {
                MyMessageBox.Show("Lỗi");
            }
        }

        private async Task LoadData()
        {
            var dbListStaff = await _staffServices.GetListStaff();
            ListStaff = [.. dbListStaff.ToList().Select(x => StaffMapper.ToDTO(x))];
            var dbListStaffDeleted = await _staffServices.GetListStaffDeleted();
            ListStaffDeleted = [.. dbListStaffDeleted.ToList().Select(x => StaffMapper.ToDTO(x))];
            CurrentListStaff = [.. ListStaff];
        }

        [RelayCommand(CanExecute = nameof(CanExcuteOpenModifyStaff))]
        private void OpenModifyStaff(StaffDTO staffDTO)
        {
            IsOpenModifyStaffView = true;
            ModifyStaffVM.IsAdding = true;
            ModifyStaffVM.IsStaffDeleted = IsListStaffDeleted;
        }

        private bool CanExcuteOpenModifyStaff()
        {
            return IsListStaffDeleted == false;
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
            ModifyStaffVM.IsStaffDeleted = IsListStaffDeleted;
        }

        private StaffDTO TempDeleteStaff = new();

        [RelayCommand(CanExecute = nameof(CanExcuteOpenModifyStaff))]
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
                    var res = ListStaff.FirstOrDefault(x => x.Staffid == TempDeleteStaff.Staffid);
                    if (res != null)
                    {
                        ListStaff.Remove(res);
                    }
                    TempDeleteStaff.Endworkingdate = EndWorkingDate;
                    ListStaffDeleted.Add(TempDeleteStaff);
                    if (IsListStaffDeleted == false)
                    {
                        CurrentListStaff = [.. ListStaff];
                    }
                    else
                    {
                        CurrentListStaff = [.. ListStaffDeleted];
                    }
                    MyMessageBox.Show("Xóa nhân viên thành công (Nhân viên nghỉ việc)");
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

        [RelayCommand]
        private void CloseDeleteStaffView()
        {
            IsOpenDeleteStaffView = false;
            EndWorkingDate = null;
        }

        public void Dispose()
        {
            ModifyStaffVM.StaffChanged -= ModifyStaffVM_StaffChanged;
        }
    }
}