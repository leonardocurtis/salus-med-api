using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Patient;

public record RegisterPatientResponse(
    long Id,
    string Name,
    string MotherName,
    string Cpf,
    PatientStatus Status,
    DateTimeOffset CreatedAt
);
