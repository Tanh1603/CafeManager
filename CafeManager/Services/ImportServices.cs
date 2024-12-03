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
    public class ImportServices(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;
        private readonly IUnitOfWork _unitOfWork = provider.GetRequiredService<IUnitOfWork>();

        public async Task<IEnumerable<Import>?> GetListImport()
        {
            return await _unitOfWork.ImportList.GetAll();
        }

        public async Task<Import?> GetImportById(int id)
        {
            return await _unitOfWork.ImportList.GetById(id);
        }

        #region Thêm, xoa, sua import

        public async Task<Import> AddImport(Import import)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var addimport = await _unitOfWork.ImportList.Create(import);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return addimport;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
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
                    foreach (var x in updateImportDetails)
                    {
                        if (x.Importdetailid == 0)
                        {
                            await _unitOfWork.ImportDetailList.Create(new()
                            {
                                Importid = import.Importid,
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
                return await res;
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