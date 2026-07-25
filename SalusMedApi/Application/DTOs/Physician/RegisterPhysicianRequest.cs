using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Physician;

public record RegisterPhysicianRequest(
    string Name,
    string Phone,
    string Email,
    string Crm,
    string Cpf,
    Gender? Gender,
    DateOnly? DateOfBirth,
    Specialty? Specialty,
    AddressRequest Address,
    long DepartmentId
);
