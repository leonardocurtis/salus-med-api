using SalusMedApi.Application.DTOs.Employee;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task CreateCredentialsAsync(
        Guid publicId,
        CreateEmployeeCredentialsRequest request,
        CancellationToken ct = default
    );
}
