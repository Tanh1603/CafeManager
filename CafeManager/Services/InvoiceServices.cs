﻿using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace CafeManager.WPF.Services
{
    public class InvoiceServices(IServiceProvider provider)
    {
        private readonly IServiceProvider _provider = provider;
        private readonly IUnitOfWork _unitOfWork = provider.GetRequiredService<IUnitOfWork>();

        // Hàm load tất cả bill
        public async Task<IEnumerable<Invoice>> GetListInvoices()
        {
            return await _unitOfWork.InvoiceList.GetAllExistedAsync();
        }

        public async Task<Invoice?> GetInvoiceById(int id)
        {
            return await _unitOfWork.InvoiceList.GetById(id);
        }

        #region Thêm xóa, sửa, tìm kiếm, sắp sếp, phân trang

        public async Task<Invoice?> CreateInvoice(Invoice invoice)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.InvoiceList.Create(invoice);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi");
            }
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

        public async Task<Invoice?> UpdateInvoice(Invoice invoice)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.InvoiceList.Update(invoice);
                _unitOfWork.Complete();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi");
            }
        }

        public async Task<(IEnumerable<Invoice>?, int)> GetSearchPaginateListInvoice(Expression<Func<Invoice, bool>>? searchPredicate = null, int skip = 0, int take = 20)
        {
            return await _unitOfWork.InvoiceList.GetByPageAsync(skip, take, searchPredicate);
        }

        #endregion Thêm xóa, sửa, tìm kiếm, sắp sếp, phân trang

        #region tính toán doanh thu của bill

        public async Task<decimal?> GetTotalPriceByInvoiceId(int id)
        {
            var res = await _unitOfWork.InvoiceList.GetById(id);
            return CaculateTotalPriceFromInvoiceDetail(res?.Invoicedetails) ?? 0;
        }

        public decimal? CaculateTotalPriceFromInvoiceDetail(IEnumerable<Invoicedetail>? invoicedetail)
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
            var listInvoices = await _unitOfWork.InvoiceList.GetAllExistedAsync();
            foreach (var invoice in listInvoices)
            {
                totalMoney += CaculateTotalPriceFromInvoiceDetail(invoice.Invoicedetails);
            }

            return totalMoney;
        }

        #endregion tính toán doanh thu của bill

        public async Task<IEnumerable<Invoicedetail>> AddArangeListInvoiceDetail(IEnumerable<Invoicedetail> invoicedetail)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.InvoiceDetail.AddArange(invoicedetail);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi");
            }
        }
    }
}