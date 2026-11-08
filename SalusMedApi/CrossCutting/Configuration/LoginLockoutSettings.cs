using System.ComponentModel.DataAnnotations;

namespace SalusMedApi.CrossCutting.Configuration;

public sealed class LoginLockoutSettings
{
    public const string SectionName = "LoginLockout";

    [Range(1, 20)]
    public int MaxAttempts { get; set; } = 5;

    [Range(1, 3600)]
    public int BaseLockoutInSeconds { get; set; } = 30;

    [Range(1, 86400)]
    public int MaxLockoutInSeconds { get; set; } = 900;
}
