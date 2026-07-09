using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence.Converters;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class PatientMapping : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients");

        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
        builder.Property(p => p.MotherName).HasMaxLength(100).IsRequired();
        builder.Property(p => p.FatherName).HasMaxLength(100);
        builder
            .Property(p => p.PhoneNumber)
            .HasConversion(new PhoneConverter())
            .HasMaxLength(20)
            .HasColumnName("phone")
            .IsRequired();
        builder
            .Property(p => p.CpfCode)
            .HasConversion(new CpfConverter())
            .HasMaxLength(11)
            .HasColumnName("cpf")
            .IsRequired();
        builder.Property(p => p.Gender).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(p => p.DateOfBirth).IsRequired();
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.HasIndex(p => p.PhoneNumber).IsUnique();
        builder.HasIndex(p => p.CpfCode).IsUnique();

        builder.ConfigureAddress(p => p.Address);

        builder
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<Patient>(p => p.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
