using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence.Converters;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class EmployeeMapping : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder
            .Property(e => e.PhoneNumber)
            .HasConversion(new PhoneConverter())
            .HasMaxLength(20)
            .HasColumnName("phone")
            .IsRequired();
        builder
            .Property(e => e.CpfNumber)
            .HasConversion(new CpfConverter())
            .HasMaxLength(11)
            .HasColumnName("cpf")
            .IsRequired();
        builder.Property(e => e.Gender).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(e => e.DateOfBirth).IsRequired();
        builder.Property(e => e.Status).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(e => e.Role).IsRequired().HasMaxLength(50).HasConversion<string>();

        builder.HasIndex(e => e.PhoneNumber).IsUnique();
        builder.HasIndex(e => e.CpfNumber).IsUnique();

        builder.ConfigureAddress(e => e.Address);

        builder
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<Employee>(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
