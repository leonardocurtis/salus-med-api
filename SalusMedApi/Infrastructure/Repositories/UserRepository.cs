using Microsoft.EntityFrameworkCore;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(Email email) =>
        await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);

    public async Task<bool> EmailExistAsync(Email email) =>
        await _context.Users.AnyAsync(x => x.EmailAddress == email);
}
