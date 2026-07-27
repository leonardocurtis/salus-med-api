namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IUnitOfWorkRepository
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
