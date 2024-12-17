using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace CafeManager.WPF.Services
{
    public class MaterialSupplierServices(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        #region Supplier

        public async Task<IEnumerable<Supplier>> GetListSupplier(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _unitOfWork.SupplierList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Supplier>> GetListExistedSupplier()
        {
            return await _unitOfWork.SupplierList.GetAllExistedAsync();
        }

        public async Task<IEnumerable<Supplier>> GetListDeletedSupplier()
        {
            return await _unitOfWork.SupplierList.GetAllDeletedAsync();
        }

        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var list = await _unitOfWork.SupplierList.Create(supplier);

                if (list == null)
                {
                    throw new InvalidOperationException("Lỗi.");
                }

                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return list;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Thêm nhà cung cấp thất bại.", ex);
            }
        }

        public async Task<Supplier?> UpdateSupplier(Supplier supplier)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.SupplierList.Update(supplier);

                await _unitOfWork.CompleteAsync();

                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá vật liệu liệu thất bại.", ex);
            }
        }

        public async Task<Supplier?> GetSupplierById(int id)
        {
            var res = await _unitOfWork.SupplierList.GetById(id);
            return res?.Isdeleted == false ? res : null;
        }

        public async Task<bool> DeleteSupplier(int id)
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
                throw new InvalidOperationException("Thêm nhà cung cấp thất bại.", ex);
            }
        }

        #endregion Supplier

        #region Material

        public async Task<IEnumerable<Material>> GetListMaterial(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _unitOfWork.MaterialList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<Material> AddMaterial(Material material)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var list = await _unitOfWork.MaterialList.Create(material);

                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();

                await _unitOfWork.CommitTransactionAsync();
                return list;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá vật liệu liệu thất bại.", ex);
            }
        }

        public async Task<Material?> UpdateMaterial(Material material)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.MaterialList.Update(material);

                await _unitOfWork.CompleteAsync();

                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá vật liệu thất bại.", ex);
            }
        }

        public async Task<Material?> UpdateMaterialById(int id, Material material)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.MaterialList.Update(material);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Sửa vật liệu thất bại.", ex);
            }
        }

        public async Task<Material?> GetMaterialById(int id)
        {
            var res = await _unitOfWork.MaterialList.GetById(id);
            return res?.Isdeleted == false ? res : null;
        }

        public async Task<bool> DeleteMaterial(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.MaterialList.Delete(id);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return deleted;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá vật liệu thất bại.", ex);
            }
        }

        #endregion Material

        //Add Material Supplier
        public async Task<Materialsupplier> AddMaterialsupplier(Materialsupplier materialsupplier)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.MaterialSupplierList.Create(materialsupplier);

                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá vật liệu liệu thất bại.", ex);
            }
        }

        public async Task<IEnumerable<Materialsupplier>> GetListMaterialSupplier(CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _unitOfWork.MaterialSupplierList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<Materialsupplier?> GetMaterialsupplierById(int id)
        {
            var res = await _unitOfWork.MaterialSupplierList.GetById(id);
            return res?.Isdeleted == false ? res : null;
        }

        // ===================== Phan trang =======================
        public async Task<(IEnumerable<Materialsupplier>?, int)> GetSearchPaginateListMaterialsupplier(Expression<Func<Materialsupplier, bool>>? searchPredicate = null, int skip = 0, int take = 20, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _unitOfWork.MaterialSupplierList.GetByPageAsync(skip, take, searchPredicate, token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<(IEnumerable<Materialsupplier>?, int)> GetSearchPaginateListMaterialsupplierAlter(Expression<Func<Materialsupplier, bool>>? searchPredicate = null, int skip = 0, int take = 20)
        {
            _unitOfWork.ClearChangeTracker();
            return await _unitOfWork.MaterialSupplierList.GetByPageAsync(skip, take, searchPredicate);
        }

        public async Task<int> GetTotalMaterialSuplier()
        {
            return await _unitOfWork.MaterialSupplierList.GetToTalMaterialSupplier();
        }
    }
}