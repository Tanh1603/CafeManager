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

        public async Task<IEnumerable<Food>> GetAllListFood()
        {
            return await _unitOfWork.FoodList.GetAllListFood();
        }

        public async Task<Food> GetFoodById(int id)
        {
            return await _unitOfWork.FoodList.GetFoodById(id);
        }

        public async Task<Food> AddFood(Food food)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var list = await _unitOfWork.FoodList.Create(food);

                if (list == null)
                {
                    throw new InvalidOperationException("Lỗi.");
                }

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return list;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Thêm thức ăn thất bại.");
            }
        }

        public Food? UpdatFood(Food? obj)
        {
            try
            {
                var res = _unitOfWork.FoodList.Update(obj);
                if (res != null)
                {
                    _unitOfWork.Complete();
                }
                return res;
            }
            catch (Exception)
            {
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Sửa thức ăn thất bại.");
            }
        }

        public async Task<bool> DeletFood(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                bool isdeleted = await _unitOfWork.FoodList.Delete(id);
                if (isdeleted)
                {
                    await _unitOfWork.CompleteAsync();
                }
                await _unitOfWork.CommitTransactionAsync();
                return isdeleted;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Xóa thức ăn thất bại.");
            }
        }
    }
}