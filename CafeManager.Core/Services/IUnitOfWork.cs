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
        IRepository<Foodcategory> FoodCategorys { get; }
        IRepository<Food> Foods { get; }

        int Complete();

        Task<int> CompleteAsync();
    }
}