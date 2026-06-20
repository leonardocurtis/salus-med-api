using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Department : IAuditable
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public DepartmentStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public long HealthUnitId { get; private set; }
    public HealthUnit HealthUnit { get; private set; }

    private Department() { }

    public static Department Create(string name, HealthUnit healthUnit) =>
        new Department
        {
            Name = name.Trim(),
            HealthUnit = healthUnit,
            Status = DepartmentStatus.Active,
        };

    public void Rename(string newName) => Name = newName.Trim();

    public void MarkAsActive() => Status = DepartmentStatus.Active;

    public void MarkAsDeactivated() => Status = DepartmentStatus.Deactivated;
}
