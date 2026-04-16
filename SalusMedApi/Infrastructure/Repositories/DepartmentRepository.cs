using Microsoft.EntityFrameworkCore;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Department?> GetDepartmentByIdAsync(long id) =>
        await _context.Departments.FirstOrDefaultAsync(x => x.Id == id);
}
