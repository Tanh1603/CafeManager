using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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

        public async Task<List<decimal>> GetTotalSalaryByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
               
                DateOnly fromDO = new(from.Year, from.Month, from.Day);
                DateOnly toDO = new(to.Year, to.Month, to.Day);

                // Lấy tất cả lịch sử lương trong khoảng thời gian từ from đến to
                var salaryHistory = await _cafeManagerContext.Staffsalaryhistories
                    .Where(sh => sh.Effectivedate >= fromDO && sh.Effectivedate <= toDO && sh.Isdeleted == false)
                    .ToListAsync(token);

                // Tạo danh sách tất cả các tháng trong khoảng thời gian từ from đến to
                var allMonths = Enumerable.Range(0, ((toDO.Year - fromDO.Year) * 12 + toDO.Month - fromDO.Month) + 1)
                                          .Select(i => new DateTime(from.Year, from.Month, 1).AddMonths(i))
                                          .ToList();

                // Group by tháng và tính tổng lương cho mỗi tháng
                var salaryGroupedByMonth = salaryHistory
                    .GroupBy(sh => new DateTime(sh.Effectivedate.Year, sh.Effectivedate.Month, 1))  // Group by Year-Month
                    .ToDictionary(
                        g => g.Key, // Key is the month (DateTime representing the 1st day of the month)
                        g => g.Sum(sh => sh.Salary) // Total salary for each month
                    );

                // Lấy tổng lương cho mỗi tháng, nếu không có thì trả về 0
                var salaryList = allMonths.Select(month =>
                {
                    return salaryGroupedByMonth.ContainsKey(month) ? salaryGroupedByMonth[month] : 0;
                }).ToList();

                return salaryList;
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

        public async Task<List<decimal>> GetTotalSalaryByYear(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                DateOnly fromDO = new(from.Year, from.Month, from.Day);
                DateOnly toDO = new(to.Year, to.Month, to.Day);

                // Lấy tất cả lịch sử lương trong khoảng thời gian từ from đến to
                var salaryHistory = await _cafeManagerContext.Staffsalaryhistories
                    .Where(sh => sh.Effectivedate >= fromDO && sh.Effectivedate <= toDO && sh.Isdeleted == false)
                    .ToListAsync(token);

                // Tạo danh sách tất cả các năm trong khoảng thời gian từ from đến to
                var allYears = Enumerable.Range(toDO.Year, to.Year - fromDO.Year + 1).ToList();

                // Group by năm và tính tổng lương cho mỗi năm
                var salaryGroupedByYear = salaryHistory
                    .GroupBy(sh => new DateTime(sh.Effectivedate.Year, 1, 1))  // Group by Year
                    .ToDictionary(
                        g => g.Key.Year, // Key is the year
                        g => g.Sum(sh => sh.Salary) // Total salary for each year
                    );

                // Lấy tổng lương cho mỗi năm, nếu không có thì trả về 0
                var salaryList = allYears.Select(year =>
                {
                    return salaryGroupedByYear.ContainsKey(year) ? salaryGroupedByYear[year] : 0;
                }).ToList();

                return salaryList;
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