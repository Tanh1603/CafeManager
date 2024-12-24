using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IFoodRepository : IRepository<Food>
    {
        Task<int> GetTotalFood(CancellationToken token = default);

        Task<List<FoodDTO>> GetMostSoldFoods(DateTime From, DateTime To, CancellationToken token = default);  
    }
}