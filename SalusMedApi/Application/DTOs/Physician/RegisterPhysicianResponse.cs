using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Physician;

public record RegisterPhysicianResponse(
    Guid Id,
    string EmployeeNumber,
    string Name,
    string Crm,
    Specialty Specialty,
    EmployeeStatus Status,
    DateTimeOffset CreatedAt
);
