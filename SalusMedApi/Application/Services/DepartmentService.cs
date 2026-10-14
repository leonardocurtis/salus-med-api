using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;

namespace SalusMedApi.Application.Services;

public sealed class DepartmentService(
    IUnitOfWorkRepository unitOfWork,
    IDepartmentRepository departmentRepository,
    IHealthUnitRepository healthUnitRepository,
    IDepartmentQueries departmentQueries,
    ICurrentUserService currentUser
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

        var department = request.ToEntity(healthUnit);

        departmentRepository.Add(department);
        await unitOfWork.CommitAsync(ct);

        return department.ToResponse();
    }

    public async Task<DepartmentDetailsResponse> GetDepartmentDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await departmentQueries.GetActiveDetailsByPublicIdAsync(publicId, ct)
        ?? throw new ResourceNotFoundException("Department not found");

    public async Task<PagedResponse<DepartmentListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) => await departmentQueries.ListAllActiveAsync(request, ct);

    public async Task DeactivateDepartmentAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department =
            await departmentRepository.GetAnyStatusByPublicIdAsync(departmentId, ct)
            ?? throw new ResourceNotFoundException("Department not found");

        department.Deactivate(currentUser.EmployeeIdNumber ?? "SYSTEM");
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<UpdateDepartmentResponse> UpdateDepartmentAsync(
        Guid departmentId,
        UpdateDepartmentRequest request,
        CancellationToken ct = default
    )
    {
        var department =
            await departmentRepository.GetActiveByPublicIdAsync(departmentId, ct)
            ?? throw new ResourceNotFoundException($"Department '{departmentId}' not found.");

        department.Rename(request.Name);
        await unitOfWork.CommitAsync(ct);

        return department.ToUpdate();
    }

    public async Task ActivateDepartmentAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department =
            await departmentRepository.GetAnyStatusByPublicIdAsync(departmentId, ct)
            ?? throw new ResourceNotFoundException("Department not found");

        department.Activate();
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<AdminDepartmentDetailsResponse> GetAdminDepartmentDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await departmentQueries.GetAdminDetailsByPublicIdAsync(publicId, ct)
        ?? throw new ResourceNotFoundException("Department not found");

    public async Task<PagedResponse<AdminDepartmentListResponse>> ListAllAdminAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) => await departmentQueries.ListAllByAnyStatusAsync(request, ct);
}
