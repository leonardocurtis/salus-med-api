using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class UnitOfWorkRepository(AppDbContext context) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken cancellationToken = default) =>
        await context.SaveChangesAsync(cancellationToken);
}
