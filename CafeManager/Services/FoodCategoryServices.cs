﻿using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class FoodCategoryServices(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;
        private readonly IUnitOfWork _unitOfWork = provider.GetRequiredService<IUnitOfWork>();

        public async Task<IEnumerable<Foodcategory>> GetListFoodCategory()
        {
            return await _unitOfWork.FoodCategoryList.GetAllFoodCategoryAsync();
        }

        public async Task<IEnumerable<Food>> GetListFoodByFoodCatgoryId(int id)
        {
            return await _unitOfWork.FoodCategoryList.GetAllFoodByFoodCatgoryIdAsync(id);
        }

        public async Task<IEnumerable<Foodcategory>> GetAllListFoodCategory()
        {
            return await _unitOfWork.FoodCategoryList.GetAll();
        }

        public async Task<Foodcategory?> AddFoodCategory(Foodcategory foodcategory)
        {
            var res = await _unitOfWork.FoodCategoryList.Create(foodcategory);
            if (res != null)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public async Task<Foodcategory?> UpdateFoodCategory(Foodcategory foodcategory)
        {
            var res = await _unitOfWork.FoodCategoryList.Update(foodcategory);
            if (res != null)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public async Task<bool> DeleteFoodCategory(int id)
        {
            var res = await _unitOfWork.FoodCategoryList.Delete(id);
            if (res)
            {
                _unitOfWork.Complete();
            }
            return res;
        }
    }
}