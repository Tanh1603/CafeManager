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
    public class ImportDetailServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public ImportDetailServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = _provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task AddImportDetailArange(List<MaterialDetailDTO> materialDetailDTOs, Import import, Material addMaterial, Supplier addSupplier)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var addImport = await _unitOfWork.ImportList.Create(import);
                await _unitOfWork.CompleteAsync();

                Materialsupplier newMaterialSuppliers = new();
                var existingMaterialSuppliers = await _unitOfWork.MaterialSupplierList.GetAll();

                foreach (var detail in materialDetailDTOs)
                {
                    var existing = existingMaterialSuppliers
                        .FirstOrDefault(m => m.Materialid == addMaterial.Materialid &&
                                             m.Supplierid == addSupplier.Supplierid &&
                                             m.Original == detail.Original &&
                                             m.Manufacturer == detail.Manufacturer &&
                                             m.Manufacturedate == detail.Manufacturedate &&
                                             m.Expirationdate == detail.Expirationdate &&
                                             m.Price == detail.Price);

                    if (existing == null)
                    {
                        newMaterialSuppliers = await _unitOfWork.MaterialSupplierList.Create(
                            new Materialsupplier
                            {
                                Materialid = addMaterial.Materialid,
                                Supplierid = addSupplier.Supplierid,
                                Manufacturedate = detail.Manufacturedate,
                                Expirationdate = detail.Expirationdate,
                                Original = detail.Original,
                                Manufacturer = detail.Manufacturer,
                                Price = detail.Price,
                            });
                        await _unitOfWork.CompleteAsync();

                        var res = await _unitOfWork.ImportDetailList.Create(new Importdetail()
                        {
                            Importid = addImport.Importid,
                            Materialsupplierid = newMaterialSuppliers.Materialsupplierid,
                            Quantity = detail.Quantity,
                        });
                        await _unitOfWork.CompleteAsync();
                    }
                    else
                    {
                        var a = await _unitOfWork.ImportDetailList.Create(new Importdetail()
                        {
                            Importid = addImport.Importid,
                            Materialsupplierid = existing.Materialsupplierid,
                            Quantity = detail.Quantity,
                        });
                        await _unitOfWork.CompleteAsync();
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}