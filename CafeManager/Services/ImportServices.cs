using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CafeManager.WPF.Services
{
    public class ImportServices(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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

        public async Task<Import?> UpdateImport(Import import)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ImportList.UpdateStaffWithListImportDetail(import);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return await _unitOfWork.ImportList.GetById(import.Importid);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi sửa phiếu nhập");
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

        // ===================== Phan trang =======================
        public async Task<(IEnumerable<Import>?, int)> GetSearchPaginateListImport(Expression<Func<Import, bool>>? searchPredicate = null, int skip = 0, int take = 20)
        {
            return await _unitOfWork.ImportList.GetByPageAsync(skip, take, searchPredicate);
        }
    }
}