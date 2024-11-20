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

        public async Task<Import?> GetImportById(int id)
        {
            return await _unitOfWork.ImportList.GetImportById(id);
        }

        //public async Task<IEnumerable<Importdetail>?> GetListImportDetailByImportId(int id)
        //{
        //    return await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);
        //}

        //public async Task<IEnumerable<MaterialDetailDTO>?> GetListImportDetailByImportId(int id)
        //{
        //    var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);

        //    var res = listImport
        //        .Select(x =>
        //        {
        //            // Lấy đối tượng MaterialSupplier đầu tiên phù hợp để tái sử dụng
        //            var materialSupplier = x.Material?.Materialsuppliers
        //                .FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid);

        //            return new MaterialDetailDTO
        //            {
        //                Materialname = x.Material?.Materialname,
        //                Suppliername = x.Import.Supplier?.Suppliername,
        //                Unit = x.Material?.Unit,
        //                Quantity = x.Quantity ?? 0,
        //                Price = materialSupplier?.Price ?? 0,
        //                Original = materialSupplier?.Original,
        //                Manufacturer = materialSupplier?.Manufacturer,
        //                Manufacturedate = materialSupplier?.Manufacturedate ?? DateTime.Now,
        //                Expirationdate = materialSupplier?.Expirationdate ?? DateTime.Now
        //            };
        //        });

        //    return res;
        //}

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

        #endregion Tính toán dữ liệu

        #region Thêm, xoa, sua import

        public async Task<Import> AddImport(Import import, IEnumerable<Importdetail> importdetails)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var dbRes = await _unitOfWork.ImportList.Create(import);
                await _unitOfWork.CompleteAsync();
                foreach (var item in importdetails)
                {
                    item.Importid = dbRes.Importid;
                }
                await _unitOfWork.ImportDetailList.AddArange(importdetails);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return dbRes;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Thêm phiếu nhập thất bại.", ex);
            }
        }

        public async Task<Import?> UpdateImport(Import import, List<Importdetail>? updateImportDetails)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = _unitOfWork.ImportList.Update(import);

                List<Importdetail> existingImportDetails = (await _unitOfWork.ImportDetailList.GetAll())
                    .Where(x => x.Isdeleted == false && x.Importid == import.Importid).ToList();

                if (updateImportDetails != null && updateImportDetails.Count > 0)
                {
                    var newImportDetailIds = updateImportDetails.Where(x => x.Importdetailid != 0).Select(x => x.Importdetailid).ToHashSet();
                    foreach (var existingImportDetail in existingImportDetails)
                    {
                        if (!newImportDetailIds.Contains(existingImportDetail.Importdetailid))
                        {
                            existingImportDetail.Isdeleted = true;
                        }
                    }

                    foreach (var newImportDetail in updateImportDetails)
                    {
                        if (newImportDetail.Importdetailid == 0)
                        {
                            await _unitOfWork.ImportDetailList.Create(new()
                            {
                                Importid = import.Importid,
                                Materialid = newImportDetail.Materialid,
                                Quantity = newImportDetail.Quantity,
                            });
                        }
                        else
                        {
                            var existingImportDetail = existingImportDetails.FirstOrDefault(x => x.Isdeleted == false && x.Importid == newImportDetail.Importid);
                            if (existingImportDetail != null)
                            {
                                existingImportDetail.Importid = newImportDetail.Importid;
                                existingImportDetail.Materialid = newImportDetail.Materialid;
                                existingImportDetail.Quantity = newImportDetail.Quantity;
                            }
                        }
                    }
                }
                else
                {
                    existingImportDetails.ForEach(x => x.Isdeleted = true);
                }
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi khi sửa thông tin nhập hàng");
            }
        }

        public async Task<bool> DeleteImport(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.ImportList.Delete(id);
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