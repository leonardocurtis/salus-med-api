using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public class EmployeeMapping : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Phone).HasMaxLength(20).IsRequired();
        builder.Property(e => e.Cpf).HasMaxLength(11).IsRequired();
        builder.Property(e => e.Gender).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(e => e.DateOfBirth).IsRequired();
        builder.Property(e => e.Status).IsRequired().HasMaxLength(50).HasConversion<string>();
        builder.Property(e => e.Role).IsRequired().HasMaxLength(50).HasConversion<string>();

        builder.HasIndex(e => e.Phone).IsUnique();
        builder.HasIndex(e => e.Cpf).IsUnique();

        builder.ConfigureAddress(e => e.Address);

        builder
            .HasOne(e => e.User)
            .WithOne()
            .HasForeignKey<Employee>(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.Department)
            .WithMany()
            .HasForeignKey(e => e.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
