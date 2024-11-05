using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.Infrastructure.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CafeManagerContext _context;
        private readonly IDbContextFactory<CafeManagerContext> dbContextFactory;

        private IDbContextTransaction _transaction;
        private bool _isTransactionActive;
        private int _transactionDepth = 0;

        #region food

        public IFoodCategoryRepository FoodCategoryList { get; private set; }
        public IFoodRepository FoodList { get; private set; }

        public IInvoicesRepository InvoiceList { get; private set; }

        public IRepository<Invoicedetail> InvoiceDetail { get; private set; }

        public ICoffeeTableRepository CoffeeTableList { get; private set; }

        #endregion food

        #region Import

        public IImportRepository ImportList { get; private set; }

        public IRepository<Importdetail> ImportDetailList { get; private set; }

        public IMaterialRepository MaterialList { get; private set; }

        public ISupplierRepository SupplierList { get; private set; }

        public IRepository<Materialsupplier> MaterialSupplierList { get; private set; }

        #endregion Import

        public UnitOfWork(IDbContextFactory<CafeManagerContext> dbContextFactory)
        {
            _context = dbContextFactory.CreateDbContext();

            FoodCategoryList = new FoodCategoryRepository(_context);
            FoodList = new FoodRepository(_context);
            CoffeeTableList = new CoffeeTableRepository(_context);
            InvoiceList = new InvoicesRepository(_context);
            InvoiceDetail = new Repository<Invoicedetail>(_context);

            ImportList = new ImportRepository(_context);
            ImportDetailList = new Repository<Importdetail>(_context);
            MaterialList = new MaterialRepository(_context);
            SupplierList = new SupplierRepository(_context);
            MaterialSupplierList = new Repository<Materialsupplier>(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            if (!_isTransactionActive)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                _isTransactionActive = true;
            }
            _transactionDepth++;
        }

        public async Task CommitTransactionAsync()
        {
            if (_isTransactionActive && --_transactionDepth == 0)
            {
                await _transaction!.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                _isTransactionActive = false;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_isTransactionActive)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
                _isTransactionActive = false;
                _transactionDepth = 0;
            }
        }
    }
}