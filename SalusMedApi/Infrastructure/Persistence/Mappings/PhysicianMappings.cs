using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class PhysicianMappings : IEntityTypeConfiguration<Physician>
{
    public void Configure(EntityTypeBuilder<Physician> builder)
    {
        builder.ToTable("physicians");

        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Phone).HasMaxLength(20).IsRequired();
        builder.Property(p => p.MedicalRegistration).HasMaxLength(6).IsRequired();
        builder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(p => p.DateOfBirth).IsRequired();
        builder.Property(p => p.Specialty).HasConversion<string>().HasMaxLength(100).IsRequired();
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.HasIndex(p => p.Phone).IsUnique();
        builder.HasIndex(p => p.MedicalRegistration).IsUnique();

        builder.ConfigureAddress(p => p.Address);

        builder
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Physician>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
