using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Auth;

public interface ITokenService
{
    TokenResult GenerateToken(User user);
}
