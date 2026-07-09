using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence.Converters;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class ClinicMapping : IEntityTypeConfiguration<Clinic>
{
    public void Configure(EntityTypeBuilder<Clinic> builder)
    {
        builder.ToTable("clinics");

        builder.Property(c => c.CorporateName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.TradeName).HasMaxLength(200);
        builder
            .Property(c => c.CnpjCode)
            .HasConversion(new CnpjConverter())
            .IsRequired()
            .HasMaxLength(14)
            .HasColumnName("cnpj");
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.HasIndex(c => c.CnpjCode).IsUnique();
    }
}
