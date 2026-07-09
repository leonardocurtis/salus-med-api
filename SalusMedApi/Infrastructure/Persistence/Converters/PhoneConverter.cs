using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Infrastructure.Persistence.Converters;

public sealed class PhoneConverter : ValueConverter<Phone, string>
{
    public PhoneConverter()
        : base(phone => phone.Value, value => Phone.Create(value)) { }
}
