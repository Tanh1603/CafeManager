using AutoMapper;
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
        private readonly IMapper _mapper;

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
            _mapper = provider.GetRequiredService<IMapper>();
            Task.Run(LoadData);
        }

        private async void ModifyStaffVM_StaffChanged(StaffDTO staff)
        {
            try
            {
                if (ModifyStaffVM.IsAdding)
                {
                    Staff? addStaff =
                        await _staffServices.CreateStaff(_mapper.Map<Staff>(staff), _mapper.Map<List<Staffsalaryhistory>>(staff.Staffsalaryhistories));
                    if (addStaff != null)
                    {
                        ListStaff.Add(_mapper.Map<StaffDTO>(addStaff));
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

                            updateStaffSalaryHitory = _mapper.Map<List<Staffsalaryhistory>>(staff.Staffsalaryhistories);

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
            ListStaff = [.. _mapper.Map<List<StaffDTO>>(dbListStaff)];

            var dbListStaffDeleted = await _staffServices.GetListStaffDeleted();
            ListStaffDeleted = [.. _mapper.Map<List<StaffDTO>>(dbListStaffDeleted)];
            CurrentListStaff = [.. ListStaff];
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
            ModifyStaffVM.IsEnable = !IsListStaffDeleted;
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