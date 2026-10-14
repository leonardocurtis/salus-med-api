using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IDepartmentQueries
{
    Task<DepartmentDetailsResponse?> GetActiveDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    );

    Task<AdminDepartmentDetailsResponse?> GetAdminDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    );

    Task<PagedResponse<DepartmentListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    );

    Task<PagedResponse<AdminDepartmentListResponse>> ListAllByAnyStatusAsync(
        PagedRequest request,
        CancellationToken ct = default
    );
}
