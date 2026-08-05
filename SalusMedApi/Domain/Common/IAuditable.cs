namespace SalusMedApi.Domain.Common;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; }
    DateTimeOffset? UpdatedAt { get; }
    DateTimeOffset? DeletedAt { get; }

    public string CreatedBy { get; }
    public string? UpdatedBy { get; }
    public string? DeletedBy { get; }
}
