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

        public async Task<Food> AddFood(Food food)
        {
            var entity = await _unitOfWork.FoodList.Create(food);
            _unitOfWork.Complete();
            return entity;
        }

        public Food? UpdatFood(Food? obj)
        {
            var res = _unitOfWork.FoodList.Update(obj);
            if (res != null)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public async Task<bool> DeletFood(int id)
        {
            var res = await _unitOfWork.FoodList.Delete(id);
            if (res)
            {
                _unitOfWork.Complete();
            }
            return res;
        }
    }
}