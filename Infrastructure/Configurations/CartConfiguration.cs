using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts"); // Set the table name

        builder.HasKey(c => c.Id); // Set the primary key

        builder.Property(c => c.UserId)
            .IsRequired();

        builder.Property(c => c.TotalPrice)
            .HasColumnType("decimal(18,2)") // Set the data type and precision for TotalPrice
            .IsRequired();

        // Configure the relationship with CartItems
        builder.HasMany(c => c.Items)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade); // Set the delete behavior when deleting a cart

        // Additional configurations for the Cart entity can be added here
    }
}

