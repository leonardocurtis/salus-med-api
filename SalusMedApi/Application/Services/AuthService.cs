using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.Services;

public sealed class AuthService(
    ITokenService tokenService,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher
) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user =
            await userRepository.GetUserByUsernameAsync(loginRequest.Username)
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (user.Status == AccountStatus.Deactivated)
            throw new ForbiddenException("Account is inactive.");

        if (!passwordHasher.Verify(loginRequest.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        var token = tokenService.GenerateToken(user);

        return new LoginResponse(user.PublicId, user.Username, token.UserToken, token.ExpiresAt);
    }
}
