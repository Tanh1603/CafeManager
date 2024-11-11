﻿using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Repositories
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<IEnumerable<Material>> GetAllMaterialAsync();

        Task<IEnumerable<MaterialDetailDTO>> GetAllMaterialWithDetail();

        Task<IEnumerable<MaterialDetailDTO>> GetAllUsedMaterial();

        Task<Material> GetMaterialById(int id);
    }
}