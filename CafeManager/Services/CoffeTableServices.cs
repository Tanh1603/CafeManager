using CafeManager.Core.Data;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class CoffeTableServices(IUnitOfWork unitOfWork)
    {
        private IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Coffeetable>> GetListCoffeTable(CancellationToken token = default)
        {
            try
            {
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.CoffeeTableList.GetAll(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Coffeetable>> GetListExistCoffeeTable(CancellationToken token = default)
        {
            try
            {
                _unitOfWork.ClearChangeTracker();
                return await _unitOfWork.CoffeeTableList.GetAllExistedAsync(token);
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

        public async Task<Coffeetable?> AddCoffeTable(Coffeetable coffeetable)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.CoffeeTableList.Create(coffeetable);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw;
            }
        }

        public async Task<Coffeetable?> UpdateCoffeeTable(Coffeetable coffeetable)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var res = await _unitOfWork.CoffeeTableList.Update(coffeetable);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCoffeeTable(int id)
        {
            var res = await _unitOfWork.CoffeeTableList.Delete(id);
            _unitOfWork.Complete();
            return res;
        }

        public async Task<int> GetTotalTable(CancellationToken token = default)
        {
            try
            {
                return await _unitOfWork.CoffeeTableList.GetTotalTable(token);
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