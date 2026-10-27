using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Generators;

namespace SalusMedApi.Application.Services;

public sealed class AuthService(
    ITokenService tokenService,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher
) : IAuthService
{
    private readonly string _dummyHash = passwordHasher.Hash("dummy-password-for-timing");

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (
            string.IsNullOrWhiteSpace(request.Username)
            || !EmployeeNumberGenerator.IsValid(request.Username)
            || string.IsNullOrEmpty(request.Password)
            || request.Password.Length > 128
        )
            throw new UnauthorizedException("Invalid credentials.");

        var user = await userRepository.GetUserByUsernameAsync(request.Username);

        var passwordOk = passwordHasher.Verify(request.Password, user?.PasswordHash ?? _dummyHash);

        if (user is null || !passwordOk || user.Status == AccountStatus.Deactivated)
            throw new UnauthorizedException("Invalid credentials.");

        var token = tokenService.GenerateToken(user);
        return new LoginResponse(user.PublicId, user.Username, token.UserToken, token.ExpiresAt);
    }
}
