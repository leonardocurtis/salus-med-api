using System.ComponentModel.DataAnnotations;

namespace SalusMedApi.CrossCutting.Configuration;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    [Required]
    public string SecretKey { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(1, 60)]
    public int ExpirationInMinutes { get; init; } = 15;

    [Range(1, 72)]
    public int RefreshTokenExpirationInHours { get; init; } = 12;

    [Range(1, 90)]
    public int MaxSessionInDays { get; init; } = 30;
}
