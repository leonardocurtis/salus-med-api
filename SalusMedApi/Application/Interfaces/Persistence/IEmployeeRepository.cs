using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IEmployeeRepository
{
    void Add(Employee employee);
    Task<bool> CpfExistsAsync(Cpf cpf, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(Email email, CancellationToken ct = default);
    Task<bool> PhoneExistsAsync(Phone phone, CancellationToken ct = default);
    Task<Employee?> GetEmployeeByEmployeeNumberAsync(
        string employeeId,
        CancellationToken ct = default
    );
}
