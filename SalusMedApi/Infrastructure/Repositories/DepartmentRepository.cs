using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    public void Add(Department department) => context.Departments.Add(department);

    public async Task<Department?> GetByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) => await context.Departments.FirstOrDefaultAsync(d => d.PublicId == publicId, ct);

    public async Task<bool> ExistsByNameInHealthUnitAsync(
        string name,
        long healthUnitId,
        CancellationToken ct = default
    ) =>
        await context.Departments.AnyAsync(
            d => d.HealthUnitId == healthUnitId && d.Name.ToLower() == name.ToLower().Trim(),
            ct
        );

    public async Task<Department?> GetActiveByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await context.Departments.FirstOrDefaultAsync(
            c => c.PublicId == publicId && c.Status == DepartmentStatus.Active,
            ct
        );
}
