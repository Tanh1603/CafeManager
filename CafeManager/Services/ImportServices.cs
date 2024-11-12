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
    public class ImportServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public ImportServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Import>?> GetListImport()
        {
            return await _unitOfWork.ImportList.GetAllImportsAsync();
        }

        //public async Task<IEnumerable<MaterialDetailDTO>?> GetListImportDetailByImportId(int id)
        //{
        //    var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
        //    var res = listImport.Where(x => x.Isdeleted == false)
        //        .Select(x => new MaterialDetailDTO
        //        {
        //            Materialname = x.Material?.Materialname,

        //            Suppliername = x.Import.Supplier?.Suppliername,
        //            Unit = x.Material?.Unit,
        //            Quantity = x.Quantity ?? 0,
        //            Price = x.Material?.Materialsuppliers.FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid)?.Price ?? 0,
        //            Original = x.Material?.Materialsuppliers.FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid)?.Original,

        //            Manufacturer = x.Material?.Materialsuppliers.FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid)?.Manufacturer,

        //            Manufacturedate =
        //            (DateTime)(x.Material?.Materialsuppliers.FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid)?.Manufacturedate),

        //            Expirationdate =
        //            (DateTime)(x.Material?.Materialsuppliers.FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid)?.Expirationdate),
        //        });
        //    return res;
        //}
        public async Task<IEnumerable<MaterialDetailDTO>?> GetListImportDetailByImportId(int id)
        {
            var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);

            var res = listImport
                .Select(x =>
                {
                    // Lấy đối tượng MaterialSupplier đầu tiên phù hợp để tái sử dụng
                    var materialSupplier = x.Material?.Materialsuppliers
                        .FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid);

                    return new MaterialDetailDTO
                    {
                        Materialname = x.Material?.Materialname,
                        Suppliername = x.Import.Supplier?.Suppliername,
                        Unit = x.Material?.Unit,
                        Quantity = x.Quantity ?? 0,
                        Price = materialSupplier?.Price ?? 0,
                        Original = materialSupplier?.Original,
                        Manufacturer = materialSupplier?.Manufacturer,
                        Manufacturedate = materialSupplier?.Manufacturedate ?? DateTime.Now,
                        Expirationdate = materialSupplier?.Expirationdate ?? DateTime.Now
                    };
                });

            return res;
        }

        #region Tính toán dữ liệu

        private decimal CaculatePriceOfListImportdetai(IEnumerable<Importdetail> importdetails)
        {
            return importdetails.Sum(
                    x => (x.Quantity ?? 0) * (x.Material?.Materialsuppliers
                        .FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid)?.Price ?? 0)
                );
        }

        public async Task<decimal> GetTotalPriceImports()
        {
            var listImport = await _unitOfWork.ImportList.GetAllImportsAsync();
            return listImport.Sum(x => CaculatePriceOfListImportdetai(x.Importdetails));
        }

        public async Task<decimal> GetTotalPriceImportById(int id)
        {
            var listImportDetailById = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
            return CaculatePriceOfListImportdetai(listImportDetailById);
        }

        public async Task<Import> GetImportById(int id)
        {
            return await _unitOfWork.ImportList.GetImportById(id);
        }

        #endregion Tính toán dữ liệu

        #region Thêm, xoa, sua import

        public async Task<Import> AddImport(Import import)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var list = await _unitOfWork.ImportList.Create(import);

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
                throw new InvalidOperationException("Thêm phiếu nhập thất bại.", ex);
            }
        }

        public Import? Update(Import import)
        {
            var res = _unitOfWork.ImportList.Update(import);
            _unitOfWork.Complete();
            return res;
        }

        public async Task<bool> DeleteImport(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.ImportList.Delete(id);
                if (deleted == false)
                {
                    throw new InvalidOperationException("Lỗi.");
                }
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return deleted;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá phiếu nhập thất bại.", ex);
            }
        }

        #endregion Thêm, xoa, sua import
    }
}