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

        public async Task<IEnumerable<Food>> GetAllFoodByFoodCatgoryIdAsync(int id)
        {
            var category = await _cafeManagerContext.Set<Foodcategory>()
                .Include(fc => fc.Foods)
                .Where(x => x.Isdeleted == false)
                .FirstOrDefaultAsync(x => x.Foodcategoryid == id);

            return category?.Foods.Where(x => x.Isdeleted == false) ?? Enumerable.Empty<Food>();
        }

        public async Task<IEnumerable<Foodcategory>> GetAllFoodCategoryAsync()
        {
            return await _cafeManagerContext.Set<Foodcategory>().Include(x => x.Foods).Where(x => x.Isdeleted == false).ToListAsync();
        }

        public async Task<Foodcategory?> GetFoodCategoryByIdAsync(int id)
        {
            return await _cafeManagerContext.Set<Foodcategory>()
                            .Include(x => x.Foods).Where(x => x.Isdeleted == false)
                            .FirstOrDefaultAsync(x => x.Foodcategoryid == id) ?? null;
        }

        public override async Task<bool> Delete(int id, CancellationToken token = default)
        {
            var foodCategoryById = await GetFoodCategoryByIdAsync(id);
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