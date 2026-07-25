namespace SalusMedApi.Application.DTOs.Auth;

public record LoginResponse(long UserId, string Username, string Token, DateTime ExpiresAt);
