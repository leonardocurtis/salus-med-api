using System.Text;
using Microsoft.Extensions.Options;

namespace SalusMedApi.CrossCutting.Configuration;

public sealed class JwtSettingsValidator : IValidateOptions<JwtSettings>
{
    private const int MinSecretKeyBytes = 32;

    public ValidateOptionsResult Validate(string? name, JwtSettings options)
    {
        List<string> failures = [];

        if (
            string.IsNullOrWhiteSpace(options.SecretKey)
            || Encoding.UTF8.GetByteCount(options.SecretKey) < MinSecretKeyBytes
        )
        {
            failures.Add($"Jwt:SecretKey must be at least {MinSecretKeyBytes} bytes.");
        }

        if (options.RefreshTokenExpirationInHours * 60 <= options.ExpirationInMinutes)
        {
            failures.Add(
                "Jwt:RefreshTokenExpirationInHours must be greater than Jwt:ExpirationInMinutes."
            );
        }

        if (options.MaxSessionInDays * 24 < options.RefreshTokenExpirationInHours)
        {
            failures.Add(
                "Jwt:MaxSessionInDays must be greater than or equal to Jwt:RefreshTokenExpirationInHours."
            );
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
