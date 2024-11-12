using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Persistence.Contexts;
public class MiLocalDbContext : DbContext, IDbContext
{
    private readonly ILogger<MiLocalDbContext> _logger;
    public MiLocalDbContext(
        DbContextOptions<MiLocalDbContext> options,
        ILogger<MiLocalDbContext> logger) : base(options)
    {
        _logger = logger;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = Assembly.GetExecutingAssembly();

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // Apply only configurations in the Sqlite namespace
            modelBuilder.ApplyConfigurationsFromAssembly(assembly, type =>
                type.Namespace == "Infrastructure.Persistence.Configurations.Sqlite");
        }
        else if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            modelBuilder.HasDefaultSchema("dbo");
            // Apply only configurations in the SqlServer namespace
            modelBuilder.ApplyConfigurationsFromAssembly(assembly, type =>
                type.Namespace == "Infrastructure.Persistence.Configurations.SqlServer");
        }
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<User> Users { get; set; }
}
