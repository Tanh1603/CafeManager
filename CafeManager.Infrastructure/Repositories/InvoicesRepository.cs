using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Repositories
{
    public class InvoicesRepository : Repository<Invoice>, IInvoicesRepository
    {
        public InvoicesRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoiceAsync()
        {
            return await _cafeManagerContext.Set<Invoice>().Where(x => x.Isdeleted == false)
                .Include(x => x.Coffeetable)
                .Include(x => x.Invoicedetails)
                .ThenInclude(x => x.Food)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoicedetail>?> GetAllInvoiceDetailByInvoiceIdAsync(int id)
        {
            var res = await _cafeManagerContext.Set<Invoice>().Where(x => x.Isdeleted == false)
                .Include(x => x.Invoicedetails).ThenInclude(x => x.Food)
                .FirstOrDefaultAsync(x => x.Invoiceid == id);

            return res?.Invoicedetails.Where(x => x.Isdeleted == false) ?? Enumerable.Empty<Invoicedetail>();
        }

        public async Task<Coffeetable?> GetCoffeTableByInvoiceIdAsync(int id)
        {
            var res = await _cafeManagerContext.Set<Invoice>().Where(x => x.Isdeleted == false)
                .Include(x => x.Invoicedetails)
                .Include(x => x.Coffeetable)
                .FirstOrDefaultAsync(x => x.Coffeetableid == id);
            if (res != null && res.Coffeetable != null && !res.Coffeetable.Isdeleted == false)
            {
                return res.Coffeetable;
            }

            return null;
        }

        public async Task<Invoice?> GetInvoicesByIdAsync(int id)
        {
            return await _cafeManagerContext.Set<Invoice>().Where(x => x.Isdeleted == false)
                        .Include(x => x.Coffeetable)
                        .Include(x => x.Invoicedetails)
                        .ThenInclude(x => x.Food)
                        .FirstOrDefaultAsync(x => x.Invoiceid == id) ?? null;
        }

        public override async Task<bool> Delete(int id)
        {
            var listInvoiceDeleted = await GetInvoicesByIdAsync(id);
            if (listInvoiceDeleted != null)
            {
                listInvoiceDeleted.Isdeleted = true;
                foreach (var item in listInvoiceDeleted.Invoicedetails)
                {
                    item.Isdeleted = true;
                }
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Invoice>?> SearchSortPaginateAsync(Expression<Func<Invoice, bool>>? searchPredicate,
                                                                   Expression<Func<Invoice, object>>? sortKeySelector,
                                                                   bool ascending, int skip, int take)
        {
            var query = _cafeManagerContext.Set<Invoice>()
                                           .Where(x => x.Isdeleted == false)
                                           .Include(x => x.Coffeetable)
                                           .Include(x => x.Invoicedetails)
                                           .ThenInclude(x => x.Food)
                                           .AsQueryable();
            if (searchPredicate != null)
            {
                query = query.Where(searchPredicate);
            }
            if (sortKeySelector != null)
            {
                query = ascending ? query.OrderBy(sortKeySelector) : query.OrderByDescending(sortKeySelector);
            }
            query = query.Skip(skip).Take(take);
            return await query.ToListAsync();
        }
    }
}