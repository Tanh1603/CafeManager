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

        public async Task<Staff?> GetStaffById(int id)
        {
            return await _cafeManagerContext.Staff
                .Where(x => x.Isdeleted == false)
                .Include(x => x.Staffsalaryhistories.Where(s => s.Isdeleted == false))
                .FirstOrDefaultAsync(x => x.Staffid == id);
        }

        public async Task<IEnumerable<Staff>> GetAllStaffAsync()
        {
            return await _cafeManagerContext.Staff
                .Where(x => x.Isdeleted == false)
                .Include(x => x.Staffsalaryhistories.Where(s => s.Isdeleted == false))
                .ToListAsync();
        }

            return update;
        }
    }
}