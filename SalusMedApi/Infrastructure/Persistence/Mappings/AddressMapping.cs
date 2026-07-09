using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence.Converters;

namespace SalusMedApi.Infrastructure.Persistence.Mappings;

public static class AddressMapping
{
    public static void ConfigureAddress<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, Address>> property
    )
        where TEntity : class
    {
        builder.ComplexProperty(
            property,
            address =>
            {
                address
                    .Property(a => a.Street)
                    .HasColumnName("street")
                    .HasMaxLength(100)
                    .IsRequired();
                address
                    .Property(a => a.Number)
                    .HasColumnName("number")
                    .HasMaxLength(10)
                    .IsRequired();
                address.Property(a => a.Complement).HasColumnName("complement").HasMaxLength(100);
                address
                    .Property(a => a.Neighborhood)
                    .HasColumnName("neighborhood")
                    .HasMaxLength(100)
                    .IsRequired();
                address.ComplexProperty(
                    a => a.PostalCode,
                    postalCodeBuilder =>
                    {
                        postalCodeBuilder
                            .Property(p => p.Value)
                            .HasColumnName("postal_code")
                            .HasMaxLength(8)
                            .IsRequired();
                    }
                );
                address.Property(a => a.City).HasColumnName("city").HasMaxLength(100).IsRequired();
                address
                    .Property(a => a.State)
                    .HasColumnName("state")
                    .HasConversion<string>()
                    .HasMaxLength(2)
                    .IsRequired();
            }
        );
    }
}
