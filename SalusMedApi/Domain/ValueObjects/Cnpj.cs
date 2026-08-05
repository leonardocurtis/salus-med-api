using System.Text.RegularExpressions;
using SalusMedApi.Application.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Cnpj
{
    public string Value { get; }

    private Cnpj(string value) => Value = value;

    public static Cnpj Create(string cnpj)
    {
        var digits = Regex.Replace(cnpj ?? string.Empty, @"[^\d]", "");

        if (!IsValid(digits))
            throw new DomainException($"'{cnpj}' is not a valid CNPJ.");

        return new Cnpj(digits);
    }

    private static bool IsValid(string digits)
    {
        if (digits.Length != 14)
            return false;

        if (digits.Distinct().Count() == 1)
            return false;

        return ValidateDigit(digits, 12) && ValidateDigit(digits, 13);
    }

    private static bool ValidateDigit(string digits, int position)
    {
        int[] weights =
            position == 12
                ? [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]
                : [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = digits.Take(position).Select((ch, i) => (ch - '0') * weights[i]).Sum();

        var remainder = sum % 11;
        var expected = remainder < 2 ? 0 : 11 - remainder;

        return (digits[position] - '0') == expected;
    }

    public string Formatted =>
        $"{Value[..2]}.{Value[2..5]}.{Value[5..8]}/{Value[8..12]}-{Value[12..]}";

    public override string ToString() => Value;
}
