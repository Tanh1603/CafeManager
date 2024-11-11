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

        public async Task<IEnumerable<MaterialDetailDTO>?> GetListImportDetailByImportId(int id)
        {
            var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
            var res = listImport.Where(x => x.Isdeleted == false)
                .Select(x => new MaterialDetailDTO
                {
                    Materialname = x.Materialsupplier.Material?.Materialname,

                    Suppliername = x.Materialsupplier.Supplier?.Suppliername,
                    Unit = x.Materialsupplier.Material?.Unit,
                    Quantity = x.Quantity ?? 0,
                    Price = x.Materialsupplier.Price ?? 0,
                    Original = x.Materialsupplier.Original,
                    Manufacturer = x.Materialsupplier.Manufacturer,
                    Manufacturedate = x.Materialsupplier.Manufacturedate,
                    Expirationdate = x.Materialsupplier.Expirationdate,
                });
            return res;
        }

        //public async Task<List<MaterialDetailDTO>?> GetListImportDetailByImportId(int id)
        //{
        //    var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
        //    List<MaterialDetailDTO> res = new List<MaterialDetailDTO>();
        //    if (listImport != null)
        //    {
        //        foreach (var item in listImport)
        //        {
        //            if(item.Isdeleted == false)
        //            {
        //                res.Add(new MaterialDetailDTO()
        //                {
        //                    Materialname = item.Materialsupplier.Material.Materialname,

        //                    Suppliername = item.Materialsupplier.Supplier.Suppliername,

        //                    Quantity = item.Quantity ?? 0,
        //                    Price = item.Materialsupplier.Price ?? 0,
        //                    Original = item.Materialsupplier.Original,
        //                    Manufacturer = item.Materialsupplier.Manufacturer,
        //                    Manufacturedate = item.Materialsupplier.Manufacturedate,
        //                    Expirationdate = item.Materialsupplier.Expirationdate,
        //                });
        //            }
        //        }
        //    }
        //    return res;
        //}

        #region Tính toán dữ liệu

        private decimal CaculatePriceOfListImportdetai(IEnumerable<Importdetail> importdetails)
        {
            return importdetails.Sum(
                    x => (x.Quantity ?? 0) * (x.Materialsupplier?.Price ?? 0)
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