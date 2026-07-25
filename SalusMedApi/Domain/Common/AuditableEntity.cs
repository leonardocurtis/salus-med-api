namespace SalusMedApi.Domain.Common;

public abstract class AuditableEntity : Entity, IAuditable
{
    public Guid PublicId { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    protected AuditableEntity() { }
}
