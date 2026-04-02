namespace SalusMedApi.Application.DTOs.Auth;

public record TokenResult(string UserToken, DateTime ExpiresAt);
