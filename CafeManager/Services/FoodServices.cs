using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class FoodServices
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;

        public FoodServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Foodcategory>> GetAllFoodCategoryAsync()
        {
            var foodCategories = await _serviceProvider.GetRequiredService<IUnitOfWork>().FoodCategorys.GetAllAsync();
            return foodCategories;
        }

        public async Task<IEnumerable<FoodDTO>> GetAllFoodByCategoryIdAsync(int id)
        {
            var foodList = await _unitOfWork.Foods.GetAllAsync();
            var foodDtos = foodList.Where(x => x.Foodcategoryid == id)
                                .Select(f => new FoodDTO()
                                {
                                    DisplayFoodName = f.Foodname,
                                    FoodPrice = f.Price ?? 0,
                                    ImageFood = f.Imagefood,
                                    DiscountFood = f.Discountfood ?? 0,
                                }).ToList();
            return foodDtos;
        }

        public async Task<Foodcategory> AddNewFoodCategory(Foodcategory obj)
        {
            var entity = await _unitOfWork.FoodCategorys.Create(obj);
            _unitOfWork.Complete();
            return entity;
        }

        public async Task<Foodcategory> UpdatNewFoodCategory(Foodcategory obj)
        {
            var existingCategory = await _unitOfWork.FoodCategorys.GetById(obj.Foodcategoryid);

            if (existingCategory != null)
            {
                existingCategory.Foodcategoryname = obj.Foodcategoryname;
            }
            var res = _unitOfWork.FoodCategorys.Update(existingCategory);
            _unitOfWork.Complete();
            return res;
        }

        public async Task<bool> DeleteFoodCategory(int id)
        {
            var res = await _unitOfWork.FoodCategorys.Delete(id);
            var obj = await _unitOfWork.FoodCategorys.GetById(id);
            foreach (var item in obj.Foods)
            {
                item.Isdeleted = true;
            }
            _unitOfWork.Complete();
            return res;
        }

        public async Task<Food> AddNewFood(Food food)
        {
            var entity = await _unitOfWork.Foods.Create(food);
            _unitOfWork.Complete();
            return entity;
        }

        public async Task<Food> UpdatNewFood(Food obj)
        {
            var existingFood = await _unitOfWork.Foods.GetById(obj.Foodid);

            if (existingFood != null)
            {
                existingFood.Foodname = obj.Foodname;
                existingFood.Price = obj.Price;
                existingFood.Imagefood = obj.Imagefood;
                existingFood.Discountfood = obj.Discountfood;
                existingFood.Foodcategoryid = obj.Foodcategoryid;
            }
            var res = _unitOfWork.Foods.Update(existingFood);
            _unitOfWork.Complete();
            return res;
        }

        public async Task<bool> DeletFood(int id)
        {
            var res = await _unitOfWork.Foods.Delete(id);
            _unitOfWork.Complete();
            return res;
        }
    }
}