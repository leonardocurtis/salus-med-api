using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Domain.ValueObjects;

public sealed record Address
{
    public string Street { get; private init; } = null!;
    public string Number { get; private init; } = null!;
    public string Neighborhood { get; private init; } = null!;
    public PostalCode PostalCode { get; private init; } = null!;
    public string? Complement { get; private init; }
    public string City { get; private init; } = null!;
    public BrazilianState State { get; private init; }

    private Address() { }

    public static Address Create(
        string street,
        string number,
        string neighborhood,
        string postalCode,
        string? complement,
        string city,
        string state
    )
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be empty.");
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("Number cannot be empty.");
        if (string.IsNullOrWhiteSpace(neighborhood))
            throw new DomainException("Neighborhood cannot be empty.");
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City cannot be empty.");
        if (!Enum.TryParse<BrazilianState>(state.Trim().ToUpperInvariant(), out var parsedState))
            throw new DomainException("Invalid Brazilian state.");

        return new Address
        {
            Street = street.Trim(),
            Number = number.Trim(),
            Neighborhood = neighborhood.Trim(),
            PostalCode = PostalCode.Create(postalCode),
            Complement = complement?.Trim(),
            City = city.Trim(),
            State = parsedState,
        };
    }
}
