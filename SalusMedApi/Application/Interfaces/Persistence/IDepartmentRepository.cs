using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IDepartmentRepository
{
    void Add(Department department);
    Task<Department?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<bool> ExistsByNameInHealthUnitAsync(
        string name,
        long healthUnitId,
        CancellationToken ct = default
    );
    Task<Department?> GetActiveByPublicIdAsync(Guid publicId, CancellationToken ct = default);
}
