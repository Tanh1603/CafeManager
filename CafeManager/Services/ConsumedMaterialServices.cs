using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CafeManager.WPF.Services
{
    public class ConsumedMaterialServices(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;
        private readonly IUnitOfWork _unitOfWork = provider.GetRequiredService<IUnitOfWork>();

        public async Task<IEnumerable<Consumedmaterial>> GetListConsumedMaterial(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _unitOfWork.ConsumedMaterialList.GetAll();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<Consumedmaterial> AddConsumedMaterial(Consumedmaterial consumedmaterial)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                consumedmaterial.Materialsupplier = null;
                var addConsumedMaterial = await _unitOfWork.ConsumedMaterialList.Create(consumedmaterial);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return addConsumedMaterial;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Thêm chi tiết sử dụng thất bại.", ex);
            }
        }

        public async Task<Consumedmaterial?> UpdateConsumedmaterial(Consumedmaterial consumedmaterial)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.ConsumedMaterialList.Update(consumedmaterial);

                await _unitOfWork.CompleteAsync();

                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi");
            }
        }

        public async Task<Consumedmaterial?> UpdateConsumedMaterialById(int id, Consumedmaterial consumedmaterial)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                //await _unitOfWork.ConsumedMaterialList.UpdateById(id, consumedmaterial);
                var res = await _unitOfWork.ConsumedMaterialList.GetById(id);
                if (res != null)
                {
                    res.Quantity = consumedmaterial.Quantity;
                }
                await _unitOfWork.CompleteAsync();

                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Sửa vật liệu đã dùng liệu thất bại.", ex);
            }
        }

        public async Task<bool> DeleteConsumedmaterial(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.ConsumedMaterialList.Delete(id);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return deleted;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi");
            }
        }

        // ===================== Phan trang =======================
        public async Task<(IEnumerable<Consumedmaterial>?, int)> GetSearchPaginateListConsumedMaterial(Expression<Func<Consumedmaterial, bool>>? searchPredicate = null, int skip = 0, int take = 20)
        {
            return await _unitOfWork.ConsumedMaterialList.GetByPageAsync(skip, take, searchPredicate);
        }

        public async Task<(IEnumerable<Consumedmaterial>?, int)> GetSearchPaginateListConsumedMaterialAlter(Expression<Func<Consumedmaterial, bool>>? searchPredicate = null, int skip = 0, int take = 20)
        {
            _unitOfWork.ClearChangeTracker();
            return await _unitOfWork.ConsumedMaterialList.GetByPageAsync(skip, take, searchPredicate);
        }
    }
}