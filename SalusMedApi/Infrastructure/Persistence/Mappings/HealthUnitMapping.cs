using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence.Converters;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class HealthUnitMapping : AuditableEntityMapping<HealthUnit>
{
    public override void Configure(EntityTypeBuilder<HealthUnit> builder)
    {
        base.Configure(builder);

        builder.ToTable("health_units");

        builder
            .Property(h => h.CnesCode)
            .HasConversion(
                cnes => cnes != null ? cnes.Value : null,
                value => value != null ? Cnes.Create(value) : null
            )
            .HasColumnName("cnes")
            .HasMaxLength(7);
        builder
            .Property(h => h.CnpjCode)
            .HasConversion(new CnpjConverter())
            .IsRequired()
            .HasMaxLength(14)
            .HasColumnName("cnpj");
        builder.Property(h => h.TechnicalManager).IsRequired().HasMaxLength(200);
        builder.ComplexProperty(
            h => h.TechnicalManagerCouncilNumber,
            registration =>
            {
                registration.Property(r => r.Number).HasColumnName("technical_manager_crm");
                registration
                    .Property(r => r.State)
                    .HasConversion<string>()
                    .HasColumnName("technical_manager_state");
            }
        );
        builder
            .Property(h => h.PhoneNumber)
            .HasConversion(new PhoneConverter())
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("phone");
        builder.Property(h => h.Status).IsRequired().HasMaxLength(50).HasConversion<string>();

        builder.HasIndex(h => h.CnesCode).IsUnique().HasFilter("cnes IS NOT NULL");
        builder.HasIndex(h => h.CnpjCode).IsUnique();

        builder.ConfigureAddress(e => e.Address);
        builder
            .HasOne(h => h.Clinic)
            .WithMany()
            .HasForeignKey(h => h.ClinicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
