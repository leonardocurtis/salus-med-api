using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Department;

public record AdminDepartmentListResponse(
    Guid DepartmentId,
    string Name,
    DepartmentStatus Status,
    DateTimeOffset CreateAt
);
