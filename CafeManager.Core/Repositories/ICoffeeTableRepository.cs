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
        Task<int> GetTotalTable(CancellationToken token = default);
    }
}