using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IDepartmentService
{
    Task<RegisterDepartmentResponse> RegisterDepartmentAsync(
        RegisterDepartmentRequest request,
        CancellationToken ct = default
    );
    Task<DepartmentDetailsResponse> GetDepartmentDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    );
    Task<PagedResponse<DepartmentListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    );
    Task DeactivateDepartmentAsync(Guid clinicId, CancellationToken ct = default);
    Task<UpdateDepartmentResponse> UpdateDepartmentAsync(
        Guid clinicId,
        UpdateDepartmentRequest request,
        CancellationToken ct = default
    );
    Task ActivateDepartmentAsync(Guid clinicId, CancellationToken ct = default);
}
