using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Infrastructure.Persistence.Converters;

public class CpfConverter : ValueConverter<Cpf, string>
{
    public CpfConverter()
        : base(cpf => cpf.Value, value => Cpf.Create(value)) { }
}
