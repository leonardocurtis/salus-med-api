using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Common;

namespace SalusMedApi.Domain.Entities;

public class Role : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { }

    public static Role Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Role description cannot be empty.");

        return new Role { Name = name.Trim(), Description = description.Trim() };
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Role description cannot be empty.");

        Description = description.Trim();
    }
}
