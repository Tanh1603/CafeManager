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

        public async Task<IEnumerable<Staff>> GetListStaffDeleted()
        {
            return await _unitOfWork.StaffList.GetAllStaffDeletedAsync();
        }

        public async Task<Staff?> GetStaffById(int id)
        {
            return await _unitOfWork.StaffList.GetStaffById(id);
        }

        public async Task<Staff?> CreateStaff(Staff staff, List<Staffsalaryhistory> staffsalaryhistories)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Staff res = await _unitOfWork.StaffList.Create(staff);
                await _unitOfWork.CompleteAsync();
                if (staffsalaryhistories != null)
                {
                    staffsalaryhistories.ForEach(x => x.Staffid = res.Staffid);
                    await _unitOfWork.StaffSalaryHistoryList.AddArange(staffsalaryhistories);
                    await _unitOfWork.CompleteAsync();
                }
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi thêm nhân viên");
            }
        }

        public async Task<Staff?> UpdateStaff(Staff staff, List<Staffsalaryhistory>? updateStaffSalaryHitory)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = _unitOfWork.StaffList.Update(staff);
                List<Staffsalaryhistory> existingHistories = (await _unitOfWork.StaffSalaryHistoryList.GetAll())
                                        .Where(x => x.Isdeleted == false && x.Staffid == staff.Staffid).ToList();
                if (updateStaffSalaryHitory != null && updateStaffSalaryHitory.Count > 0)
                {
                    var newHistoryIds = updateStaffSalaryHitory.Where(x => x.Staffsalaryhistoryid != 0)
                        .Select(x => x.Staffsalaryhistoryid).ToHashSet();
                    foreach (var existingHistory in existingHistories)
                    {
                        if (!newHistoryIds.Contains(existingHistory.Staffsalaryhistoryid))
                        {
                            existingHistory.Isdeleted = true;
                        }
                    }
                    foreach (var newHistory in updateStaffSalaryHitory)
                    {
                        if (newHistory.Staffsalaryhistoryid == 0)
                        {
                            await _unitOfWork.StaffSalaryHistoryList.Create(new()
                            {
                                Staffid = staff.Staffid,
                                Salary = newHistory.Salary,
                                Effectivedate = newHistory.Effectivedate,
                            });
                        }
                        else
                        {
                            var existingHistory = existingHistories
                                .FirstOrDefault(x => x.Isdeleted == false && x.Staffsalaryhistoryid == newHistory.Staffsalaryhistoryid);
                            if (existingHistory != null && existingHistory != newHistory)
                            {
                                existingHistory.Salary = newHistory.Salary;
                                existingHistory.Effectivedate = newHistory.Effectivedate;
                            }
                        }
                    }
                }
                else
                {
                    existingHistories.ForEach(x => x.Isdeleted = true);
                }
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi sửa nhân viên");
            }
        }

        public async Task<bool> DeleteStaff(int id, DateOnly? dateOnly)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var deletedStaff = await _unitOfWork.StaffList.GetStaffById(id);
                if (deletedStaff != null)
                {
                    deletedStaff.Endworkingdate = dateOnly;
                }
                bool res = await _unitOfWork.StaffList.Delete(id);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi xóa nhân viên");
            }
        }
    }
}