using SalusMedApi.CrossCutting.Exceptions;
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

    public static Department Create(string name, HealthUnit healthUnit)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Department name is required.");

        return new Department
        {
            Name = name.Trim(),
            HealthUnit = healthUnit,
            Status = DepartmentStatus.Active,
        };
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Department name is required.");

        Name = newName.Trim();
    }

    public void Activate()
    {
        if (Status == DepartmentStatus.Active)
            throw new DomainException("Department is already active.");

        Status = DepartmentStatus.Active;
    }

    public void Deactivate()
    {
        if (Status != DepartmentStatus.Active)
            throw new DomainException($"Cannot deactivate a department in status '{Status}'.");

        Status = DepartmentStatus.Deactivated;
    }
}
