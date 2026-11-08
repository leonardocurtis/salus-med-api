using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalusMedApi.CrossCutting.Configuration;

namespace SalusMedApi.CrossCutting.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddOptions<LoginLockoutSettings>()
            .BindConfiguration(LoginLockoutSettings.SectionName)
            .ValidateDataAnnotations()
            .Validate(
                s => s.MaxLockoutInSeconds >= s.BaseLockoutInSeconds,
                "MaxLockoutInSeconds must be >= BaseLockoutInSeconds."
            )
            .ValidateOnStart();

        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IValidateOptions<JwtSettings>, JwtSettingsValidator>()
        );

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services
            .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtSettings>>(
                (options, jwtOptions) =>
                {
                    var jwt = jwtOptions.Value;

                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwt.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwt.Audience,

                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.FromSeconds(30),

                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt.SecretKey)
                        ),

                        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],

                        NameClaimType = JwtRegisteredClaimNames.UniqueName,
                        RoleClaimType = "role",
                    };
                }
            );

        return services;
    }
}
