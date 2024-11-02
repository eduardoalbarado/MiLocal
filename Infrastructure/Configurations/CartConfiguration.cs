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

        builder.Property(c => c.Created)
            .IsRequired()
            .HasColumnType("DATETEXT"); // Assuming Created should not be null

        builder.Property(c => c.LastModified)
            .HasColumnType("DATETEXT"); // LastModified can be nullable, set its type to TEXT


        builder.HasOne(c => c.User)
            .WithMany(p => p.Carts)
            .HasForeignKey(pc => pc.UserId)
            .IsRequired();

        // Configure the relationship with CartItems
        builder.HasMany(c => c.Items)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade); // Set the delete behavior when deleting a cart

    }
}
