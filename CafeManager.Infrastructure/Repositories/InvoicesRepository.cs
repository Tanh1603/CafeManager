using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            try
            {
                return await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).CountAsync(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetTotalRevenueFromTo(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                var list = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync(token);

                return list.Sum(x => x.Invoicedetails.Sum(x => x.Quantity * x.Food.Price)) ?? 0;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<decimal>> GetRevenueByDay(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                var invoices = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync(token);


                var allDates = Enumerable.Range(0, (to - from).Days + 1).Select(i => from.AddDays(i)).ToList();

               
                var revenueByDay = invoices
                    .Where(invoice => invoice.Invoicedetails != null) // Lọc hóa đơn có chi tiết
                    .GroupBy(invoice => invoice.Paymentstartdate.Date) // Nhóm theo ngày
                    .ToDictionary(
                        g => g.Key,  // Lấy ngày
                        g => g.Sum(x => x.Invoicedetails.Sum(d => d.Quantity * d.Food.Price)) ?? 0  // Tính tổng doanh thu
                    );

                
                var revenueList = allDates.Select(date =>
                {
               
                    return revenueByDay.ContainsKey(date) ? revenueByDay[date] : 0;
                }).ToList();

                return revenueList;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<decimal>> GetRevenueByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {

                var invoices = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync(token);

                var allMonths = Enumerable.Range(0, ((to.Year - from.Year) * 12 + to.Month - from.Month) + 1)
                             .              Select(i => new DateTime(from.Year, from.Month, 1).AddMonths(i))
                                        .ToList();

                
                var revenueByMonth = invoices
                    .Where(invoice => invoice.Invoicedetails != null) 
                    .GroupBy(invoice => new DateTime(invoice.Paymentstartdate.Year, invoice.Paymentstartdate.Month, 1)) 
                    .ToDictionary(
                        g => g.Key, 
                        g => g.Sum(x => x.Invoicedetails.Sum(d => d.Quantity * d.Food.Price)) ?? 0 
                    );

                
                var revenueList = allMonths.Select(month =>
                {
                   
                    return revenueByMonth.ContainsKey(month) ? revenueByMonth[month] : 0;
                }).ToList();

                return revenueList;
            }
            catch (OperationCanceledException)
            {

                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<int>> GetTotalInvoiceByDay(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                var invoices = await _cafeManagerContext.Invoices
          .Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to)
          .ToListAsync(token);


                var allDays = Enumerable.Range(0, (to - from).Days + 1)
                    .Select(i => from.AddDays(i))
                    .ToList();


                var invoicesByDay = invoices
                    .GroupBy(invoice => invoice.Paymentstartdate.Date)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Count()
                    );


                var invoiceCountList = allDays.Select(day =>
                {
                    return invoicesByDay.ContainsKey(day) ? invoicesByDay[day] : 0;
                }).ToList();

                return invoiceCountList;
            }
            catch (OperationCanceledException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task<List<int>> GetTotalInvoiceByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {

            try
            {
                var invoices = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync(token);


                var allMonths = Enumerable.Range(0, ((to.Year - from.Year) * 12 + to.Month - from.Month) + 1)
                                .Select(i => new DateTime(from.Year, from.Month, 1).AddMonths(i))
                                           .ToList();

                var invoicesByMonth = invoices
                    .GroupBy(invoice => new DateTime(invoice.Paymentstartdate.Year, invoice.Paymentstartdate.Month,1))
                    .ToDictionary(
                        g => g.Key,
                        g => g.Count()
                    );


                var invoiceCountList = allMonths.Select(month =>
                {

                    return invoicesByMonth.ContainsKey(month) ? invoicesByMonth[month] : 0;
                }).ToList();

                return invoiceCountList;
            }
            catch (OperationCanceledException)
            {

                throw;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<List<decimal>> GetRevenueByYear(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                var invoices = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync(token);

                var allMonths = Enumerable.Range(0, ((to.Year - from.Year) * 12 + to.Month - from.Month) + 1)
                             .Select(i => new DateTime(from.Year, from.Month, 1).AddMonths(i))
                                        .ToList();


                var revenueByMonth = invoices
                    .Where(invoice => invoice.Invoicedetails != null)
                    .GroupBy(invoice => new DateTime(invoice.Paymentstartdate.Year, invoice.Paymentstartdate.Month, 1))
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(x => x.Invoicedetails.Sum(d => d.Quantity * d.Food.Price)) ?? 0
                    );


                var revenueList = allMonths.Select(month =>
                {

                    return revenueByMonth.ContainsKey(month) ? revenueByMonth[month] : 0;
                }).ToList();

                return revenueList;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<int>> GetTotalInvoiceByYear(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                var invoices = await _cafeManagerContext.Invoices.Where(x => x.Isdeleted == false && x.Paymentstartdate >= from && x.Paymentstartdate <= to).ToListAsync(token);


                var allYears = Enumerable.Range(from.Year, (to.Year - from.Year) + 1).ToList();


                var invoicesByYear = invoices
                    .GroupBy(invoice => new DateTime(invoice.Paymentstartdate.Year, 1, 1))
                    .ToDictionary(
                        g => g.Key.Year,
                        g => g.Count()
                    );


                var invoiceCountList = allYears.Select(year =>
                    invoicesByYear.ContainsKey(year) ? invoicesByYear[year] : 0).ToList();

                return invoiceCountList;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}