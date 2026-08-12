using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public void Add(Employee employee) => context.Employees.Add(employee);

    public async Task<bool> CpfExistsAsync(Cpf cpf, CancellationToken ct = default) =>
        await context.Employees.AnyAsync(x => x.CpfNumber == cpf, ct);

    public async Task<bool> EmailExistsAsync(Email email, CancellationToken ct = default) =>
        await context.Employees.AnyAsync(x => x.EmailAddress == email, ct);

    public async Task<bool> PhoneExistsAsync(Phone phone, CancellationToken ct = default) =>
        await context.Employees.AnyAsync(x => x.PhoneNumber == phone, ct);

    public async Task<Employee?> GetEmployeeByEmployeeNumberAsync(
        string employeeId,
        CancellationToken ct = default
    ) =>
        await context
            .Employees.Include(e => e.User)
            .FirstOrDefaultAsync(x => x.EmployeeNumber == employeeId, ct);
}
