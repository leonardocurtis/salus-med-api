using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken ct = default);
    Task<RefreshResponse> RefreshAsync(RefreshRequest request, CancellationToken ct = default);
    Task LogoutAsync(LogoutRequest request, CancellationToken ct = default);
    Task RevokeAllSessionsAsync(long userId, CancellationToken ct = default);
}
