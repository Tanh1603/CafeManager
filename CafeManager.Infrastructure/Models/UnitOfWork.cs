using CafeManager.Core.Data;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

#nullable disable

namespace CafeManager.Infrastructure.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CafeManagerContext _context;
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

        public IMaterialSupplierRepository MaterialSupplierList { get; private set; }

        #endregion Import

        public IStaffRepository StaffList { get; private set; }

        public IAppUserRepository AppUserList { get; private set; }

        public IRepository<Staffsalaryhistory> StaffSalaryHistoryList { get; private set; }

        public IConsumedMaterialRepository ConsumedMaterialList { get; private set; }

        public UnitOfWork(IDbContextFactory<CafeManagerContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();

            FoodCategoryList = new FoodCategoryRepository(_context);
            FoodList = new FoodRepository(_context);
            CoffeeTableList = new CoffeeTableRepository(_context);
            InvoiceList = new InvoicesRepository(_context);
            InvoiceDetail = new Repository<Invoicedetail>(_context);

            ImportList = new ImportRepository(_context);
            ImportDetailList = new Repository<Importdetail>(_context);
            MaterialList = new MaterialRepository(_context);
            SupplierList = new SupplierRepository(_context);
            MaterialSupplierList = new MaterialSupplierRepository(_context);
            ConsumedMaterialList = new ConsumedMaterialRepository(_context);

            StaffList = new StaffRepository(_context);
            AppUserList = new AppUserRepository(_context);
            StaffSalaryHistoryList = new Repository<Staffsalaryhistory>(_context);
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
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
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

        public void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }
    }
}