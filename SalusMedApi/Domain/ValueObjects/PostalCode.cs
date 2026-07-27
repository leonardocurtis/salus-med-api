using System.Text.RegularExpressions;
using SalusMedApi.CrossCutting.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record PostalCode
{
    public string Value { get; }

    private static readonly Regex CepRegex = new(@"^\d{5}-?\d{3}$", RegexOptions.Compiled);

    private PostalCode(string value) => Value = value;

    public static PostalCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Postal code cannot be empty.");

        var trimmed = value.Trim();

        if (!CepRegex.IsMatch(trimmed))
            throw new DomainException(
                "Postal code must match the Brazilian CEP format (e.g. 90000-000)."
            );

        return new PostalCode(trimmed.Replace("-", ""));
    }

    public string Formatted => Value.Length == 8 ? $"{Value[..5]}-{Value[5..]}" : Value;
    public override string ToString() => Value;
}
