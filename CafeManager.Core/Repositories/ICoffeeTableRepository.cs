﻿using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface ICoffeeTableRepository : IRepository<Coffeetable>
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesByCoffeeTableIdAsync(int id);

        Task<Coffeetable?> GetCoffeeTableByIdAsync(int id);

        Task<IEnumerable<Coffeetable>> GetAllCoffeTableAsync();
    }
}