using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record PostalCode
{
    public string Value { get; }

    private PostalCode(string value) => Value = value;

    public static PostalCode Create(string value) =>
        IsValid(value)
            ? new PostalCode(value)
            : throw new DomainException(
                "Postal code must contain exactly 8 digits, without mask (e.g. 90000000)."
            );

    public static bool IsValid(string? value) =>
        value is { Length: 8 } && value.All(char.IsAsciiDigit);

    public string Formatted => $"{Value[..5]}-{Value[5..]}";

    public override string ToString() => Value;
}
