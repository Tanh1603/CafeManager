using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IInvoicesRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllInvoiceAsync();

        Task<Invoice?> GetInvoicesByIdAsync(int id);

        Task<IEnumerable<Invoicedetail>?> GetAllInvoiceDetailByInvoiceIdAsync(int id);

        Task<Coffeetable?> GetCoffeTableByInvoiceIdAsync(int id);

        Task<IEnumerable<Invoice>?> SearchSortPaginateAsync(Expression<Func<Invoice, bool>>? searchPredicate,
                                                            Expression<Func<Invoice, object>>? sortKeySelector,
                                                            bool ascending, int skip, int take);
    }
}