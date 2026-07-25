using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Mapper;

public static class AddressMapper
{
    public static AddressResponse ToResponse(this Address address)
    {
        return new AddressResponse(
            address.Street,
            address.Number,
            address.Complement,
            address.Neighborhood,
            address.PostalCode.Formatted(),
            address.City,
            address.State
        );
    }
}
