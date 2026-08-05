using SalusMedApi.Application.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Cnes
{
    private const int ExpectedLength = 7;
    public string Value { get; }

    private Cnes(string value) => Value = value;

    public static Cnes Create(string cnes)
    {
        if (string.IsNullOrWhiteSpace(cnes))
            throw new DomainException("CNES cannot be empty.");

        var cleaned = cnes.Trim();

        if (cleaned.Length != ExpectedLength)
            throw new DomainException($"CNES must have exactly {ExpectedLength} digits.");

        if (!cleaned.All(char.IsDigit))
            throw new DomainException("CNES must contain only digits.");

        if (cleaned.Distinct().Count() == 1)
            throw new DomainException("CNES is not a valid code.");

        return new Cnes(cleaned);
    }

    public override string ToString() => Value;
}
