﻿using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeManager.Infrastructure.Repositories
{
    public class ImportRepository : Repository<Import>, IImportRepository
    {
        public ImportRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<Import?> UpdateStaffWithListImportDetail(Import import)
        {
            var update = await _cafeManagerContext.Imports.FindAsync(import.Importid);
            if (update != null)
            {
                // Cập nhật thông tin Staff
                _cafeManagerContext.Entry(update).CurrentValues.SetValues(import);

                // Lấy danh sách Importdetail hiện có trong cơ sở dữ liệu
                var existingImportdetails = await _cafeManagerContext.Importdetails
                    .Where(x => x.Isdeleted == false && x.Importid == import.Importid).ToListAsync();

                // Phân loại các bản ghi mới
                var newEntities = import.Importdetails.Where(x => x.Importdetailid == 0).ToList();
                var updateEntities = import.Importdetails
                    .Where(x => x.Importdetailid != 0)
                    .ToDictionary(x => x.Importdetailid); // Tạo dictionary từ các bản ghi có ID

                // Cập nhật các bản ghi hiện có
                foreach (var existingEntity in existingImportdetails)
                {
                    if (updateEntities.TryGetValue(existingEntity.Importdetailid, out var newEntity))
                    {
                        // Xử lý Materialsupplier
                        if (newEntity.Materialsupplier != null)
                        {
                            _cafeManagerContext.Entry(existingEntity.Materialsupplier).CurrentValues.SetValues(newEntity.Materialsupplier);
                        }
                        // Cập nhật bản ghi nếu tìm thấy
                        _cafeManagerContext.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                        updateEntities.Remove(existingEntity.Importdetailid);
                    }
                }

                // Thêm các bản ghi mới
                if (newEntities.Count != 0)
                {
                    foreach (var newEntity in newEntities)
                    {
                        // Xử lý Materialsupplier
                        newEntity.Materialsupplier = await FindOrCreateMaterialsupplier(newEntity.Materialsupplier);
                    }
                    await _cafeManagerContext.AddRangeAsync(newEntities);
                }

                await _cafeManagerContext.SaveChangesAsync();
            }

            return update;
        }

        // Hàm xử lý Materialsupplier
        private async Task<Materialsupplier> FindOrCreateMaterialsupplier(Materialsupplier materialsupplier)
        {
            // Tìm kiếm một Materialsupplier với các thuộc tính tương tự trong cơ sở dữ liệu
            var existingSupplier = await _cafeManagerContext.Materialsuppliers
                .FirstOrDefaultAsync(ms =>
                    ms.Materialid == materialsupplier.Materialid &&
                    ms.Supplierid == materialsupplier.Supplierid &&
                    ms.Manufacturedate == materialsupplier.Manufacturedate &&
                    ms.Expirationdate == materialsupplier.Expirationdate &&
                    ms.Original == materialsupplier.Original &&
                    ms.Manufacturer == materialsupplier.Manufacturer &&
                    ms.Price == materialsupplier.Price); // Không xét Materialsupplierid

            if (existingSupplier != null)
            {
                if (existingSupplier.Isdeleted == true)
                    existingSupplier.Isdeleted = false;

                return existingSupplier; // Sử dụng Materialsupplier đã tồn tại
            }

            // Nếu chưa tồn tại, thêm mới
            var newSupplier = new Materialsupplier
            {
                Materialid = materialsupplier.Materialid,
                Supplierid = materialsupplier.Supplierid,
                Manufacturedate = materialsupplier.Manufacturedate,
                Expirationdate = materialsupplier.Expirationdate,
                Original = materialsupplier.Original,
                Manufacturer = materialsupplier.Manufacturer,
                Price = materialsupplier.Price,
                Isdeleted = false
            };

            _cafeManagerContext.Materialsuppliers.Add(newSupplier);
            await _cafeManagerContext.SaveChangesAsync();

            return newSupplier; // Trả về bản ghi mới tạo
        }

        public override async Task<bool> Delete(int id, CancellationToken token = default)
        {
            var importDeleted = await _cafeManagerContext.Imports.FindAsync(id);
            if (importDeleted != null)
            {
                importDeleted.Isdeleted = true;
                var listImporDetailtDeleted = importDeleted.Importdetails;
                foreach (var item in listImporDetailtDeleted)
                {
                    item.Isdeleted = true;
                    //item.Materialsupplier.Isdeleted = true;
                }
                return true;
            }

            return false;
        }

    
    

        public async Task<List<decimal>> GetTotalMaterialCostByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                from = from.Date; // Bắt đầu từ 00:00:00
                to = to.Date.AddDays(1).AddTicks(-1); // Kết thúc vào 23:59:59.9999999

                var imports = await _cafeManagerContext.Imports
                    .Where(x => x.Isdeleted == false && x.Receiveddate >= from && x.Receiveddate <= to)
                    .Include(i => i.Importdetails.Where(d => d.Isdeleted == false)) // Include Importdetails để tính tổng giá trị
                    .ThenInclude(d => d.Materialsupplier) // Include Materialsupplier để lấy đơn giá (hoặc lấy thông tin giá từ Material)
                    .ToListAsync(token);

                // Tạo danh sách tất cả các tháng trong khoảng thời gian
                var allMonths = Enumerable.Range(0, ((to.Year - from.Year) * 12 + to.Month - from.Month) + 1)
                             .Select(i => new DateTime(from.Year, from.Month, 1).AddMonths(i))
                             .ToList();

                // Tính tổng tiền cho từng tháng
                var totalByMonth = imports.Where(i => i.Importdetails != null) // Lấy tất cả chi tiết nhập kho
                    .GroupBy(i => new DateTime(i.Receiveddate.Year, i.Receiveddate.Month, 1)) // Nhóm theo tháng
                    .ToDictionary(
                        g => g.Key,
                        g => g.Sum(x => x.Importdetails.Sum(d => (d.Quantity ?? 0) * (d.Materialsupplier?.Price ?? 0))) // Tính tổng tiền (số lượng * đơn giá)
                    );

                // Dự phòng cho những tháng không có dữ liệu, tính giá trị 0
                var totalMaterialCostByMonth = allMonths.Select(date =>
                    totalByMonth.ContainsKey(date) ? totalByMonth[date] : 0
                ).ToList();

                return totalMaterialCostByMonth;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

      


    } }