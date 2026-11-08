using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public void Add(RefreshToken token) => context.RefreshTokens.Add(token);

    public async Task<RefreshToken?> GetByHashAsync(
        string tokenHash,
        CancellationToken ct = default
    ) => await context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, ct);

    public async Task<List<RefreshToken>> GetActiveByFamilyAsync(
        Guid familyId,
        DateTimeOffset now,
        CancellationToken ct = default
    ) =>
        await context
            .RefreshTokens.Where(t =>
                t.FamilyId == familyId && t.RevokedAt == null && t.ExpiresAt > now
            )
            .ToListAsync(ct);

    public async Task<List<RefreshToken>> GetActiveByUserAsync(
        long userId,
        DateTimeOffset now,
        CancellationToken ct = default
    ) =>
        await context
            .RefreshTokens.Where(t =>
                t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > now
            )
            .ToListAsync(ct);
}
