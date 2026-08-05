using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IClinicService
{
    Task<RegisterClinicResponse> RegisterClinicAsync(
        RegisterClinicRequest request,
        CancellationToken ct = default
    );
    Task<PagedResponse<ClinicListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    );
    Task<PagedResponse<AdminClinicListResponse>> ListAllAdminAsync(
        PagedRequest request,
        CancellationToken ct = default
    );
    Task<ClinicDetailsResponse> GetClinicDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    );
    Task<AdminClinicDetailsResponse> GetAdminClinicDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    );
    Task DeactivateClinicAsync(Guid clinicId, CancellationToken ct = default);
    Task<UpdateClinicResponse> UpdateClinicAsync(
        Guid clinicId,
        UpdateClinicRequest request,
        CancellationToken ct = default
    );
    Task SuspendClinicAsync(Guid clinicId, CancellationToken ct = default);
    Task ActivateClinicAsync(Guid clinicId, CancellationToken ct = default);
}
