using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.UnitTests.Builders;

public static class HealthUnitBuilder
{
    public static HealthUnit Active() => Build();

    public static HealthUnit PendingRegistration() => Build(cnes: "");

    public static HealthUnit Build(
        string cnes = "1234567",
        string cnpj = "11444777000161",
        string managerName = "Carlos Silva",
        string managerNumber = "123456/RS",
        string phone = "5133334444",
        Address? address = null
    ) =>
        HealthUnit.Create(
            cnes: cnes,
            cnpj: cnpj,
            managerName: managerName,
            managerNumber: managerNumber,
            address: address ?? AddressBuilder.Default(),
            phone: phone
        );
}
