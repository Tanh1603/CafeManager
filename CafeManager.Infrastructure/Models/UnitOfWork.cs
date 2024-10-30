using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CafeManagerContext _context;
        public IRepository<Foodcategory> FoodCategorys { get; private set; }
        public IRepository<Food> Foods { get; private set; }

        public IInvoicesRepository InvoiceRepository { get; private set; }

        public IRepository<Invoicedetail> InvoiceDetail { get; private set; }

        public UnitOfWork(CafeManagerContext context)
        {
            _context = context;
            FoodCategorys = new Repository<Foodcategory>(context);
            Foods = new Repository<Food>(context);

            InvoiceDetail = new Repository<Invoicedetail>(context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}