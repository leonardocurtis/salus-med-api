using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Services;

public class DepartmentService(
    IUnitOfWorkRepository unitOfWork,
    IDepartmentRepository departmentRepository,
    IHealthUnitRepository healthUnitRepository
) : IDepartmentService
{
    public async Task<RegisterDepartmentResponse> RegisterDepartmentAsync(
        RegisterDepartmentRequest request,
        CancellationToken ct = default
    )
    {
        var healthUnit =
            await healthUnitRepository.GetByPublicIdAsync(request.HealthUnitId, ct)
            ?? throw new ResourceNotFoundException("Health Unit not found.");

        if (
            await departmentRepository.ExistsByNameInHealthUnitAsync(
                request.Name,
                healthUnit.Id,
                ct
            )
        )
            throw new ConflictException($"Department {request.Name} exists in this health unit.");

        var department = Department.Create(request.Name, healthUnit);

        departmentRepository.Add(department);
        await unitOfWork.CommitAsync(ct);

        return new RegisterDepartmentResponse(department.PublicId, department.Name);
    }

    public async Task<DepartmentDetailsResponse> GetDepartmentDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    )
    {
        var department =
            await departmentRepository.GetActiveByPublicIdAsync(publicId, ct)
            ?? throw new ResourceNotFoundException("Department not found");

        return new DepartmentDetailsResponse(department.PublicId, department.Name);
    }
}
