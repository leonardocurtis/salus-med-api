using Microsoft.EntityFrameworkCore;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CpfExistsAsync(string cpf)
    {
        var cpfVo = Cpf.Create(cpf);

        return await _context.Employees.AnyAsync(x => x.CpfNumber == cpfVo);
    }
}
