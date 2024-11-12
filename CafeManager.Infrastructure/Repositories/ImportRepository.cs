using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class ImportRepository : Repository<Import>, IImportRepository
    {
        public ImportRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<Import> GetImportById(int id)
        {
            return await _cafeManagerContext.Imports
                        .Where(x => x.Isdeleted == false)
                        .FirstOrDefaultAsync(x => x.Importid == id);
        }

        public async Task<IEnumerable<Import>> GetAllImportsAsync()
        {
            return await _cafeManagerContext.Imports.Where(x => x.Isdeleted == false)
                .Include(x => x.Importdetails)
                .ThenInclude(x => x.Materialsupplier).ToListAsync();
        }

        public async Task<IEnumerable<Importdetail>> GetAllImportsDetailsByImportIdAsync(int id)
        {
            var listImportDetail = await _cafeManagerContext.Imports
                .Where(x => x.Isdeleted == false)
                .Include(x => x.Importdetails)
                .ThenInclude(x => x.Materialsupplier)
                .ThenInclude(x => x.Material)
                .Include(x => x.Importdetails)
                .ThenInclude(x => x.Materialsupplier)
                .ThenInclude(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.Importid == id);
            return listImportDetail?.Importdetails ?? Enumerable.Empty<Importdetail>();
        }

        public override async Task<bool> Delete(int id)
        {
            var importDeleted = await _cafeManagerContext.Imports.FirstOrDefaultAsync(x => x.Importid == id);
            if (importDeleted != null)
            {

                var listImporDetailtDeleted = await GetAllImportsDetailsByImportIdAsync(importDeleted.Importid);
                foreach (var item in listImporDetailtDeleted)
                {
                    item.Isdeleted = true;
                    item.Materialsupplier.Isdeleted = true;
                }

                var entity = await _cafeManagerContext.Set<Import>().FindAsync(id);
                if (entity != null)
                {
                    var property = typeof(Import).GetProperty("Isdeleted");
                    if (property != null)
                    {
                        property.SetValue(entity, true);
                    }
                }

                return true;
            }

            return false;
        }
    }
}