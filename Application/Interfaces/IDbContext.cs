using Domain.Entities;

namespace Application.Interfaces;
public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}