﻿using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.WPF.Services
{
    public class FoodServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Food?> GetFoodById(int id)
        {
            return await _unitOfWork.FoodList.GetById(id);
        }

        public async Task<IEnumerable<Food>> GetAllFood(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.FoodList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Food>> GetAllExistFood(CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.FoodList.GetAllExistedAsync(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Food> CreateFood(Food food)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.FoodList.Create(food);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Thêm thức ăn thất bại.");
            }
        }

        public async Task<Food?> UpdatFood(Food obj)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.FoodList.Update(obj);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
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
                throw new InvalidOperationException("Xóa thức ăn thất bại.");
            }
        }

        public async Task<int> GetTotalFood(CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.FoodList.GetTotalFood(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}