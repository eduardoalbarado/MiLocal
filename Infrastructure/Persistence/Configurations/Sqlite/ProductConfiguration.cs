using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Sqlite;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.ShortName)
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(p => p.PriceInDollars)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(p => p.Cost)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(p => p.CostInDollars)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(p => p.Enabled)
            .IsRequired();

        builder.Property(p => p.Kit)
            .IsRequired();

        builder.HasMany(p => p.ProductCategories)
            .WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}