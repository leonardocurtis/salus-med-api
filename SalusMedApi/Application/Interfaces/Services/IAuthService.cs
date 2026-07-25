using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}
