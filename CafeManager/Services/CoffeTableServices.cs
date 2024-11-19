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
    public class CoffeTableServices
    {
        private IServiceProvider _provider;
        private IUnitOfWork _unitOfWork;

        public CoffeTableServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<IEnumerable<Coffeetable>> GetListCoffeTable()
        {
            return await _unitOfWork.CoffeeTableList.GetAllCoffeTableAsync();
        }

        public async Task<IEnumerable<Invoice>> GetListInvoicesByCoffeeTableId(int id)
        {
            return await _unitOfWork.CoffeeTableList.GetAllInvoicesByCoffeeTableIdAsync(id);
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

        public Coffeetable? UpdateCoffeeTable(Coffeetable coffeetable)
        {
            var res = _unitOfWork.CoffeeTableList.Update(coffeetable);
            _unitOfWork.Complete();
            return res;
        }

        public async Task<bool> DeleteCoffeeTable(int id)
        {
            var res = await _unitOfWork.CoffeeTableList.Delete(id);
            _unitOfWork.Complete();
            return res;
        }
    }
}