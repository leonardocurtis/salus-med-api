using SalusMedApi.Application.DTOs.Employee;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IEmployeeService
{
    Task CreateCredentialsAsync(string employeeId, CreateEmployeeCredentialsRequest request);
}
