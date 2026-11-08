namespace SalusMedApi.Application.DTOs.Auth;

public record RefreshResponse(string UserToken, DateTimeOffset ExpiresAt, string RefreshToken);
