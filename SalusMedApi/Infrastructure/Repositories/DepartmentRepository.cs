using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Extensions;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    public void Add(Department department) => context.Departments.Add(department);

    public async Task<Department?> GetByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) => await context.Departments.ByPublicId(publicId).FirstOrDefaultAsync(ct);

    public async Task<bool> ExistsByNameInHealthUnitAsync(
        string name,
        long healthUnitId,
        CancellationToken ct = default
    ) =>
        await context.Departments.AnyAsync(
            d => d.HealthUnitId == healthUnitId && d.Name.ToLower() == name.ToLower().Trim(),
            ct
        );

    public async Task<Department?> GetActiveByPublicIdAsync(Guid publicId, CancellationToken ct) =>
        await context.Departments.ActiveByPublicId(publicId).FirstOrDefaultAsync(ct);
}
