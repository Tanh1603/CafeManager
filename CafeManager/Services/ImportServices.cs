using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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


        public async Task<IEnumerable<ImportMaterialDetailDTO>?> GetListImportDetailByImportId(int id)
        {
            var listImport = await _unitOfWork.ImportList.GetAllImportsDetailsByImportIdAsync(id);

            var res = listImport
                .Select(x =>
                {
                    // Lấy đối tượng MaterialSupplier đầu tiên phù hợp để tái sử dụng
                    var materialSupplier = x.Material?.Materialsuppliers
                        .FirstOrDefault(f => f.Supplierid == x.Import.Supplierid && f.Materialid == x.Materialid);

                    return new ImportMaterialDetailDTO
                    {
                        Importdetailid = x.Importdetailid,
                        Materialid = x.Materialid,
                        Materialsupplierid = materialSupplier.Materialsupplierid,
                        Materialname = materialSupplier.Material.Materialname,
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

        public async Task<decimal> GetImportPriceById(int id)
        {
            var listImport = (await _unitOfWork.ImportDetailList.GetAll())
                .Where(x => x.Isdeleted == false && x.Importid == id);
            decimal res = 0;
            foreach(var item in listImport)
            {
                var tmp = item.Material.Materialsuppliers.FirstOrDefault(x => x.Supplierid == item.Import.Supplierid && x.Isdeleted == false);
                res += (item.Quantity ?? 0) * (tmp.Price ?? 0);
            }
            return res;
        }

        public async Task<decimal> GetTotalImportPrice()
        {
            var listImport = await _unitOfWork.ImportList.GetAll();
            var validImports = listImport.Where(x => x.Isdeleted == false);

            // Thực thi tất cả tác vụ bất đồng bộ và chờ chúng hoàn thành
            var importPrices = await Task.WhenAll(validImports.Select(import => GetImportPriceById(import.Importid)));

            // Tính tổng
            return importPrices.Sum();
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

        #endregion Tính toán dữ liệu

        #region Thêm, xoa, sua import

        public async Task<Import> AddImport(Import import, List<ImportMaterialDetailDTO> importMaterials)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var addimport = await _unitOfWork.ImportList.Create(import);
                await _unitOfWork.CompleteAsync();

                Materialsupplier newMaterialSupplier = new();
                var existingMaterialSuppliers = await _unitOfWork.MaterialSupplierList.GetAll();

                foreach (var detail in importMaterials)
                {
                    var existing = existingMaterialSuppliers
                        .FirstOrDefault(m => m.Materialid == detail.Materialid &&
                                             m.Supplierid == import.Supplierid &&
                                             m.Original == detail.Original &&
                                             m.Manufacturer == detail.Manufacturer &&
                                             m.Manufacturedate == detail.Manufacturedate &&
                                             m.Expirationdate == detail.Expirationdate &&
                                             m.Price == detail.Price);
                    if(existing == null)
                    {
                        newMaterialSupplier = await _unitOfWork.MaterialSupplierList.Create(
                            new Materialsupplier
                            {
                                Materialid = detail.Materialid,
                                Supplierid = import.Supplierid,
                                Manufacturedate = detail.Manufacturedate,
                                Expirationdate = detail.Expirationdate,
                                Original = detail.Original,
                                Manufacturer = detail.Manufacturer,
                                Price = detail.Price,
                            });
                        await _unitOfWork.CompleteAsync();

                        var res = await _unitOfWork.ImportDetailList.Create(new Importdetail()
                        {
                            Importid = import.Importid,
                            Materialid = detail.Materialid,
                            Quantity = detail.Quantity,
                        });
                        await _unitOfWork.CompleteAsync();
                    }
                    else
                    {
                        var a = await _unitOfWork.ImportDetailList.Create(new Importdetail()
                        {
                            Importid = import.Importid,
                            Materialid = existing.Materialid,
                            Quantity = detail.Quantity,
                        });
                        await _unitOfWork.CompleteAsync();
                    }
                }
                await _unitOfWork.CommitTransactionAsync();
                return addimport;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Thêm phiếu nhập thất bại.", ex);
            }
        }

        public async Task<Import?> UpdateImport(Import import, List<ImportMaterialDetailDTO>? updateImportDetails)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = _unitOfWork.ImportList.Update(import);
                Materialsupplier newMaterialSupplier = new();
                var existingMaterialSuppliers = (await _unitOfWork.MaterialSupplierList.GetAll())
                    .Where(x => x.Isdeleted == false).ToList();
                var existingImportDetails = await _unitOfWork.ImportDetailList.GetAll();

                if (updateImportDetails != null)
                {
                    foreach (var x in updateImportDetails) {
                        if (x.Importdetailid == 0)
                        {
                            await _unitOfWork.ImportDetailList.Create(new()
                            {
                                Importid = import.Importid,
                                Materialid = x.Materialid,
                                Quantity = x.Quantity
                            });
                        }
                        else
                        {
                            var existingImportDetail = existingImportDetails.FirstOrDefault(m => m.Isdeleted == false && m.Importdetailid == x.Importdetailid);

                            if (existingImportDetail != null)
                            {
                                if (x.Isdeleted == true)
                                {
                                    existingImportDetail.Isdeleted = false;
                                }
                                else
                                {
                                    existingImportDetail.Importid = import.Importid;
                                    existingImportDetail.Materialid = x.Materialid;
                                    existingImportDetail.Quantity = x.Quantity;
                                }
                            }
                        }

                        if (x.Materialsupplierid == 0)
                        {
                            var existing = existingMaterialSuppliers
                                .FirstOrDefault(m => m.Materialid == x.Materialid &&
                                             m.Supplierid == import.Supplierid &&
                                             m.Original == x.Original &&
                                             m.Manufacturer == x.Manufacturer &&
                                             m.Manufacturedate == x.Manufacturedate &&
                                             m.Expirationdate == x.Expirationdate &&
                                             m.Price == x.Price);
                            if (existing == null)
                            {
                                newMaterialSupplier = await _unitOfWork.MaterialSupplierList.Create(
                                    new Materialsupplier
                                    {
                                        Materialid = x.Materialid,
                                        Supplierid = import.Supplierid,
                                        Manufacturedate = x.Manufacturedate,
                                        Expirationdate = x.Expirationdate,
                                        Original = x.Original,
                                        Manufacturer = x.Manufacturer,
                                        Price = x.Price,
                                    });
                                await _unitOfWork.CompleteAsync();
                            }
                        }
                        else
                        {
                            var existMaterialSupplier = await _unitOfWork.MaterialSupplierList.GetById(x.Materialsupplierid);
                            if (existMaterialSupplier != null)
                            {
                                existMaterialSupplier.Materialid = x.Materialid;
                                existMaterialSupplier.Supplierid = import.Supplierid;
                                existMaterialSupplier.Manufacturedate = x.Manufacturedate;
                                existMaterialSupplier.Expirationdate = x.Expirationdate;
                                existMaterialSupplier.Original = x.Original;
                                existMaterialSupplier.Manufacturer = x.Manufacturer;
                                existMaterialSupplier.Price = x.Price;
                            }
                            await _unitOfWork.CompleteAsync();
                        }
                    }
                }
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi sửa thông tin nhập hàng");
            }
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