using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IFoodCategoryRepository FoodCategoryList { get; }
        IFoodRepository FoodList { get; }

        ICoffeeTableRepository CoffeeTableList { get; }
        IInvoicesRepository InvoiceList { get; }
        IRepository<Invoicedetail> InvoiceDetail { get; }

        IImportRepository ImportList { get; }
        IRepository<Importdetail> ImportDetailList { get; }
        IMaterialRepository MaterialList { get; }
        ISupplierRepository SupplierList { get; }
        IRepository<Materialsupplier> MaterialSupplierList { get; }

        int Complete();

        Task<int> CompleteAsync();

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}