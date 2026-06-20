using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class DepartmentMapping : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.HasIndex(d => d.Name).IsUnique();

        builder
            .HasOne(d => d.HealthUnit)
            .WithMany()
            .HasForeignKey(d => d.HealthUnitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
