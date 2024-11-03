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
            return await _unitOfWork.MaterialList.GetAllMaterialAsync();
        }

        public async Task<IEnumerable<Supplier>> GetListSupplier()
        {
            return await _unitOfWork.SupplierList.GetAllSupplierAsync();
        }

        public async Task<IEnumerable<MaterialDetailDTO>?> GetListConsumedMaterial()
        {
            return await _unitOfWork.MaterialList.GetAllUsedMaterial();
        }

        public async Task<List<MaterialDetailDTO>> GetInventoryList()
        {
            var totalList = await GetListMaterialWithDetail();
            var usedList = await GetListConsumedMaterial();
            List<MaterialDetailDTO> res = new List<MaterialDetailDTO>();
            if (totalList != null)
            {
                foreach (var item in totalList)
                {
                    var tmp = usedList?.FirstOrDefault(
                        x => x.Materialname == item.Materialname && x.Suppliername == item.Suppliername
                        && x.Unit == item.Unit && x.Manufacturer == item.Manufacturer && x.Original == item.Original
                        && x.Manufacturedate == item.Manufacturedate && x.Expirationdate == item.Expirationdate && x.Price == item.Price

                    );
                    res.Add(new MaterialDetailDTO()
                    {
                        Materialname = item.Materialname,
                        Suppliername = item.Suppliername,
                        Unit = item.Unit,
                        Manufacturer = item.Manufacturer,
                        Manufacturedate = item.Manufacturedate,
                        Expirationdate = item.Expirationdate,
                        Price = item.Price,
                        Original = item.Original,
                        Quantity = item.Quantity - (tmp?.Quantity ?? 0),
                    });
                }
            }
            return res;
        }

        public async Task<IEnumerable<MaterialDetailDTO>?> GetListMaterialWithDetail()
        {
            return await _unitOfWork.MaterialList.GetAllMaterialWithDetail();
        }
    }
}