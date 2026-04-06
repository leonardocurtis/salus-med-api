using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Patient;

public record RegisterPatientResponse(
    long PatientId,
    string Name,
    string Phone,
    string Cpf,
    Gender Gender,
    DateOnly DateOfBirth,
    AddressResponse Address,
    PatientStatus Status,
    DateTimeOffset CreatedAt
);
