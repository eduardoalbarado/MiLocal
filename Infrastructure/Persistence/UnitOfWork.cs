// // TODO: Update the unit of work implementation to include operations related to the shopping cart.
using Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly MiLocalDbContext _context;
    private IDbContextTransaction _transaction;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(MiLocalDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IRepositoryAsync<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new RepositoryAsync<TEntity>(_context);
        }

        return (IRepositoryAsync<TEntity>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Log the exception
            throw new Exception("Error saving changes to the database", ex);
        }
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
        {
            return;
        }

        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            _transaction?.Commit();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
