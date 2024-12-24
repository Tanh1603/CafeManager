using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Repositories;
using CafeManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Repositories
{
    public class FoodRepository : Repository<Food>, IFoodRepository
    {
        public FoodRepository(CafeManagerContext cafeManagerContext) : base(cafeManagerContext)
        {
        }

        public async Task<List<FoodDTO>> GetMostSoldFoods(DateTime From, DateTime To, CancellationToken token = default)
        {
            try
            {
                var mostSoldFoods = await _cafeManagerContext.Invoicedetails
                                .Join(_cafeManagerContext.Invoices,
                                    id => id.Invoiceid,   // Khóa ngoại từ InvoiceDetail
                                    inv => inv.Invoiceid, // Khóa chính từ Invoice
                                    (id, inv) => new { InvoiceDetail = id, Invoice = inv })
                                .Where(joined => joined.Invoice.Paymentstartdate >= From && joined.Invoice.Paymentstartdate <= To)
                                .GroupBy(joined => joined.InvoiceDetail.Foodid)
                                .Select(g => new
                                {
                                    FoodId = g.Key,
                                    TotalQuantity = g.Sum(j => j.InvoiceDetail.Quantity)
                                })
                                .OrderByDescending(g => g.TotalQuantity)
                                .Take(10)
                                .ToListAsync(token);

                // Lấy danh sách FoodId từ kết quả trên
                var foodIds = mostSoldFoods.Select(msf => msf.FoodId).ToList();

                // Lấy thông tin chi tiết các món ăn từ bảng Food
                var foods = await _cafeManagerContext.Foods
                    .Where(f => foodIds.Contains(f.Foodid))
                    .ToListAsync(token);

                // Ánh xạ sang FoodDTO và thêm thông tin TotalSold
                var foodDTOs = foods.Select(f => new FoodDTO
                {
                    Foodname = f.Foodname,
                    Price = f.Price ?? 0 // Đảm bảo xử lý null
                }).ToList();

                return foodDTOs;
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

        public async Task<int> GetTotalFood(CancellationToken token = default)
        {
            try
            {
                return await _cafeManagerContext.Foods.Where(x => x.Isdeleted == false).CountAsync(token);
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
    }
}