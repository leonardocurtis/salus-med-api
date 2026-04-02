using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Services.Interfaces;
using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(ITokenService tokenService, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    public async Task<RegisterPhysicianResponse> RegisterPhysicianAsync(
        RegisterPhysicianRequest physicianRequest
    )
    {
        throw new NotImplementedException();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user =
            await _userRepository.GetUserByEmailAsync(loginRequest.Email.ToLower())
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (user.Status == AccountStatus.Deactivated)
            throw new ForbiddenException("Account is inactive.");

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse(user.Id, user.Email, token.UserToken, token.ExpiresAt);
    }
}
