// TODO: Update the base repository to include additional repository methods related to the shopping cart.
using Application.Interfaces;
using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repository;
public class RepositoryAsync<T> : RepositoryBase<T>, IRepositoryAsync<T> where T : class
{
    private readonly MiLocalDbContext dbContext;
    private IDbContextTransaction _transaction;

    public RepositoryAsync(MiLocalDbContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        _transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }
}
