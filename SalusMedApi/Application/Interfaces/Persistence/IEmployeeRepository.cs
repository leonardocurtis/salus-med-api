using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IEmployeeRepository
{
    void Add(Employee employee);
    Task<bool> CpfExistsAsync(Cpf cpf);
    Task<bool> EmailExistsAsync(Email email);
    Task<bool> PhoneExistsAsync(Phone phone);
    Task<Employee?> GetEmployeeByEmployeeNumberAsync(string employeeId);
}
