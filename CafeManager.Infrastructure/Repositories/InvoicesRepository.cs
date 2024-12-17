using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class InvoicesRepository : Repository<Invoice>, IInvoicesRepository
    {
        public InvoicesRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public override async Task<bool> Delete(int id, CancellationToken token = default)
        {
            var listInvoiceDeleted = await GetById(id);
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

        public async Task<int> GetTotalInvoiceFromTo(DateTime from, DateTime to, CancellationToken token = default)
        {
            return await _cafeManagerContext
                .Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).CountAsync();
        }

        public async Task<decimal> GetTotalRevenueFromTo(DateTime from, DateTime to)
        {
            var list = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync();

            return list.Sum(x => x.Invoicedetails.Sum(x => x.Quantity * x.Food.Price)) ?? 0;
        }
    }
}