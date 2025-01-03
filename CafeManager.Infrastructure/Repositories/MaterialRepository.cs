﻿using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace CafeManager.Infrastructure.Repositories
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }
    }
}