using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(Email email);
    Task<bool> EmailExistAsync(Email email);
}
