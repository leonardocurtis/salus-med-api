using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Mapper;

public static class DepartmentMapper
{
    public static Department ToEntity(
        this RegisterDepartmentRequest request,
        HealthUnit healthUnitId
    ) => Department.Create(name: request.Name, healthUnit: healthUnitId);

    extension(Department department)
    {
        public RegisterDepartmentResponse ToResponse() => new(department.PublicId, department.Name);

        public DepartmentDetailsResponse ToDetails() => new(department.PublicId, department.Name);

        public UpdateDepartmentResponse ToUpdate() => new(department.PublicId, department.Name);

        public AdminDepartmentDetailsResponse ToDetailsAdmin() =>
            new(
                department.PublicId,
                department.Name,
                department.Status,
                department.CreatedAt,
                department.UpdatedAt,
                department.DeletedAt,
                department.CreatedBy,
                department.UpdatedBy,
                department.DeletedBy
            );
    }
}
