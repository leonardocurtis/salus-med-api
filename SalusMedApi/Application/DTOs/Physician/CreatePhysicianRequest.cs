using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Physician;

public record CreatePhysicianRequest(
    string Name,
    string Phone,
    string MedicalRegistration,
    string Cpf,
    Gender Gender,
    DateOnly DateOfBirth,
    Specialty Specialty,
    AddressRequest Address
) { }
