using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.UnitTests.Builders;

public static class AddressBuilder
{
    public static Address Default() =>
        Address.Create(
            street: "Avenida Independência",
            number: "1234",
            neighborhood: "Moinhos de Vento",
            postalCode: "90560002",
            complement: "Apartamento 302",
            city: "Porto Alegre",
            state: "RS"
        );
}
