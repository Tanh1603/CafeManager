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
        private bool _isOpenModifyStaffView;

        [ObservableProperty]
        private ModifyStaffViewModel _modifyStaffVM;

        [ObservableProperty]
        private ObservableCollection<StaffDTO> _listStaff = [];

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
        }

        [RelayCommand]
        private void OpenModifyStaff()
        {
            IsOpenModifyStaffView = true;
            ModifyStaffVM.IsAdding = true;
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
        }

        [RelayCommand]
        private void DeleteStaff(StaffDTO staff)
        {
        }

        public void Dispose()
        {
            ModifyStaffVM.StaffChanged -= ModifyStaffVM_StaffChanged;
        }
    }
}