using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
            return await _unitOfWork.ConsumedMaterialList.GetAll();
        }

        public async Task<Consumedmaterial> AddConsumedmaterial(Consumedmaterial addConsumedMaterial)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var consumedMaterial = (await _unitOfWork.ConsumedMaterialList.GetAll())
                    .FirstOrDefault(x => x.Materialsupplierid == addConsumedMaterial.Materialsupplierid);

                if (consumedMaterial != null)
                {
                    
                    if(consumedMaterial.Isdeleted == true)
                    {
                        consumedMaterial.Isdeleted = false;
                        consumedMaterial.Quantity = addConsumedMaterial.Quantity;
                    }
                    else
                    {
                        consumedMaterial.Quantity += addConsumedMaterial.Quantity;
                    }
                    await _unitOfWork.CompleteAsync();
                    //_unitOfWork.ClearChangeTracker();

                    await _unitOfWork.CommitTransactionAsync();
                    return consumedMaterial;
                }
                else
                {
                    var list = await _unitOfWork.ConsumedMaterialList.Create(addConsumedMaterial);
                    await _unitOfWork.CompleteAsync();
                    _unitOfWork.ClearChangeTracker();

                    await _unitOfWork.CommitTransactionAsync();
                    return list;
                }

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
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

                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
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

                await _unitOfWork.ConsumedMaterialList.UpdateById(id, consumedmaterial);
                var res = await _unitOfWork.ConsumedMaterialList.GetById(id);

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
    }
}