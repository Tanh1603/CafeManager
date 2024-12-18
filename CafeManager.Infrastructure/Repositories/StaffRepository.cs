using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class StaffRepository(CafeManagerContext cafeManagerContext) : Repository<Staff>(cafeManagerContext), IStaffRepository
    {
        public async Task<Staff?> UpdateStaffWithListSatffSalaryHistory(Staff staff)
        {
            var update = await _cafeManagerContext.Staff.FindAsync(staff.Staffid);
            if (update != null)
            {
                // Cập nhật thông tin Staff
                _cafeManagerContext.Entry(update).CurrentValues.SetValues(staff);

                // Lấy danh sách Staffsalaryhistory hiện có trong cơ sở dữ liệu
                var existingStaffSalaryHistory = await _cafeManagerContext.Staffsalaryhistories
                    .Where(x => x.Isdeleted == false && x.Staffid == staff.Staffid).ToListAsync();

                // Phân loại các bản ghi mới
                var newEntities = staff.Staffsalaryhistories.Where(x => x.Staffsalaryhistoryid == 0).ToList();
                var updateEntities = staff.Staffsalaryhistories
                    .Where(x => x.Staffsalaryhistoryid != 0)
                    .ToDictionary(x => x.Staffsalaryhistoryid); // Tạo dictionary từ các bản ghi có ID

                // Cập nhật các bản ghi hiện có
                existingStaffSalaryHistory.ForEach(existingEntity =>
                {
                    if (updateEntities.TryGetValue(existingEntity.Staffsalaryhistoryid, out var newEntity))
                    {
                        // Cập nhật bản ghi nếu tìm thấy
                        _cafeManagerContext.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                        updateEntities.Remove(existingEntity.Staffsalaryhistoryid);
                    }
                });
                if (newEntities.Count != 0)
                {
                    await _cafeManagerContext.AddRangeAsync(newEntities);
                }
            }

            return update;
        }

        public async Task<int> GetStaffFromTo(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                DateOnly fromDO = new(from.Year, from.Month, from.Day);
                DateOnly toDO = new(to.Year, to.Month, to.Day);

                return await _cafeManagerContext.Staff.Where(x => x.Isdeleted == false && x.Startworkingdate >= fromDO && x.Startworkingdate <= toDO).CountAsync(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetTotalSalaryFromTo(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                DateOnly fromDO = new(from.Year, from.Month, from.Day);
                DateOnly toDO = new(to.Year, to.Month, to.Day);
                decimal sum = decimal.Zero;
                var list = await _cafeManagerContext.Staff.Where(x => x.Isdeleted == false && x.Startworkingdate >= fromDO && x.Startworkingdate <= toDO).ToListAsync(token);

                foreach (var staff in list)
                {
                    var salaryHistories = staff.Staffsalaryhistories
                            .OrderBy(h => h.Effectivedate)
                            .ToList();

                    var salariesInRange = salaryHistories.Where(h => h.Effectivedate >= fromDO && h.Effectivedate <= toDO).ToList();

                    if (salariesInRange.Any())
                    {
                        foreach (var salary in salariesInRange)
                        {
                            sum += salary.Salary;
                        }
                    }
                    else
                    {
                        var previousSalary = salaryHistories
                            .Where(h => h.Effectivedate < fromDO)
                            .OrderByDescending(h => h.Effectivedate)
                            .FirstOrDefault();

                        if (previousSalary != null)
                        {
                            // Tính số tháng từ from -> to
                            int monthsInRange = ((toDO.Year - fromDO.Year) * 12) + (toDO.Month - fromDO.Month) + 1;
                            sum += previousSalary.Salary * monthsInRange;
                        }
                    }
                }
                return sum;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}