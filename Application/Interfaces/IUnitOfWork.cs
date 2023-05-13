using Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepositoryAsync<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    void RollbackTransaction();
}