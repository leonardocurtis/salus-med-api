using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Services.Interfaces;

public interface ITokenService
{
    TokenResult GenerateToken(User user);
}
