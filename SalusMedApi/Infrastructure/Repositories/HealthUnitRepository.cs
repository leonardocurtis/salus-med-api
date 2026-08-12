using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class HealthUnitRepository(AppDbContext context) : IHealthUnitRepository
{
    public void Add(HealthUnit healthUnit) => context.HealthUnits.Add(healthUnit);

    public async Task<HealthUnit?> GetByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) => await context.HealthUnits.FirstOrDefaultAsync(x => x.PublicId == publicId, ct);
}
