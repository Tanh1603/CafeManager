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
    public class FoodCategoryRepository : Repository<Foodcategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<IEnumerable<Foodcategory>> GetAllExistFoodCategoryAsync(CancellationToken token = default)
        {
            try
            {
                return await _cafeManagerContext.Set<Foodcategory>().Include(x => x.Foods.Where(f => f.Isdeleted == false)).Where(x => x.Isdeleted == false).ToListAsync(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<bool> Delete(int id, CancellationToken token = default)
        {
            var foodCategoryById = await GetById(id);
            if (foodCategoryById == null)
            {
                return false;
            }
            foodCategoryById.Isdeleted = true;
            foreach (var item in foodCategoryById.Foods)
            {
                item.Isdeleted = true;
            }
            return true;
        }
    }
}