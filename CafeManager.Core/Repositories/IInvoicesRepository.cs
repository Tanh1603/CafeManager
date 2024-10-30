using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IInvoicesRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetAllInvoice();

        Task<IEnumerable<Invoicedetail>> GetAllInvoiceDetailById(int id);

        //Task<decimal?> GetTotalMoneyById(int id);
    }
}