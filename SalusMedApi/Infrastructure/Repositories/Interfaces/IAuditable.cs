namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}
