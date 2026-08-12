using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IHealthUnitRepository
{
    void Add(HealthUnit healthUnit);
    Task<HealthUnit?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default);
}
