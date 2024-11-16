using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class StaffServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public StaffServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Staff>> GetListStaff()
        {
            return await _unitOfWork.StaffList.GetAllStaffAsync();
        }

        public async Task<Staff?> GetStaffById(int id)
        {
            return await _unitOfWork.StaffList.GetStaffById(id);
        }

        public async Task<Staff?> CreateStaff(Staff staff)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Staff res = await _unitOfWork.StaffList.Create(staff);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi khi thêm nhân viên");
            }
        }

        public async Task<Staff?> UpdateStaff(Staff staff, List<Staffsalaryhistory>? updateStaffSalaryHitory)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = _unitOfWork.StaffList.Update(staff);
                if (updateStaffSalaryHitory != null)
                {
                    updateStaffSalaryHitory.ForEach(async x =>
                    {
                        if (x.Staffsalaryhistoryid == 0)
                        {
                            await _unitOfWork.StaffSalaryHistoryList.Create(new()
                            {
                                Staffid = staff.Staffid,
                                Salary = x.Salary,
                                Effectivedate = x.Effectivedate,
                            });
                        }
                        else
                        {
                            var existingHistory = await _unitOfWork.StaffSalaryHistoryList.GetById(x.Staffsalaryhistoryid);

                            if (existingHistory != null && existingHistory.Isdeleted == false)
                            {
                                if (x.Isdeleted == true)
                                {
                                    existingHistory.Isdeleted = true;
                                }
                                else
                                {
                                    existingHistory.Salary = x.Salary;
                                    existingHistory.Effectivedate = x.Effectivedate;
                                }
                            }
                        }
                    });
                }
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi khi sửa nhân viên");
            }
        }

        public async Task<bool> DeleteStaff(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                bool res = await _unitOfWork.StaffList.Delete(id);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi khi xóa nhân viên");
            }
        }
    }
}