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
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;

        public StaffServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Staff>> GetListStaff()
        {
            return await _unitOfWork.StaffList.GetAll();
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
    }
}