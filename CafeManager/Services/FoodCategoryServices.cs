using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class FoodCategoryServices(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Foodcategory>> GetAllListFoodCategory(CancellationToken token = default)
        {
            try
            {
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.FoodCategoryList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("Dừng lấy food");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Foodcategory>> GetAllListExistFoodCategory(CancellationToken token)
        {
            try
            {
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.FoodCategoryList.GetAllExistFoodCategoryAsync(token);
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

        public async Task<Foodcategory?> AddFoodCategory(Foodcategory foodcategory)
        {
            var res = await _unitOfWork.FoodCategoryList.Create(foodcategory);
            if (res != null)
            {
                await _unitOfWork.CompleteAsync();
            }
            return res;
        }

        public async Task<Foodcategory?> UpdateFoodCategory(Foodcategory foodcategory)
        {
            var res = await _unitOfWork.FoodCategoryList.Update(foodcategory);
            if (res != null)
            {
                await _unitOfWork.CompleteAsync();
            }
            return res;
        }

        public async Task<bool> DeleteFoodCategory(int id)
        {
            var res = await _unitOfWork.FoodCategoryList.Delete(id);
            if (res)
            {
                await _unitOfWork.CompleteAsync();
            }
            return res;
        }
    }
}