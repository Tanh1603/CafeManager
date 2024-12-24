using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class StaffServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Staff>> GetListStaff(CancellationToken token = default)
        {
            try
            {
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.StaffList.GetAll(token);
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

        public async Task<IEnumerable<Staff>> GetListExistStaff(CancellationToken token = default)
        {
            try
            {
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.StaffList.GetAllExistedAsync();
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

        public async Task<Staff?> CreateStaff(Staff staff)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Staff res = await _unitOfWork.StaffList.Create(staff);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi thêm nhân viên");
            }
        }

        public async Task<Staff?> UpdateStaff(Staff staff)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.StaffList.UpdateStaffWithListSatffSalaryHistory(staff);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi sửa nhân viên");
            }
        }

        public async Task<bool> DeleteStaff(int id, DateOnly? dateOnly)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var deletedStaff = (await _unitOfWork.StaffList.GetById(id));
                if (deletedStaff != null && deletedStaff.Isdeleted == false)
                {
                    deletedStaff.Endworkingdate = dateOnly;
                }
                bool res = await _unitOfWork.StaffList.Delete(id);
                await _unitOfWork.CompleteAsync();
                _unitOfWork.ClearChangeTracker();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Lỗi khi xóa nhân viên");
            }
        }

        public async Task<int> GetTotalStaff(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.StaffList.GetStaffFromTo(from, to, token);
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

        public async Task<decimal> GetTotalSalaryFromTo(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.StaffList.GetTotalSalaryFromTo(from, to, token);
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

        public async Task<List<decimal>> GetTotalSalaryByMonth(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.StaffList.GetTotalSalaryByMonth(from, to, token);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<decimal>> GetTotalSalaryByYear(DateTime from, DateTime to, CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.StaffList.GetTotalSalaryByYear(from, to, token);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}