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
}
