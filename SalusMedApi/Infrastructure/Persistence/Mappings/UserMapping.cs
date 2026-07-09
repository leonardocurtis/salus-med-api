using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder
            .Property(u => u.EmailAddress)
            .HasConversion(e => e.Value, v => Email.Create(v))
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(60);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(u => u.Status).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(u => u.CreatedAt).IsRequired();

        builder.HasIndex(u => u.EmailAddress).IsUnique();
    }
}
