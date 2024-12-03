using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;

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

        public async Task<Food?> GetFoodById(int id)
        {
            return await _unitOfWork.FoodList.GetById(id);
        }

        public async Task<IEnumerable<Food>> GetAllFood()
        {
            return await _unitOfWork.FoodList.GetAll();
        }

        public async Task<Food> CreateFood(Food food)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.FoodList.Create(food);

                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
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
                _unitOfWork.ClearChangeTracker();
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
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return isdeleted;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xóa thức ăn thất bại.");
            }
        }
    }
}