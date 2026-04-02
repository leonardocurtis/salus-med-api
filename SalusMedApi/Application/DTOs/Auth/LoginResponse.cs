namespace SalusMedApi.Application.DTOs.Auth;

public record LoginResponse(long UserId, string Email, string Token, DateTime ExpiresAt);
