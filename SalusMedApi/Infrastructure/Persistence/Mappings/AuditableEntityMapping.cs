using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Common;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public abstract class AuditableEntityMapping<TEntity> : EntityMapping<TEntity>
    where TEntity : AuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.PublicId).IsRequired().ValueGeneratedNever();

        builder.HasIndex(e => e.PublicId).IsUnique();

        builder.Property(e => e.CreatedBy).IsRequired().HasMaxLength(11);

        builder.Property(e => e.UpdatedBy).HasMaxLength(11);

        builder.Property(e => e.DeletedBy).HasMaxLength(11);
    }
}
