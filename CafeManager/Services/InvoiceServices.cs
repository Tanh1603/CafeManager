using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class InvoiceServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        // Hàm load tất cả bill
        public async Task<IEnumerable<Invoice>> GetListInvoices()
        {
            return await _unitOfWork.InvoiceList.GetAllInvoiceAsync();
        }

        public async Task<Invoice?> GetInvoiceById(int id)
        {
            return await _unitOfWork.InvoiceList.GetInvoicesByIdAsync(id);
        }

        public async Task<IEnumerable<Invoicedetail>?> GetListIvoiceDetailByInvoiceId(int id)
        {
            return await _unitOfWork.InvoiceList.GetAllInvoiceDetailByInvoiceIdAsync(id);
        }

        #region Thêm xóa, sửa, tìm kiếm, sắp sếp, phân trang

        public async Task<Invoice?> AddInvoice(Invoice invoice)
        {
            var res = await _unitOfWork.InvoiceList.Create(invoice);
            if (res != null)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            bool res = await _unitOfWork.InvoiceList.Delete(id);
            if (res)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public Invoice? UpdateInvoice(Invoice invoice)
        {
            var res = _unitOfWork.InvoiceList.Update(invoice);
            if (res != null)
            {
                _unitOfWork.Complete();
            }
            return res;
        }

        public async Task<IEnumerable<Invoice>?> GetSearchSortPaginateListInvoice(Expression<Func<Invoice, bool>>? searchPredicate = null,
                                                            Expression<Func<Invoice, object>>? sortKeySelector = null,
                                                            bool ascending = true, int skip = 0, int take = 20)
        {
            return await _unitOfWork.InvoiceList.SearchSortPaginateAsync(searchPredicate, sortKeySelector, ascending, skip, take);
        }

        #endregion Thêm xóa, sửa, tìm kiếm, sắp sếp, phân trang

        #region tính toán doanh thu của bill

        public async Task<decimal?> GetTotalPriceByInvoiceId(int id)
        {
            var res = await _unitOfWork.InvoiceList.GetAllInvoiceDetailByInvoiceIdAsync(id);
            return CaculateTotalPriceFromInvoiceDetail(res);
        }

        private decimal? CaculateTotalPriceFromInvoiceDetail(IEnumerable<Invoicedetail>? invoicedetail)
        {
            return invoicedetail?.Sum(x =>
            {
                decimal? discountInvoice = (100 - x.Invoice.Discountinvoice) / 100;
                decimal? foodPrice = x.Food.Price;
                decimal? foodDiscount = (100 - x.Food.Discountfood) / 100;
                decimal? quantity = x.Quantity;

                return discountInvoice * foodDiscount * foodPrice * quantity;
            }) ?? decimal.Zero;
        }

        public async Task<decimal?> GetTotalPriceFromAllInvoices()
        {
            decimal? totalMoney = decimal.Zero;
            var listInvoices = await _unitOfWork.InvoiceList.GetAllInvoiceAsync();
            foreach (var invoice in listInvoices)
            {
                totalMoney += CaculateTotalPriceFromInvoiceDetail(invoice.Invoicedetails);
            }

            return totalMoney;
        }

        #endregion tính toán doanh thu của bill
    }
}