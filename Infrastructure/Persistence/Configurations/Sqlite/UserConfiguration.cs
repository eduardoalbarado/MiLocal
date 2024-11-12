using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations.Sqlite;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User"); // Set the table name

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnType("TEXT")
            .IsRequired()
            .HasConversion(new GuidToStringConverter());

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.B2CUserId)
            .HasMaxLength(100);

        builder.Property(u => u.GmailId)
            .HasMaxLength(100);

        builder.Property(u => u.FacebookId)
            .HasMaxLength(100);

        builder.HasMany(u => u.Carts)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
