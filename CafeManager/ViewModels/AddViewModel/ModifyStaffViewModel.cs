using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

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
        [NotifyCanExecuteChangedFor(nameof(OpenPopup_AddCommand))]
        [NotifyCanExecuteChangedFor(nameof(OpenPopup_UpdateCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteStaffSalaryHistoryCommand))]
        [NotifyCanExecuteChangedFor(nameof(SubmitModifyStaffCommand))]
        [NotifyPropertyChangedFor(nameof(IsEnable))]
        private bool _isStaffDeleted = false;

        public bool IsEnable => !IsStaffDeleted;

        [ObservableProperty]
        private StaffDTO _modifyStaff = new()
        {
            Startworkingdate = DateOnly.FromDateTime(DateTime.Now),
            Birthday = DateOnly.FromDateTime(DateTime.Now),
        };

        [ObservableProperty]
        private StaffsalaryhistoryDTO _currentStaffSalary = new() { Effectivedate = DateOnly.FromDateTime(DateTime.Now) };

        private int _currentStaffSalaryIndex = -1;

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
            Endworkingdate = ModifyStaff.Endworkingdate;
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

        [RelayCommand(CanExecute = nameof(CanexcuteOpenPopUpBox))]
        private void OpenPopup_Add()
        {
            IsPopupOpen = true;
            IsAddingStaffSalaryHistory = true;
        }

        private bool CanexcuteOpenPopUpBox()
        {
            return IsStaffDeleted == false;
        }

        [RelayCommand]
        private void ModifyStaffSalaryHistory()
        {
            if (IsAddingStaffSalaryHistory)
            {
                var existing = ModifyStaff.Staffsalaryhistories.FirstOrDefault(x =>
                    x.Effectivedate.Month == CurrentStaffSalary.Effectivedate.Month &&
                    x.Effectivedate.Year == CurrentStaffSalary.Effectivedate.Year);

                if (existing != null)
                {
                    MyMessageBox.ShowDialog("Ngày hiệu lực thêm mới bị trùng.");
                }
                else
                {
                    ModifyStaff.Staffsalaryhistories.Add(CurrentStaffSalary);
                    _currentStaffSalaryIndex = ModifyStaff.Staffsalaryhistories.IndexOf(CurrentStaffSalary);
                    MyMessageBox.Show("Thêm lịch sử lương thành công.");
                }

                IsAddingStaffSalaryHistory = false;
            }

            if (IsUpdatingStaffSalaryHistory)
            {
                bool ok = false;

                for (int i = 0; i < ModifyStaff.Staffsalaryhistories.Count; i++)
                {
                    StaffsalaryhistoryDTO staffsalaryhistory = ModifyStaff.Staffsalaryhistories[i];
                    if (i != _currentStaffSalaryIndex && CurrentStaffSalary.Effectivedate.Month == staffsalaryhistory.Effectivedate.Month && CurrentStaffSalary.Effectivedate.Year == staffsalaryhistory.Effectivedate.Year)
                    {
                        MyMessageBox.Show("Ngày hiệu lực thay đổi bị trùng");
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    ModifyStaff.Staffsalaryhistories[_currentStaffSalaryIndex].Effectivedate = CurrentStaffSalary.Effectivedate;
                    ModifyStaff.Staffsalaryhistories[_currentStaffSalaryIndex].Salary = CurrentStaffSalary.Salary;
                    MyMessageBox.Show("Sửa lịch sử lương thành công.");
                }
                IsUpdatingStaffSalaryHistory = false;
            }

            // Reset lại `CurrentStaffSalary` sau khi thêm/sửa
            CurrentStaffSalary = new()
            {
                Effectivedate = DateOnly.FromDateTime(DateTime.Now)
            };
            IsPopupOpen = false;
        }

        [RelayCommand(CanExecute = nameof(CanexcuteOpenPopUpBox))]
        private void DeleteStaffSalaryHistory(StaffsalaryhistoryDTO staffsalaryhistory)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa lịch sử lương này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
            if (res.Equals("1") && res != null)
            {
                ModifyStaff.Staffsalaryhistories.Remove(staffsalaryhistory);
                MyMessageBox.Show("Xóa lịch sử lương thành công");
            }
        }

        [RelayCommand(CanExecute = nameof(CanexcuteOpenPopUpBox))]
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
            _currentStaffSalaryIndex = ModifyStaff.Staffsalaryhistories.IndexOf(staffsalaryhistory);
        }

        public event Action<StaffDTO>? StaffChanged;

        [RelayCommand(CanExecute = nameof(CanexcuteOpenPopUpBox))]
        private void SubmitModifyStaff()
        {
            StaffChanged?.Invoke(ModifyStaff.Clone());
        }
    }
}