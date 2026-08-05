namespace SalusMedApi.Domain.Common;

public abstract class AuditableEntity : Entity, IAuditable
{
    public Guid PublicId { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; protected set; }

    public string CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }
    public string? DeletedBy { get; protected set; }

    protected AuditableEntity() { }
}
