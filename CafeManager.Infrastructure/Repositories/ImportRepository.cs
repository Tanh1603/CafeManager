using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;

namespace CafeManager.Infrastructure.Repositories
{
    public class ImportRepository : Repository<Import>, IImportRepository
    {
        public ImportRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public override async Task<bool> Delete(int id)
        {
            var importDeleted = await _cafeManagerContext.Imports.FindAsync(id);
            if (importDeleted != null)
            {
                importDeleted.Isdeleted = true;
                var listImporDetailtDeleted = importDeleted.Importdetails;
                foreach (var item in listImporDetailtDeleted)
                {
                    item.Isdeleted = true;
                    item.Materialsupplier.Isdeleted = true;
                }
            }

            return false;
        }
    }
}