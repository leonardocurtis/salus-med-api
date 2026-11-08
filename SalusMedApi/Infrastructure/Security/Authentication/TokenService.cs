using System.Buffers.Text;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.CrossCutting.Configuration;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Security.Authentication;

public sealed class TokenService(IOptions<JwtSettings> jwtSettings, TimeProvider time)
    : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public TokenResult GenerateToken(User user)
    {
        var now = time.GetUtcNow();
        var expiresAt = now.AddMinutes(_jwtSettings.ExpirationInMinutes);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.PublicId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(
                JwtRegisteredClaimNames.Iat,
                now.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
                ClaimValueTypes.Integer64
            ),
        };

        claims.AddRange(user.RoleNames.Select(roleName => new Claim("role", roleName)));

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials
        );

        return new TokenResult(
            UserToken: new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt: expiresAt
        );
    }

    public RefreshTokenValue GenerateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        var plain = Base64Url.EncodeToString(bytes);
        return new RefreshTokenValue(plain, HashRefreshToken(plain));
    }

    public string HashRefreshToken(string plainToken) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(plainToken)));
}
