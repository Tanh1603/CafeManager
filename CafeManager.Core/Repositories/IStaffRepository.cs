using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<IEnumerable<Staff>> GetAllStaffAsync();

        Task<IEnumerable<Staff>> GetAllStaffDeletedAsync();

        Task<Staff?> GetStaffById(int id);
    }
}