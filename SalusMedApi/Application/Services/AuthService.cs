using Microsoft.Extensions.Options;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.CrossCutting.Configuration;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Generators;

namespace SalusMedApi.Application.Services;

public sealed class AuthService(
    ITokenService tokenService,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWorkRepository unitOfWork,
    TimeProvider timeProvider,
    IOptions<JwtSettings> jwtSettings,
    IOptions<LoginLockoutSettings> lockoutSettings
) : IAuthService
{
    private readonly JwtSettings _jwt = jwtSettings.Value;
    private readonly LoginLockoutSettings _lockout = lockoutSettings.Value;

    private const string InvalidCredentials = "Invalid credentials.";
    private const string InvalidRefreshToken = "Invalid refresh token.";

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken ct = default
    )
    {
        if (
            string.IsNullOrWhiteSpace(request.Username)
            || !EmployeeNumberGenerator.IsValid(request.Username)
            || string.IsNullOrEmpty(request.Password)
            || request.Password.Length > 128
        )
            throw new UnauthorizedException(InvalidCredentials);

        var now = timeProvider.GetUtcNow();
        var user = await userRepository.GetUserByUsernameAsync(request.Username);

        var passwordOk = passwordHasher.Verify(
            request.Password,
            user?.PasswordHash ?? passwordHasher.DummyHash
        );

        if (user is null || user.IsLockedOut(now))
            throw new UnauthorizedException(InvalidCredentials);

        if (!passwordOk)
        {
            user.RegisterFailedLogin(
                now,
                _lockout.MaxAttempts,
                TimeSpan.FromSeconds(_lockout.BaseLockoutInSeconds),
                TimeSpan.FromSeconds(_lockout.MaxLockoutInSeconds)
            );

            await unitOfWork.CommitAsync(ct);
            throw new UnauthorizedException(InvalidCredentials);
        }

        if (user.Status == AccountStatus.Deactivated)
            throw new UnauthorizedException(InvalidCredentials);

        user.RegisterSuccessfulLogin();

        var (access, refresh) = IssueTokens(user, familyId: Guid.NewGuid());
        await unitOfWork.CommitAsync(ct);

        return new LoginResponse(
            user.PublicId,
            user.Username,
            access.UserToken,
            access.ExpiresAt,
            refresh.Plain
        );
    }

    public async Task<RefreshResponse> RefreshAsync(
        RefreshRequest request,
        CancellationToken ct = default
    )
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken) || request.RefreshToken.Length > 200)
            throw new UnauthorizedException(InvalidRefreshToken);

        var now = timeProvider.GetUtcNow();
        var hash = tokenService.HashRefreshToken(request.RefreshToken);

        var stored =
            await refreshTokenRepository.GetByHashAsync(hash, ct)
            ?? throw new UnauthorizedException(InvalidRefreshToken);

        if (stored.IsRevoked)
        {
            await RevokeFamilyAsync(stored.FamilyId, now, ct);
            await unitOfWork.CommitAsync(ct);
            throw new UnauthorizedException(InvalidRefreshToken);
        }

        if (stored.IsExpired(now))
            throw new UnauthorizedException(InvalidRefreshToken);

        var user = await userRepository.GetByIdAsync(stored.UserId, ct);
        if (user is null || user.Status == AccountStatus.Deactivated)
        {
            await RevokeFamilyAsync(stored.FamilyId, now, ct);
            await unitOfWork.CommitAsync(ct);
            throw new UnauthorizedException(InvalidRefreshToken);
        }

        var (access, refresh) = IssueTokens(user, stored.FamilyId);
        stored.Revoke(now, replacedByTokenHash: refresh.Hash);
        await unitOfWork.CommitAsync(ct);

        return new RefreshResponse(access.UserToken, access.ExpiresAt, refresh.Plain);
    }

    public async Task LogoutAsync(LogoutRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken) || request.RefreshToken.Length > 200)
            return;

        var hash = tokenService.HashRefreshToken(request.RefreshToken);
        var stored = await refreshTokenRepository.GetByHashAsync(hash, ct);
        if (stored is null)
            return;

        await RevokeFamilyAsync(stored.FamilyId, timeProvider.GetUtcNow().UtcDateTime, ct);
        await unitOfWork.CommitAsync(ct);
    }

    public async Task RevokeAllSessionsAsync(long userId, CancellationToken ct = default)
    {
        var now = timeProvider.GetUtcNow();
        foreach (var token in await refreshTokenRepository.GetActiveByUserAsync(userId, now, ct))
            token.Revoke(now);
    }

    private (TokenResult Access, RefreshTokenValue Refresh) IssueTokens(User user, Guid familyId)
    {
        var now = timeProvider.GetUtcNow();
        var access = tokenService.GenerateToken(user);
        var refresh = tokenService.GenerateRefreshToken();

        refreshTokenRepository.Add(
            RefreshToken.Create(
                user.Id,
                familyId,
                refresh.Hash,
                now,
                TimeSpan.FromHours(_jwt.RefreshTokenExpirationInHours)
            )
        );

        return (access, refresh);
    }

    private async Task RevokeFamilyAsync(Guid familyId, DateTimeOffset now, CancellationToken ct)
    {
        foreach (
            var token in await refreshTokenRepository.GetActiveByFamilyAsync(familyId, now, ct)
        )
            token.Revoke(now);
    }
}
