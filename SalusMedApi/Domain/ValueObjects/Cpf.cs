using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Cpf
{
    public string Value { get; }

    private Cpf(string value) => Value = value;

    public static Cpf Create(string cpf) =>
        IsValid(cpf) ? new Cpf(cpf) : throw new DomainException("CPF is invalid.");

    public static bool IsValid(string? cpf) =>
        cpf is { Length: 11 }
        && cpf.All(char.IsAsciiDigit)
        && cpf.Distinct().Count() > 1
        && HasValidCheckDigits(cpf);

    public string Formatted => $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..]}";

    public override string ToString() => Value;

    private static bool HasValidCheckDigits(string digits) =>
        CalculateCheckDigit(digits[..9], weightStart: 10) == digits[9] - '0'
        && CalculateCheckDigit(digits[..10], weightStart: 11) == digits[10] - '0';

    private static int CalculateCheckDigit(string baseDigits, int weightStart)
    {
        var sum = 0;
        var weight = weightStart;

        foreach (var c in baseDigits)
            sum += (c - '0') * weight--;

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
