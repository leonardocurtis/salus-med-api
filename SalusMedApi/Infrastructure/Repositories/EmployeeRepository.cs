using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public void Add(Employee employee) => context.Employees.Add(employee);

    public async Task<bool> CpfExistsAsync(Cpf cpf) =>
        await context.Employees.AnyAsync(x => x.CpfNumber == cpf);

    public async Task<bool> EmailExistsAsync(Email email) =>
        await context.Employees.AnyAsync(x => x.EmailAddress == email);

    public async Task<bool> PhoneExistsAsync(Phone phone) =>
        await context.Employees.AnyAsync(x => x.PhoneNumber == phone);

    public async Task<Employee?> GetEmployeeByEmployeeNumberAsync(string employeeId) =>
        await context
            .Employees.Include(e => e.User)
            .FirstOrDefaultAsync(x => x.EmployeeNumber == employeeId);
}
