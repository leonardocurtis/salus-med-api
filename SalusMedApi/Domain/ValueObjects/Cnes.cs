using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Cnes
{
    public string Value { get; }

    private Cnes(string value) => Value = value;

    public static bool IsValid(string? cnes) =>
        cnes is { Length: 7 } && cnes.All(char.IsAsciiDigit);

    public static Cnes Create(string cnes) =>
        IsValid(cnes)
            ? new Cnes(cnes)
            : throw new DomainException("CNES must contain exactly 7 digits.");

    public override string ToString() => Value;
}
