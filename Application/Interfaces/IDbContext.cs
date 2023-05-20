using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;
public interface IDbContext
{
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}