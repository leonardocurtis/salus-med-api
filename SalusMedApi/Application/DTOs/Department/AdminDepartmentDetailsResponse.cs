using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Department;

public record AdminDepartmentDetailsResponse(
    Guid DepartmentId,
    string Name,
    DepartmentStatus Status,
    DateTimeOffset CreateAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt,
    string CreatedBy,
    string? UpdatedBy,
    string? DeletedBy
);
