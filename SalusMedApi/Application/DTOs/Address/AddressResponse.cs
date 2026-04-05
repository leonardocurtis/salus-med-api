namespace SalusMedApi.Application.DTOs.Address;

public record AddressResponse(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string PostalCode,
    string City,
    string State
);
