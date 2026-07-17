using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(Email email);
    Task<bool> EmailExistAsync(Email email);
}
