using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.Entities;

public sealed class RefreshToken : Entity
{
    public long UserId { get; private set; }
    public Guid FamilyId { get; private set; }
    public string TokenHash { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }

    private RefreshToken() { }

    public bool IsRevoked => RevokedAt is not null;

    public bool IsExpired(DateTimeOffset now) => now >= ExpiresAt;

    public bool IsActive(DateTimeOffset now) => !IsRevoked && !IsExpired(now);

    public static RefreshToken Create(
        long userId,
        Guid familyId,
        string tokenHash,
        DateTimeOffset now,
        TimeSpan lifetime
    )
    {
        if (now.Offset != TimeSpan.Zero)
            throw new DomainException("Date must be in UTC.");

        if (userId <= 0)
            throw new DomainException("Refresh token must be linked to a valid user.");

        if (familyId == Guid.Empty)
            throw new DomainException("Refresh token family id cannot be empty.");

        ValidateTokenHash(tokenHash, "Token hash");

        if (lifetime <= TimeSpan.Zero)
            throw new DomainException("Refresh token lifetime must be greater than zero.");

        return new RefreshToken
        {
            UserId = userId,
            FamilyId = familyId,
            TokenHash = tokenHash,
            CreatedAt = now,
            ExpiresAt = now.Add(lifetime),
        };
    }

    public void Revoke(DateTimeOffset now, string? replacedByTokenHash = null)
    {
        if (now.Offset != TimeSpan.Zero)
            throw new DomainException("Date must be in UTC.");

        if (IsRevoked)
            throw new DomainException("Refresh token is already revoked.");

        if (replacedByTokenHash is not null)
        {
            ValidateTokenHash(replacedByTokenHash, "Replacement token hash");

            if (replacedByTokenHash == TokenHash)
                throw new DomainException("Refresh token cannot be replaced by itself.");
        }

        if (now < CreatedAt)
            throw new DomainException("Revocation date cannot be before the creation date.");

        RevokedAt = now;
        ReplacedByTokenHash = replacedByTokenHash;
    }

    private static void ValidateTokenHash(string? hash, string fieldName)
    {
        const int tokenHashLength = 64;

        if (string.IsNullOrWhiteSpace(hash))
            throw new DomainException($"{fieldName} cannot be empty.");

        if (hash.Length != tokenHashLength)
            throw new DomainException($"{fieldName} must have {tokenHashLength} characters.");
    }
}
