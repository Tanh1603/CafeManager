using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class MaterialSupplierRepository : Repository<Materialsupplier>, IMaterialSupplierRepository
    {
        public MaterialSupplierRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<IEnumerable<Materialsupplier>> GetAllMaterialSuppierAsync()
        {
            //var materialsuppliers = await _cafeManagerContext.Materialsuppliers
            //        .Where(x => x.Isdeleted == false)
            //        .ToListAsync();  // Chỉ lấy Materialsupplier mà không cần Include

            //// Nếu bạn truy cập vào Material, Supplier hay Importdetails ở bất kỳ đâu trong quá trình xử lý,
            //// chúng sẽ được tải lazily khi truy cập vào các thuộc tính navigation.
            //foreach (var supplier in materialsuppliers)
            //{
            //    // Lazy loading sẽ tự động tải các mối quan hệ khi bạn truy cập chúng.
            //    var material = supplier.Material;  // Material sẽ được nạp khi truy cập
            //    var supplierDetail = supplier.Supplier;  // Supplier sẽ được nạp khi truy cập
            //    var importdetails = material.Importdetails.Where(x => x.Isdeleted == false && x.Import.Isdeleted == false);  // Importdetails sẽ nạp khi truy cập

            //    // Bạn có thể xử lý dữ liệu tại đây nếu cần.
            //}

            //return materialsuppliers;
            return await _cafeManagerContext.Materialsuppliers.Include(x => x.Material).ThenInclude(x => x.Importdetails).Include(x => x.Supplier).Where(x => x.Isdeleted == false).ToListAsync();
        }
    }
}