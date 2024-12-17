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
    public class CoffeeTableRepository : Repository<Coffeetable>, ICoffeeTableRepository
    {
        public CoffeeTableRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public Task<int> GetTotalTable()
        {
            return _cafeManagerContext.Coffeetables.CountAsync();
        }
    }
}