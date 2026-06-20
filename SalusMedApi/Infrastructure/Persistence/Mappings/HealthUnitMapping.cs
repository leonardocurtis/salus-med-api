using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class HealthUnitMapping : IEntityTypeConfiguration<HealthUnit>
{
    public void Configure(EntityTypeBuilder<HealthUnit> builder)
    {
        builder.ToTable("health_units");

        builder.Property(h => h.Cnes).IsRequired().HasMaxLength(7);
        builder.Property(h => h.Cnpj).IsRequired().HasMaxLength(14);
        builder.Property(h => h.TechnicalManagerName).IsRequired().HasMaxLength(200);
        builder.Property(h => h.TechnicalManagerCouncilNumber).IsRequired().HasMaxLength(20);
        builder.Property(h => h.Phone).IsRequired().HasMaxLength(20);
        builder.Property(h => h.Status).IsRequired().HasMaxLength(50).HasConversion<string>();

        builder.HasIndex(h => h.Cnes).IsUnique();
        builder.HasIndex(h => h.Cnpj).IsUnique();

        builder.ConfigureAddress(e => e.Address);
        builder
            .HasOne(h => h.Clinic)
            .WithMany()
            .HasForeignKey(h => h.ClinicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
