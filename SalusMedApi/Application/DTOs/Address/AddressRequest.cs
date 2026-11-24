using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Address;

public record AddressRequest(
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string PostalCode,
    string City,
    BrazilianState? State
);
