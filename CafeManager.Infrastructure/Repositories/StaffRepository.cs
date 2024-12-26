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

        public async Task<Dictionary<DateOnly, decimal>> GetTotalSalaryByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                DateOnly fromDO = new(from.Year, from.Month, 1); // Bắt đầu từ đầu tháng
                DateOnly toDO = new(to.Year, to.Month, DateTime.DaysInMonth(to.Year, to.Month)); // Cuối tháng của tháng cuối cùng

                // Khởi tạo kết quả
                var salaryByMonth = new Dictionary<DateOnly, decimal>();

                // Lấy danh sách nhân viên thỏa mãn điều kiện
                var staffList = await _cafeManagerContext.Staff
                    .Where(x => x.Isdeleted == false && x.Startworkingdate <= toDO)
                    .Include(s => s.Staffsalaryhistories) // Tải lịch sử lương
                    .ToListAsync(token);

                // Duyệt từng tháng trong khoảng thời gian
                for (var month = fromDO; month <= toDO; month = month.AddMonths(1))
                {
                    decimal monthlySum = 0;

                    foreach (var staff in staffList)
                    {
                        // Lấy lịch sử lương của nhân viên
                        var salaryHistories = staff.Staffsalaryhistories
                            .OrderBy(h => h.Effectivedate)
                            .ToList();

                        // Kiểm tra nếu có lương trong tháng hiện tại
                        var currentSalary = salaryHistories
                            .Where(h =>h.Isdeleted == false && h.Effectivedate.Month <= month.Month && h.Effectivedate.Year <= month.Year)
                            .OrderByDescending(h => h.Effectivedate)
                            .FirstOrDefault();
                        

                        if (currentSalary != null)
                        {
                            monthlySum += currentSalary.Salary;
                        }
                        else
                        {
                            // Nếu không có lương trong tháng, lấy mức lương của tháng trước đó
                            var previousSalary = salaryHistories
                                .Where(h => h.Effectivedate < month)
                                .OrderByDescending(h => h.Effectivedate)
                                .FirstOrDefault();

                            if (previousSalary != null)
                            {
                                // Thêm lương của tháng trước đó vào tổng
                                monthlySum += previousSalary.Salary;
                            }
                        }
                    }

                    // Thêm kết quả cho từng tháng vào dictionary
                    salaryByMonth[month] = monthlySum;
                }

                return salaryByMonth;
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