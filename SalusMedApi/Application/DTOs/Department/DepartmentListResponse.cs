using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Department;

public record DepartmentListResponse(Guid DepartmentId, string Name, DepartmentStatus Status);
