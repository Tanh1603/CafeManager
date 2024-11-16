using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace CafeManager.WPF.ViewModels.AddViewModel
{
    public partial class ModifyStaffViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly StaffServices _staffServices;

        [ObservableProperty]
        private bool _isPopupOpen = false;

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private bool _isAddingStaffSalaryHistory = false;

        [ObservableProperty]
        private bool _isUpdatingStaffSalaryHistory = false;

        [ObservableProperty]
        private StaffDTO _modifyStaff = new()
        {
            Startworkingdate = DateOnly.FromDateTime(DateTime.Now),
            Birthday = DateOnly.FromDateTime(DateTime.Now),
        };

        private List<int> sendDeletedStaffSalaryHistory = [];

        [ObservableProperty]
        private StaffsalaryhistoryDTO _currentStaffSalary = new() { Effectivedate = DateOnly.FromDateTime(DateTime.Now) };

        [ObservableProperty]
        private DateOnly? _endworkingdate;

        public ModifyStaffViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _staffServices = provider.GetRequiredService<StaffServices>();
            IsPopupOpen = false;
        }

        public void RecieveStaff(StaffDTO staff)
        {
            ModifyStaff = staff;
        }

        public void ClearValueOfViewModel()
        {
            IsAdding = false;
            IsUpdating = false;
            ModifyStaff = new()
            {
                Startworkingdate = DateOnly.FromDateTime(DateTime.Now),
                Birthday = DateOnly.FromDateTime(DateTime.Now),
            };
            CurrentStaffSalary = new() { Effectivedate = DateOnly.FromDateTime(DateTime.Now) };
            Endworkingdate = null;
        }

        [RelayCommand]
        private void OpenPopup_Add()
        {
            IsPopupOpen = true;
            IsAddingStaffSalaryHistory = true;
        }

        [RelayCommand]
        private void ModifyStaffSalaryHistory()
        {
            bool isSuitable = true;
            foreach (var history in ModifyStaff.Staffsalaryhistories)
            {
                if (CurrentStaffSalary.Staffsalaryhistoryid == 0)
                {
                    if (history.Effectivedate.Month == CurrentStaffSalary.Effectivedate.Month && history.Effectivedate.Year == CurrentStaffSalary.Effectivedate.Year)
                    {
                        isSuitable = false;
                        break;
                    }
                }
                else
                {
                    if (history.Staffsalaryhistoryid != CurrentStaffSalary.Staffsalaryhistoryid && history.Effectivedate.Month == CurrentStaffSalary.Effectivedate.Month && history.Effectivedate.Year == CurrentStaffSalary.Effectivedate.Year)
                    {
                        isSuitable = false;
                        break;
                    }
                }
            }
            if (IsAddingStaffSalaryHistory)
            {
                if (isSuitable)
                {
                    IsPopupOpen = false;
                    ModifyStaff.Staffsalaryhistories.Add(CurrentStaffSalary);
                    MyMessageBox.Show("Thêm lịch sử lương thành công");
                }
                else
                {
                    MyMessageBox.ShowDialog("Ngày hiệu lực thêm mới bị trùng.");
                }
                IsAddingStaffSalaryHistory = false;
            }

            if (IsUpdatingStaffSalaryHistory)
            {
                var find = ModifyStaff.Staffsalaryhistories.FirstOrDefault(x => x.Staffsalaryhistoryid == CurrentStaffSalary.Staffsalaryhistoryid);
                if (isSuitable && find != null)
                {
                    IsPopupOpen = false;
                    find.Effectivedate = CurrentStaffSalary.Effectivedate;
                    find.Salary = CurrentStaffSalary.Salary;
                    MyMessageBox.Show("Sửa lịch sử lương thành công");
                }
                else
                {
                    MyMessageBox.ShowDialog("Ngày hiệu lực sửa bị trùng.");
                }
                IsUpdatingStaffSalaryHistory = false;
            }
            CurrentStaffSalary = new() { Effectivedate = DateOnly.FromDateTime(DateTime.Now) };
        }

        [RelayCommand]
        private void DeleteStaffSalaryHistory(StaffsalaryhistoryDTO staffsalaryhistory)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa lịch sử lương này ko", MyMessageBox.Buttons.Yes_No_Cancel, MyMessageBox.Icons.Warning);
            if (res.Equals("1"))
            {
                ModifyStaff.Staffsalaryhistories.Remove(staffsalaryhistory);
                MyMessageBox.Show("Xóa lịch sử lương thành công");
            }
        }

        [RelayCommand]
        private void OpenPopup_Update(StaffsalaryhistoryDTO staffsalaryhistory)
        {
            IsPopupOpen = true;
            IsUpdatingStaffSalaryHistory = true;
            CurrentStaffSalary = new StaffsalaryhistoryDTO
            {
                Staffsalaryhistoryid = staffsalaryhistory.Staffsalaryhistoryid,
                Salary = staffsalaryhistory.Salary,
                Effectivedate = staffsalaryhistory.Effectivedate,
                Staffid = staffsalaryhistory.Staffid,
            };
        }

        public event Action<StaffDTO>? StaffChanged;

        [RelayCommand]
        private void SubmitModifyStaff()
        {
            //if (IsUpdating)
            //{
            //    StaffChanged?.Invoke(ModifyStaff.Clone());
            //    return;
            //}
            StaffChanged?.Invoke(ModifyStaff.Clone());
        }
    }
}