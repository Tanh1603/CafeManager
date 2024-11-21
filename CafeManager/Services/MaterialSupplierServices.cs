using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CafeManager.WPF.Services
{
    public class MaterialSupplierServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialSupplierServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Material>> GetListMaterial()
        {
            return (await _unitOfWork.MaterialList.GetAll()).Where(x => x.Isdeleted == false);
        }

        public async Task<IEnumerable<Supplier>> GetListSupplier()
        {
            return (await _unitOfWork.SupplierList.GetAll()).Where(x => x.Isdeleted == false);
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

                var res = _unitOfWork.SupplierList.Update(supplier);

                await _unitOfWork.CompleteAsync();

                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Xoá vật liệu liệu thất bại.", ex);
            }
        }

        public async Task<Supplier?> GetSupplierById(int id)
        {
            var res = await _unitOfWork.SupplierList.GetById(id);
            return res.Isdeleted == false ? res : null;
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

        //Material
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

                var res = _unitOfWork.MaterialList.Update(material);

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

        public async Task<Material?> GetMaterialById(int id)
        {
            var res = await _unitOfWork.MaterialList.GetById(id);
            return res.Isdeleted == false ? res : null;
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

        public async Task<IEnumerable<Materialsupplier>> GetListMaterialSupplier()
        {
            return await _unitOfWork.MaterialSupplierList.GetAll();
        }
    }
}