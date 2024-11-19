using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Core.Services
{
    public static class FoodCategoryMapper
    {
        public static FoodCategoryDTO ToDTO(Foodcategory foodCategory)
        {
            return new FoodCategoryDTO()
            {
                Foodcategoryid = foodCategory.Foodcategoryid,
                Foodcategoryname = foodCategory.Foodcategoryname,
                Isdeleted = foodCategory.Isdeleted,
                Foods = [.. foodCategory.Foods.Select(f => FoodMapper.ToDTO(f)).ToList()],
            };
        }

        public static Foodcategory ToEntity(FoodCategoryDTO foodCategoryDTO)
        {
            return new Foodcategory()
            {
                Foodcategoryid = foodCategoryDTO.Foodcategoryid,
                Foodcategoryname = foodCategoryDTO.Foodcategoryname,
                Isdeleted = foodCategoryDTO.Isdeleted,
                Foods = [.. foodCategoryDTO.Foods.Select(f => FoodMapper.ToEntity(f)).ToList()],
            };
        }
    }
}