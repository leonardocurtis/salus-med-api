using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Services;

public interface ITokenService
{
    TokenResult GenerateToken(User user);
}
