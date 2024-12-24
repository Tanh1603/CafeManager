using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class ConsumedMaterialRepository : Repository<Consumedmaterial>, IConsumedMaterialRepository
    {
        public ConsumedMaterialRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }
    }
}