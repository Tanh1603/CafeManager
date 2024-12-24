using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IImportRepository : IRepository<Import>
    {
        public Task<Import> UpdateStaffWithListImportDetail(Import import);

        Task<List<decimal>> GetTotalMaterialByDay(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetTotalMaterialByMonth(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetTotalMaterialByYear(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetTotalMaterialCostByMonth(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetTotalMaterialCostByYear(DateTime from, DateTime to, CancellationToken token = default);
    }
}