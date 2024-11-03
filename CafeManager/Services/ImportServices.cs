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

        public async Task<IEnumerable<Import>?> GetImportList()
        {
            return await _unitOfWork.ImportList.GetAllImportsAsync();
        }

        public async Task<IEnumerable<MaterialDetailDTO>?> GetImportDetailByImportId(int id)
        {
            var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
            return listImport.Where(x => x.Materialsupplier.Isdeleted == false)
                .Select(x => new MaterialDetailDTO
                {
                    Materialname = x.Materialsupplier.Material.Materialname,

                    Suppliername = x.Materialsupplier.Supplier.Suppliername,

                    Quantity = x.Quantity ?? 0,
                    Price = x.Materialsupplier.Price ?? 0,
                    Original = x.Materialsupplier.Original,
                    Manufacturer = x.Materialsupplier.Manufacturer,
                    Manufacturedate = x.Materialsupplier.Manufacturedate,
                    Expirationdate = x.Materialsupplier.Expirationdate,
                });
        }

        #region Tính toán dữ liệu

        private decimal CaculatePriceOfImportdetailist(IEnumerable<Importdetail> importdetails)
        {
            return importdetails.Sum(
                    x => (x.Quantity ?? 0) * (x.Materialsupplier?.Price ?? 0)
                );
        }

        public async Task<decimal> GetTotalPriceImports()
        {
            var listImport = await _unitOfWork.ImportList.GetAllImportsAsync();
            return listImport.Sum(x => CaculatePriceOfImportdetailist(x.Importdetails));
        }

        public async Task<decimal> GetTotalPriceImportById(int id)
        {
            var listImportDetailById = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
            return CaculatePriceOfImportdetailist(listImportDetailById);
        }

        #endregion Tính toán dữ liệu

        #region Thêm, xoa, sua import

        public async Task<Import> AddImport(Import import)
        {
            return await _unitOfWork.ImportList.Create(import);
        }

        public Import? Update(Import import)
        {
            var res = _unitOfWork.ImportList.Update(import);
            _unitOfWork.Complete();
            return res;
        }

        public async Task<bool> DeleteImport(int id)
        {
            var res = await _unitOfWork.ImportList.Delete(id);
            _unitOfWork.Complete();
            return res;
        }

        #endregion Thêm, xoa, sua import
    }
}