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
    public class AppUserRepository : Repository<Appuser>, IAppUserRepository
    {
        public AppUserRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<Appuser?> GetAppUserByUserName(string name)
        {
            return await _cafeManagerContext.Appusers.FirstOrDefaultAsync(x => x.Username.Equals(name) && x.Isdeleted == false);
        }
    }
}