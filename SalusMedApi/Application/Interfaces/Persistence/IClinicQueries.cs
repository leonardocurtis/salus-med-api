using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IClinicQueries
{
    Task<PagedResponse<AdminClinicListResponse>> ListAllByAnyStatusAsync(
        PagedRequest request,
        CancellationToken ct = default
    );

    Task<PagedResponse<ClinicListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    );

    Task<AdminClinicDetailsResponse?> GetAdminDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    );

    Task<ClinicDetailsResponse?> GetActiveDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    );
}
