namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
