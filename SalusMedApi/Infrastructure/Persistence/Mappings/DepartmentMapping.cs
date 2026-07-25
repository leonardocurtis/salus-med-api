using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class DepartmentMapping : AuditableEntityMapping<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        base.Configure(builder);

        builder.ToTable("departments");

        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.HasIndex(d => new { d.HealthUnitId, d.Name }).IsUnique();

        builder
            .HasOne(d => d.HealthUnit)
            .WithMany()
            .HasForeignKey(d => d.HealthUnitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
