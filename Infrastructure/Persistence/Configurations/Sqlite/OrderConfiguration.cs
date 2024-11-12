using Domain.Entities;
using Infrastructure.Persistence.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Sqlite;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserId).IsRequired();
        builder.Property(o => o.OrderDate).IsRequired();
        builder.Property(o => o.ShippingMethod).HasMaxLength(100);
        builder.Property(o => o.ShippingAddress).HasMaxLength(250);
        builder.Property(o => o.PaymentMethod).HasMaxLength(50);

        // Configure Location to use a WKT string format, using HasConversion
        builder.Property(o => o.Location)
               .HasColumnType("TEXT")
               .HasMaxLength(100)
               .HasConversion(
                   v => WktConverter.ToWkt(v.Latitude, v.Longitude), // Convert to WKT on save
                   v => WktConverter.FromWkt(v)                     // Convert from WKT on retrieval
               );

        builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
        //Configure the relationship with OrderItem
        builder.HasMany(o => o.OrderItems)
               .WithOne(oi => oi.Order)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}

