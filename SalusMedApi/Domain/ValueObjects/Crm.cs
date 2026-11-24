using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Crm
{
    public const int MinNumberLength = 4;
    public const int MaxNumberLength = 9;

    public string Number { get; }
    public BrazilianState State { get; }

    private Crm(string number, BrazilianState state)
    {
        Number = number;
        State = state;
    }

    public static bool IsValidNumber(string? number) =>
        number is { Length: >= MinNumberLength and <= MaxNumberLength }
        && number.All(char.IsAsciiDigit);

    public static Crm Create(string number, BrazilianState state)
    {
        if (!IsValidNumber(number))
            throw new DomainException(
                $"CRM number must contain {MinNumberLength} to {MaxNumberLength} digits."
            );

        if (!Enum.IsDefined(state))
            throw new DomainException("CRM state (UF) is invalid.");

        return new Crm(number, state);
    }

    public string Formatted => $"{Number}/{State}";

    public override string ToString() => Formatted;
}
