using Microsoft.EntityFrameworkCore;
using Npgsql;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class UnitOfWorkRepository(AppDbContext context, ILogger<UnitOfWorkRepository> logger)
    : IUnitOfWorkRepository
{
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException
                    is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } pgEx
            )
        {
            logger.LogWarning(
                ex,
                "Unique constraint violated. Constraint: {ConstraintName}, Table: {TableName}",
                pgEx.ConstraintName,
                pgEx.TableName
            );

            throw new ConflictException("A record with the same unique value already exists.", ex);
        }
    }
}
