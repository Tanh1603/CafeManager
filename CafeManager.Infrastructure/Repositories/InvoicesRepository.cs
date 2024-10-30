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
    public class InvoicesRepository : Repository<Invoice>, IInvoicesRepository
    {
        private readonly CafeManagerContext _cafeManagerContext;

        public InvoicesRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
            _cafeManagerContext = cafeManagerContext;
        }

        public async Task<IEnumerable<Invoice>> GetAllInvoice()
        {
            return await _cafeManagerContext.Set<Invoice>().Include(x => x.Invoicedetails)
                .Include(x => x.Coffeetable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Invoicedetail>> GetAllInvoiceDetailById(int id)
        {
            var listInvoice = await GetAllInvoice();
            var listInvoiceDetail = listInvoice.FirstOrDefault(x => x.Invoiceid == id)?.Invoicedetails;
            return listInvoiceDetail ?? Enumerable.Empty<Invoicedetail>();
        }

        //public async Task<decimal?> GetTotalMoneyById(int id)
        //{
        //    decimal? res = 0;
        //    var listInvoiceDetail = await GetAllInvoiceDetailById(id);
        //    foreach (var item in listInvoiceDetail)
        //    {
        //        var foodPrice = item.Food.Price ?? 0;
        //        var foodDiscount = item.Food.Discountfood ?? 0;
        //        res += (foodPrice * item.Quantity) * ((100 - foodDiscount) / 100);
        //    }
        //    return res;
        //}
    }
}