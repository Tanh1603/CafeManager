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
        Task<Import?> GetImportById(int id);

        Task<IEnumerable<Import>> GetAllImportsAsync();

        Task<IEnumerable<Importdetail>> GetAllImportsDetailsByImportIdAsync(int id);
    }
}