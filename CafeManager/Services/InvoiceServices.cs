using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.RightsManagement;

namespace CafeManager.WPF.Services
{
    public class InvoiceServices(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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

        public async Task<(IEnumerable<Invoice>?, int)> GetSearchPaginateListInvoice(Expression<Func<Invoice, bool>>? searchPredicate = null, int skip = 0, int take = 20, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                return await _unitOfWork.InvoiceList.GetByPageAsync(skip, take, searchPredicate, token);
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException("Dừng lấy hóa đơn");
            }
        }

        #endregion Thêm xóa, sửa, tìm kiếm, sắp sếp, phân trang

        public async Task<decimal> GetRevenue(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.InvoiceList.GetTotalRevenueFromTo(from, to);
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

        public async Task<int> GetTotalInvoice(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.InvoiceList.GetTotalInvoiceFromTo(from, to, token);
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
        public async Task<List<decimal>> GetRevenueByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.InvoiceList.GetRevenueByMonth(from, to, token);
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

        public async Task<List<decimal>> GetRevenueByDay(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.InvoiceList.GetRevenueByDay(from, to);
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