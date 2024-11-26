using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

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
                    else
                    {
                        // Đánh dấu là đã xóa nếu không tồn tại trong danh sách mới
                        existingEntity.Isdeleted = true;
                    }
                });

                // Thêm các bản ghi mới
                foreach (var newEntity in newEntities)
                {
                    newEntity.Staffid = staff.Staffid; // Gán StaffId cho bản ghi mới
                    _cafeManagerContext.Staffsalaryhistories.Add(newEntity);
                }
            }

            return update;
        }
    }
}