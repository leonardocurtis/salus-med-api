using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Crm
{
    public string Number { get; }
    public BrazilianState State { get; }

    private Crm(string number, BrazilianState state)
    {
        Number = number;
        State = state;
    }

    public static Crm Create(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
            throw new DomainException("Medical registration cannot be empty.");

        var parts = rawValue.Trim().ToUpperInvariant().Split('/');

        if (parts.Length != 2)
            throw new DomainException("CRM must follow the 'number/UF' format (e.g. 123456/RS).");

        var (numberPart, statePart) = (parts[0], parts[1]);

        if (numberPart.Length == 0 || !numberPart.All(char.IsDigit))
            throw new DomainException("CRM number must contain only digits.");

        if (!Enum.TryParse<BrazilianState>(statePart, out var state))
            throw new DomainException($"'{statePart}' is not a valid Brazilian state.");

        return new Crm(numberPart, state);
    }

    public string Formatted => $"{Number}/{State}";
    public override string ToString() => Formatted;
}
