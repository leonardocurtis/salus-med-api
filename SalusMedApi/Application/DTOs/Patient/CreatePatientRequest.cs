using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Patient;

public record CreatePatientRequest(
    string Name,
    string Phone,
    string Cpf,
    Gender? Gender,
    DateOnly? DateOfBirth,
    AddressRequest Address
);
