using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<Department?> GetDepartmentByIdAsync(long id);
}
