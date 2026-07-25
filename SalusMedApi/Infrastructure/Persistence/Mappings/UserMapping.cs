using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class UserMapping : AuditableEntityMapping<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.ToTable("users");

        builder.Property(u => u.Username).IsRequired().HasMaxLength(20);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(60);
        builder.Property(u => u.Status).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(u => u.CreatedAt).IsRequired();

        builder.HasIndex(u => u.Username).IsUnique();
    }
}
