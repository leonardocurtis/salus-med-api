using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.DTOs.Address;

public record AddressResponse(
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string PostalCode,
    string City,
    BrazilianState State
);
