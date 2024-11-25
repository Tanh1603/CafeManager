using AutoMapper;
using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.MessageBox;
using CafeManager.WPF.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace CafeManager.WPF.ViewModels.AdminViewModel
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly IServiceProvider _provider;
        private readonly MaterialSupplierServices _materialSupplierServices;
        private readonly ConsumedMaterialServices _consumedMaterialServices;
        private readonly IMapper _mapper;

        [ObservableProperty]
        private bool _isPopupOpen = false;

        [ObservableProperty]
        private bool _isAdding = false;

        [ObservableProperty]
        private bool _isUpdating = false;

        private int _currentMaterialsupplierid = 0;

        [ObservableProperty]
        private ConsumedMaterialDTO _currentConsumedMaterial = new();

        [ObservableProperty]
        private ObservableCollection<ConsumedMaterialDTO> _listConsumedMaterialDTO = [];

        [ObservableProperty]
        private ObservableCollection<MaterialSupplierDTO> _listInventoryDTO = [];

        public InventoryViewModel(IServiceProvider provider)
        {
            _provider = provider;
            _materialSupplierServices = provider.GetRequiredService<MaterialSupplierServices>();
            _consumedMaterialServices = provider.GetRequiredService<ConsumedMaterialServices>();
            _mapper = provider.GetRequiredService<IMapper>();
            //Task.Run(LoadData);
            _ = LoadData();
        }

        private async Task LoadData()
        {
            var dbMaterialSupplier = await _materialSupplierServices.GetListMaterialSupplier();
            var dbConsumedMaterial = await _consumedMaterialServices.GetListConsumedMaterial();

            ListConsumedMaterialDTO = [.. _mapper.Map<List<ConsumedMaterialDTO>>(dbConsumedMaterial)];
            ListInventoryDTO = [.. _mapper.Map<List<MaterialSupplierDTO>>(dbMaterialSupplier)];

            var list = _mapper.Map<List<Consumedmaterial>>(ListConsumedMaterialDTO);
        }

        public void ClearValueOfPopupBox()
        {
            IsAdding = false;
            IsUpdating = false;
            CurrentConsumedMaterial = new();
        }

        [RelayCommand]
        private void OpenPopup_Add(MaterialSupplierDTO materialSupplierDTO)
        {
            _currentMaterialsupplierid = materialSupplierDTO.Materialsupplierid;
            IsPopupOpen = true;
            IsAdding = true;
        }
        [RelayCommand]
        private void OpenPopup_Update(ConsumedMaterialDTO consumedMaterialDTO)
        {
            _currentMaterialsupplierid = consumedMaterialDTO.Materialsupplierid ?? 0;
            IsPopupOpen = true;
            IsAdding = true;
        }

        [RelayCommand]
        private void ModifyStaffSalaryHistory()
        {
            if (IsAdding)
            {
                var existing = ListConsumedMaterialDTO.FirstOrDefault(x =>
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
    }
}