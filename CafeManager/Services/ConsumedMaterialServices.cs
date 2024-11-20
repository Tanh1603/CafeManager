using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class ConsumedMaterialServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public ConsumedMaterialServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Consumedmaterial>> GetListConsumedMaterial()
        {
            return await _unitOfWork.ConsumedMaterialList.GetAllConsumedMaterialsAsync();
        }

        public async Task<Consumedmaterial> AddConsumedmaterial(Consumedmaterial consumedmaterial)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var list = await _unitOfWork.ConsumedMaterialList.Create(consumedmaterial);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return list;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi.");
            }
        }

        public async Task<Consumedmaterial?> UpdateConsumedmaterial(Consumedmaterial consumedmaterial)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = _unitOfWork.ConsumedMaterialList.Update(consumedmaterial);

                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi");
            }
        }

        public async Task<bool> DeleteConsumedmaterial(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.SupplierList.Delete(id);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return deleted;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi");
            }
        }
    }
}