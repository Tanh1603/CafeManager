using AutoMapper;
using CafeManager.Core.DTOs;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

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
        private bool _isStaffDeleted = false;

        [ObservableProperty]
        private bool _isEnable = true;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSubmit))]
        [NotifyCanExecuteChangedFor(nameof(SubmitModifyStaffCommand))]
        private StaffDTO _modifyStaff = new();

        [ObservableProperty]
        private StaffsalaryhistoryDTO _currentStaffSalary = new();

        public ObservableCollection<StaffsalaryhistoryDTO> ListExisted
            => [.. ModifyStaff.Staffsalaryhistories.Where(x => x.Isdeleted == false)];

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
            OnPropertyChanged(nameof(ListExisted));
        }

        public void ClearValueOfViewModel()
        {
            IsAdding = false;
            IsUpdating = false;
            ModifyStaff = new();
            CurrentStaffSalary = new();
            Endworkingdate = null;
            OnPropertyChanged(nameof(ListExisted));
        }

        [RelayCommand]
        private void DeleteStaffSalaryHistory(StaffsalaryhistoryDTO staffsalaryhistory)
        {
            var res = MyMessageBox.ShowDialog("Bạn có muốn xóa lịch sử lương này ko", MyMessageBox.Buttons.Yes_No, MyMessageBox.Icons.Warning);
            if (res.Equals("1") && res != null)
            {
                var deleted = ModifyStaff.Staffsalaryhistories.FirstOrDefault(x => x.Isdeleted == false && x.Id == staffsalaryhistory.Id);
                if (deleted != null)
                {
                    deleted.Isdeleted = true;
                    MyMessageBox.ShowDialog("Xóa lịch sử lương thành công");
                    OnPropertyChanged(nameof(ModifyStaff));
                    OnPropertyChanged(nameof(ListExisted));
                }
            }
        }

        [RelayCommand]
        private void OpenUpdateStaffSalaryHistory(StaffsalaryhistoryDTO staffsalaryhistoryDTO)
        {
            IsOpenModifySalary = true;
            CurrentStaffSalary = staffsalaryhistoryDTO.Clone();
        }

        [RelayCommand]
        private void OpenModifyStaffSalaryHistory()
        {
            IsOpenModifySalary = true;
        }

        [RelayCommand]
        private void SubmitModifySalaryHistory()
        {
            if (ModifyStaff.Startworkingdate > CurrentStaffSalary.Effectivedate)
            {
                MyMessageBox.ShowDialog("Ngày bắt đầu của lịch sử lương không thể nhỏ hơn ngày vào làm.");
                return;
            }

            var existedHistory = ModifyStaff.Staffsalaryhistories
                .FirstOrDefault(x => x.Staffsalaryhistoryid == CurrentStaffSalary.Staffsalaryhistoryid || (x.Isdeleted == false &&
                    x.Effectivedate.Month == CurrentStaffSalary.Effectivedate.Month && x.Effectivedate.Year == CurrentStaffSalary.Effectivedate.Year));
            if (existedHistory != null)
            {
                var duplicated = ModifyStaff.Staffsalaryhistories.FirstOrDefault(x => x.Id != CurrentStaffSalary.Id && x.Effectivedate.Month == CurrentStaffSalary.Effectivedate.Month && x.Effectivedate.Year == CurrentStaffSalary.Effectivedate.Year && x.Isdeleted == false);

                if (duplicated != null)
                {
                    MyMessageBox.ShowDialog("Lịch sử lương bị trùng tháng năm với đã có");
                }
                else
                {
                    MyMessageBox.ShowDialog("Sửa lịch sử lương thành công");
                    existedHistory.Salary = CurrentStaffSalary.Salary;
                    existedHistory.Effectivedate = CurrentStaffSalary.Effectivedate;
                }
            }
            else
            {
                MyMessageBox.ShowDialog("Thêm lịch sử lương thành công");
                ModifyStaff.Staffsalaryhistories.Add(CurrentStaffSalary);
            }
            IsOpenModifySalary = false;
            CurrentStaffSalary = new();
            OnPropertyChanged(nameof(ListExisted));
        }

        [RelayCommand]
        private void CloseModifySalaryHistory()
        {
            IsOpenModifySalary = false;
            CurrentStaffSalary = new();
        }

        public event Action<StaffDTO>? StaffChanged;

        public bool CanSubmit => !ModifyStaff.GetErrors().Any();

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private void SubmitModifyStaff()
        {
            if (IsAdding)
            {
                ModifyStaff.Staffsalaryhistories = [.. ModifyStaff.Staffsalaryhistories.Where(x => x.Isdeleted == false)];
                StaffChanged?.Invoke(ModifyStaff.Clone());
            }
            if (IsUpdating)
            {
                ModifyStaff.Staffsalaryhistories.ToList().ForEach(x => x.Staffid = ModifyStaff.Staffid);
                StaffChanged?.Invoke(ModifyStaff.Clone());
            }
        }
    }
}