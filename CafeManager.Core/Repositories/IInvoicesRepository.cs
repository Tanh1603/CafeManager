﻿using CafeManager.Core.Data;
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
        Task<int> GetTotalInvoiceFromTo(DateTime from, DateTime to, CancellationToken token = default);

        Task<decimal> GetTotalRevenueFromTo(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetRevenueByDay(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetRevenueByMonth(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<decimal>> GetRevenueByYear(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<int>> GetTotalInvoiceByDay(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<int>> GetTotalInvoiceByMonth(DateTime from, DateTime to, CancellationToken token = default);

        Task<List<int>> GetTotalInvoiceByYear(DateTime from, DateTime to, CancellationToken token = default);
    }
}