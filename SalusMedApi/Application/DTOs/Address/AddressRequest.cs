namespace SalusMedApi.Application.DTOs.Address;

public record AddressRequest(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string PostalCode,
    string City,
    string State
) { }
