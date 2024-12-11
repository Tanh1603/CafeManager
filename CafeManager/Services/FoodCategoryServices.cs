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

        public async Task<IEnumerable<Foodcategory>> GetListFoodCategory()
        {
            try
            {
                return await _unitOfWork.FoodCategoryList.GetAllFoodCategoryAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Food>> GetListFoodByFoodCatgoryId(int id)
        {
            return await _unitOfWork.FoodCategoryList.GetAllFoodByFoodCatgoryIdAsync(id);
        }

        public async Task<IEnumerable<Foodcategory>> GetAllListFoodCategory(CancellationToken token = default)
        {
            try
            {

                    token.ThrowIfCancellationRequested();
                
                return await _unitOfWork.FoodCategoryList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("Dừng lấy food");
            }
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