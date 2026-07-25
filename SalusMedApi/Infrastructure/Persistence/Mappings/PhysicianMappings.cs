using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class PhysicianMappings : AuditableEntityMapping<Physician>
{
    public override void Configure(EntityTypeBuilder<Physician> builder)
    {
        base.Configure(builder);

        builder.ToTable("physicians");

        builder.ComplexProperty(
            p => p.MedicalRegistration,
            crm =>
            {
                crm.Property(c => c.Number)
                    .HasColumnName("crm_number")
                    .HasMaxLength(6)
                    .IsRequired();

                crm.Property(c => c.State)
                    .HasColumnName("crm_state")
                    .HasConversion<string>()
                    .HasMaxLength(2)
                    .IsRequired();
            }
        );
        builder.Property(p => p.Specialty).HasConversion<string>().HasMaxLength(100).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder
            .HasOne(p => p.Employee)
            .WithOne()
            .HasForeignKey<Physician>(p => p.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
