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

        public FoodServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IEnumerable<Foodcategory>> GetAllFoodCategoryAsync()
        {
            var foodCategories = await _serviceProvider.GetRequiredService<IUnitOfWork>().FoodCategorys.GetAllAsync();
            return foodCategories;
        }

        public async Task<IEnumerable<FoodDTO>> GetAllFoodByCategoryIdAsync(int id)
        {
            var foods = await _serviceProvider.GetRequiredService<IUnitOfWork>().Foods.GetAllAsync();
            var foodDtos = foods.Where(x => x.Foodcategoryid == id)
                                .Select(f => new FoodDTO()
                                {
                                    DisplayFoodName = f.Displayname,
                                    FoodPrice = f.Price ?? 0,
                                    ImageFood = f.Imagefood,
                                    DiscountFood = f.Discountfood ?? 0,
                                }).ToList();
            return foodDtos;
        }

        public async Task<Foodcategory> AddNewFoodCategory(Foodcategory obj)
        {
            var entity = await _serviceProvider.GetRequiredService<IUnitOfWork>().FoodCategorys.Create(obj);
            return entity;
        }
    }
}