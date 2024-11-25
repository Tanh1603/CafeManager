using AutoMapper;
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
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isOpenModifySalary = false;

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        [ObservableProperty]
        private bool _isAddingStaffSalaryHistory = false;

        [ObservableProperty]
        private bool _isUpdatingStaffSalaryHistory = false;

        [ObservableProperty]
        private bool _isStaffDeleted = false;

        [ObservableProperty]
        private bool _isEnable = true;

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
            _mapper = provider.GetRequiredService<IMapper>();
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
                Birthday = DateOnly.FromDateTime(DateTime.Now)
            };
            CurrentStaffSalary = new() { Effectivedate = DateOnly.FromDateTime(DateTime.Now) };
            Endworkingdate = null;
        }

        [RelayCommand]
        private void DeleteStaffSalaryHistory(StaffsalaryhistoryDTO staffsalaryhistory)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa lịch sử lương này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
            if (res.Equals("1") && res != null)
            {
                ModifyStaff.Staffsalaryhistories.Remove(staffsalaryhistory);
                MyMessageBox.Show("Xóa lịch sử lương thành công");
            }
        }

        public event Action<StaffDTO>? StaffChanged;

        [RelayCommand]
        private void SubmitModifyStaff()
        {
            StaffChanged?.Invoke(ModifyStaff.Clone());
        }

        [RelayCommand]
        private void OpenUpdateStaffSalaryHistory(StaffsalaryhistoryDTO staffsalaryhistoryDTO)
        {
            IsOpenModifySalary = true;
            IsUpdatingStaffSalaryHistory = true;
            CurrentStaffSalary = staffsalaryhistoryDTO.Clone();
            _currentStaffSalaryIndex = ModifyStaff.Staffsalaryhistories.IndexOf(staffsalaryhistoryDTO);
        }

        [RelayCommand]
        private void OpenModifyStaffSalaryHistory()
        {
            IsOpenModifySalary = true;
            IsAddingStaffSalaryHistory = true;
        }

        [RelayCommand]
        private void SubmitModifySalaryHistory()
        {
            if (IsAddingStaffSalaryHistory)
            {
                var existing = ModifyStaff.Staffsalaryhistories.FirstOrDefault(x =>
                    x.Effectivedate.Month == CurrentStaffSalary.Effectivedate.Month &&
                    x.Effectivedate.Year == CurrentStaffSalary.Effectivedate.Year);

                if (existing != null)
                {
                    MyMessageBox.ShowDialog("Ngày hiệu lực thêm mới bị trùng.");
                    IsAddingStaffSalaryHistory = true;
                    return;
                }
                else
                {
                    ModifyStaff.Staffsalaryhistories.Add(CurrentStaffSalary);
                    _currentStaffSalaryIndex = ModifyStaff.Staffsalaryhistories.IndexOf(CurrentStaffSalary);
                    MyMessageBox.Show("Thêm lịch sử lương thành công.");
                    IsAddingStaffSalaryHistory = false;
                }
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
                    IsUpdatingStaffSalaryHistory = false;
                }
                else
                {
                    IsUpdatingStaffSalaryHistory = true;
                    return;
                }
            }

            // Reset lại `CurrentStaffSalary` sau khi thêm/sửa
            CurrentStaffSalary = new()
            {
                Effectivedate = DateOnly.FromDateTime(DateTime.Now)
            };
            IsOpenModifySalary = false;
        }

        [RelayCommand]
        private void CloseModifySalaryHistory()
        {
            IsOpenModifySalary = false;
            CurrentStaffSalary = new()
            {
                Effectivedate = DateOnly.FromDateTime(DateTime.Now)
            };
        }
    }
}