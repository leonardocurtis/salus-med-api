namespace SalusMedApi.Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Neighborhood { get; private set; }
    public string PostalCode { get; private set; }
    public string? Complement { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }

    private Address() { }

    public static Address Create(
        string street,
        string number,
        string neighborhood,
        string postalCode,
        string? complement,
        string city,
        string state
    ) =>
        new Address
        {
            Street = street.Trim(),
            Number = number.Trim(),
            Neighborhood = neighborhood.Trim(),
            PostalCode = postalCode.Trim(),
            Complement = complement?.Trim(),
            City = city.Trim(),
            State = state.Trim(),
        };

    public void Update(
        string street,
        string number,
        string neighborhood,
        string postalCode,
        string? complement,
        string city,
        string state
    )
    {
        Street = street.Trim();
        Number = number.Trim();
        Neighborhood = neighborhood.Trim();
        PostalCode = postalCode.Trim();
        Complement = complement?.Trim();
        City = city.Trim();
        State = state.Trim();
    }
}
