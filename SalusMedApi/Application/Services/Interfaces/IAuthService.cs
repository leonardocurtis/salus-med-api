using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;

namespace SalusMedApi.Application.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterPhysicianResponse> RegisterPhysicianAsync(
        RegisterPhysicianRequest physicianRequest
    );
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}
