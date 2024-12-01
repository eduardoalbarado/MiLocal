using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Sqlite;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => new { oi.OrderId, oi.ProductId });

        //builder.HasOne(oi => oi.Order)
        //       .WithMany(o => o.OrderItems)
        //       .HasForeignKey(oi => oi.OrderId)
        //       .OnDelete(DeleteBehavior.Cascade);

        builder.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.Discount).HasColumnType("decimal(18,2)");

        builder.HasOne(oi => oi.Product)
               .WithMany()
               .HasForeignKey(oi => oi.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
