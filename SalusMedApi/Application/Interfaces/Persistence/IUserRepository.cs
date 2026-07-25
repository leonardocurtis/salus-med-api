using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IUserRepository
{
    void Add(User user);

    Task<User?> GetUserByUsernameAsync(string username);
}
