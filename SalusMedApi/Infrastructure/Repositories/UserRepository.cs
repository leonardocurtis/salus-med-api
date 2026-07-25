using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public void Add(User user) => context.Users.Add(user);

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await context
            .Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.Username == username);
    }
}
