using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken token);
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct = default);
    Task<List<RefreshToken>> GetActiveByFamilyAsync(
        Guid familyId,
        DateTimeOffset now,
        CancellationToken ct = default
    );
    Task<List<RefreshToken>> GetActiveByUserAsync(
        long userId,
        DateTimeOffset now,
        CancellationToken ct = default
    );
}
