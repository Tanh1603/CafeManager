using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Models
{
    public class CafeManagerContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public CafeManagerContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public CafeManagerContext CreateDbContext()
        {
            DbContextOptionsBuilder<CafeManagerContext> builder = new DbContextOptionsBuilder<CafeManagerContext>();
            _configureDbContext(builder);
            return new CafeManagerContext(builder.Options);
        }
    }
}