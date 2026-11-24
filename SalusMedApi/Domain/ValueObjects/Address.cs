using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Address
{
    public const int MaxFieldLength = 100;
    public const int MaxNumberLength = 10;

    public string Street { get; private init; } = string.Empty;
    public string Number { get; private init; } = string.Empty;
    public string Neighborhood { get; private init; } = string.Empty;
    public PostalCode PostalCode { get; private init; } = null!;
    public string? Complement { get; private init; }
    public string City { get; private init; } = string.Empty;
    public BrazilianState State { get; private init; }

    private Address() { }

    public static Address Create(
        string street,
        string number,
        string neighborhood,
        string postalCode,
        string? complement,
        string city,
        BrazilianState state
    )
    {
        if (!Enum.IsDefined(state))
            throw new DomainException("Invalid Brazilian state.");

        return new Address
        {
            Street = NormalizeAndValidate(street, nameof(Street), MaxFieldLength),
            Number = NormalizeAndValidate(number, nameof(Number), MaxNumberLength),
            Neighborhood = NormalizeAndValidate(neighborhood, nameof(Neighborhood), MaxFieldLength),
            PostalCode = PostalCode.Create(postalCode),
            Complement = string.IsNullOrWhiteSpace(complement)
                ? null
                : NormalizeAndValidate(complement, nameof(Complement), MaxFieldLength),
            City = NormalizeAndValidate(city, nameof(City), MaxFieldLength),
            State = state,
        };
    }

    private static string NormalizeAndValidate(string? value, string field, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{field} cannot be empty.");

        var trimmed = value.Trim();

        if (trimmed.Length > maxLength)
            throw new DomainException($"{field} cannot exceed {maxLength} characters.");

        return trimmed;
    }
}
