using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name);
}
