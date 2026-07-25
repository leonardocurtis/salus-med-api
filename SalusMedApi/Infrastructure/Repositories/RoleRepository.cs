using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class RoleRepository(AppDbContext dbContext) : IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name) =>
        await dbContext.Roles.FirstOrDefaultAsync(role => role.Name == name);
}
