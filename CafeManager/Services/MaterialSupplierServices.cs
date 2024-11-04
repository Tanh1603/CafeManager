﻿using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public Supplier? UpdateSupplier(Supplier supplier)
        {
            var res = _unitOfWork.SupplierList.Update(supplier);
            if (res != null)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public async Task<Supplier> GetSupplierById(int id)
        {
            return await _unitOfWork.SupplierList.GetSupplierById(id);
        }

        public async Task<bool> DeleteSupplier(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.SupplierList.Delete(id);
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
                throw new InvalidOperationException("Thêm nhà cung cấp thất bại.", ex);
            }
        }
    }
}