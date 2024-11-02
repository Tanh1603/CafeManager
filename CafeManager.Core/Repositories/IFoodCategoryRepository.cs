using CafeManager.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IFoodCategoryRepository : IRepository<Foodcategory>
    {
        Task<IEnumerable<Foodcategory>> GetAllFoodCategoryAsync();

        Task<Foodcategory?> GetFoodCategoryByIdAsync(int id);

        Task<IEnumerable<Food>> GetAllFoodByFoodCatgoryIdAsync(int id);
    }
}