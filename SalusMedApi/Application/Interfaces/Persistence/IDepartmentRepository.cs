using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IDepartmentRepository
{
    Task<Department?> GetDepartmentByIdAsync(long id);
}
