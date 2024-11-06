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
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        public FoodRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<IEnumerable<Food>> GetAllListFood()
        {
            return await _cafeManagerContext.Foods.Where(x => x.Isdeleted == false)
                .Include(x => x.Invoicedetails).Include(x => x.Foodcategory).Where(x => x.Foodcategory.Isdeleted == false).ToListAsync();
        }

        public async Task<Food> GetFoodById(int id)
        {
            return await _cafeManagerContext.Foods
                .Where(x => x.Isdeleted == false)
                .Include(x => x.Invoicedetails)
                .Include(x => x.Foodcategory).FirstOrDefaultAsync(x => x.Foodid == id);
        }
    }
}