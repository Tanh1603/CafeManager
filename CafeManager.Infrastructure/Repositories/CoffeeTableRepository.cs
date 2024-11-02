using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Repositories
{
    public class CoffeeTableRepository : Repository<Coffeetable>, ICoffeeTableRepository
    {
        public CoffeeTableRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<Coffeetable?> GetCoffeeTableByIdAsync(int id)
        {
            return await _cafeManagerContext.Set<Coffeetable>().Where(x => x.Isdeleted == false)
                .Include(x => x.Invoices).FirstOrDefaultAsync(x => x.Coffeetableid == id);
        }

        public async Task<IEnumerable<Coffeetable>> GetAllCoffeTableAsync()
        {
            return await _cafeManagerContext.Set<Coffeetable>().Where(x => x.Isdeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoicesByCoffeeTableIdAsync(int id)
        {
            var res = await _cafeManagerContext.Set<Coffeetable>().Where(x => x.Isdeleted == false)
                            .Include(x => x.Invoices)
                            .FirstOrDefaultAsync(x => x.Coffeetableid == id);
            return res?.Invoices?.Where(x => x.Isdeleted == false) ?? Enumerable.Empty<Invoice>();
        }
    }
}