using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> EmailExistAsync(string email);
    Task<User> SaveAsync(User user);
}
