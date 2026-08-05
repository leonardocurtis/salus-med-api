using SalusMedApi.Application.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Cpf
{
    public string Value { get; }

    private Cpf(string value) => Value = value;

    public static Cpf Create(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("CPF cannot be empty.");

        var digitsOnly = new string(cpf.Where(char.IsDigit).ToArray());

        if (digitsOnly.Length != 11)
            throw new DomainException("CPF must contain 11 digits.");

        if (HasAllSameDigits(digitsOnly))
            throw new DomainException("CPF is invalid.");

        if (!HasValidCheckDigits(digitsOnly))
            throw new DomainException("CPF is invalid.");

        return new Cpf(digitsOnly);
    }

    private static bool HasAllSameDigits(string digits) => digits.Distinct().Count() == 1;

    private static bool HasValidCheckDigits(string digits)
    {
        var firstCheckDigit = CalculateCheckDigit(digits[..9], weightStart: 10);
        if (firstCheckDigit != digits[9] - '0')
            return false;

        var secondCheckDigit = CalculateCheckDigit(digits[..10], weightStart: 11);
        return secondCheckDigit == digits[10] - '0';
    }

    private static int CalculateCheckDigit(string baseDigits, int weightStart)
    {
        var sum = 0;
        var weight = weightStart;

        foreach (var c in baseDigits)
        {
            sum += (c - '0') * weight;
            weight--;
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

    public string Formatted => $"{Value[..3]}.{Value[3..6]}.{Value[6..9]}-{Value[9..]}";

    public override string ToString() => Value;
}
