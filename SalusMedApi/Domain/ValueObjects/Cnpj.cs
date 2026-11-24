using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Cnpj
{
    public string Value { get; }

    private Cnpj(string value) => Value = value;

    public static bool IsValid(string? cnpj) =>
        cnpj is { Length: 14 }
        && cnpj[..12].All(IsDigitOrUpperLetter)
        && cnpj[12..].All(char.IsAsciiDigit)
        && cnpj.Distinct().Count() > 1
        && HasValidCheckDigits(cnpj);

    public static Cnpj Create(string cnpj) =>
        IsValid(cnpj) ? new Cnpj(cnpj) : throw new DomainException("CNPJ is invalid.");

    public string Formatted =>
        $"{Value[..2]}.{Value[2..5]}.{Value[5..8]}/{Value[8..12]}-{Value[12..]}";

    public override string ToString() => Value;

    private static bool IsDigitOrUpperLetter(char c) =>
        char.IsAsciiDigit(c) || char.IsAsciiLetterUpper(c);

    private static bool HasValidCheckDigits(string cnpj) =>
        CalculateCheckDigit(cnpj[..12]) == cnpj[12] - '0'
        && CalculateCheckDigit(cnpj[..13]) == cnpj[13] - '0';

    private static int CalculateCheckDigit(string baseChars)
    {
        var sum = 0;
        var weight = 2;

        for (var i = baseChars.Length - 1; i >= 0; i--)
        {
            sum += (baseChars[i] - '0') * weight;
            weight = weight == 9 ? 2 : weight + 1;
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
