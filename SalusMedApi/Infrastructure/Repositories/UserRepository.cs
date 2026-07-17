using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(Email email)
    {
        return await _context
            .Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(x => x.EmailAddress == email);
    }

    public async Task<bool> EmailExistAsync(Email email) =>
        await _context.Users.AnyAsync(x => x.EmailAddress == email);
}
