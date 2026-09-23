using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Department;

public record DepartmentListResponse(Guid Id, string Name, DepartmentStatus Status);
