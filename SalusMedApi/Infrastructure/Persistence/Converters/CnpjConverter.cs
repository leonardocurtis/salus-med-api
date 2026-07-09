using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Infrastructure.Persistence.Converters;

public sealed class CnpjConverter : ValueConverter<Cnpj, string>
{
    public CnpjConverter()
        : base(cnpj => cnpj.Value, value => Cnpj.Create(value)) { }
}
