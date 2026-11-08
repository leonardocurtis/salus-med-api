namespace SalusMedApi.Application.DTOs.Auth;

public record LoginResponse(
    Guid Id,
    string Username,
    string Token,
    DateTimeOffset ExpiresAt,
    string RefreshToken
);
