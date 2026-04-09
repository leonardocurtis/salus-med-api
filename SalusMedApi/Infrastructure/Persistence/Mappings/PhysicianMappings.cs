using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class PhysicianMappings : IEntityTypeConfiguration<Physician>
{
    public void Configure(EntityTypeBuilder<Physician> builder)
    {
        builder.ToTable("physicians");

        builder.Property(p => p.MedicalRegistration).HasMaxLength(6).IsRequired();
        builder.Property(p => p.Specialty).HasConversion<string>().HasMaxLength(100).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.HasIndex(p => p.MedicalRegistration).IsUnique();

        builder
            .HasOne(p => p.Employee)
            .WithOne()
            .HasForeignKey<Physician>(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
