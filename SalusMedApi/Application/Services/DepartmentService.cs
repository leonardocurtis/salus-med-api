using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Services;

public sealed class DepartmentService(
    IUnitOfWorkRepository unitOfWork,
    IDepartmentRepository departmentRepository,
    IHealthUnitRepository healthUnitRepository,
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
    )
    {
        var department =
            await departmentRepository.GetActiveByPublicIdAsync(publicId, ct)
            ?? throw new ResourceNotFoundException("Department not found");

        return department.ToDetails();
    }

    public async Task<PagedResponse<DepartmentListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    )
    {
        var pagedDepartments = await departmentRepository.ListAllActiveAsync(request, ct);

        var dto = pagedDepartments.Content.Select(d => new DepartmentListResponse(
            d.PublicId,
            d.Name,
            d.Status
        ));

        return new PagedResponse<DepartmentListResponse>()
        {
            Content = dto,
            Page = pagedDepartments.Page,
            Size = pagedDepartments.Size,
            TotalElements = pagedDepartments.TotalElements,
            TotalPages = pagedDepartments.TotalPages,
            First = pagedDepartments.First,
            Last = pagedDepartments.Last,
        };
    }

    public async Task DeactivateDepartmentAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department =
            await departmentRepository.GetByPublicIdAsync(departmentId, ct)
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

    public async Task ActivateDepartmentAsync(Guid clinicId, CancellationToken ct = default)
    {
        var department =
            await departmentRepository.GetByPublicIdAsync(clinicId, ct)
            ?? throw new ResourceNotFoundException("Department not found");

        department.Activate();
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<AdminDepartmentDetailsResponse> GetAdminDepartmentDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    )
    {
        var department =
            await departmentRepository.GetByPublicIdAsync(publicId, ct)
            ?? throw new ResourceNotFoundException("Clinic not found");

        return department.ToDetailsAdmin();
    }

    public async Task<PagedResponse<AdminDepartmentListResponse>> ListAllAdminAsync(
        PagedRequest request,
        CancellationToken ct = default
    )
    {
        var pagedClinics = await departmentRepository.ListAllAsync(request, ct);

        var dto = pagedClinics.Content.Select(d => new AdminDepartmentListResponse(
            d.PublicId,
            d.Name,
            d.Status,
            d.CreatedAt
        ));

        return new PagedResponse<AdminDepartmentListResponse>()
        {
            Content = dto,
            Page = pagedClinics.Page,
            Size = pagedClinics.Size,
            TotalElements = pagedClinics.TotalElements,
            TotalPages = pagedClinics.TotalPages,
            First = pagedClinics.First,
            Last = pagedClinics.Last,
        };
    }
}
