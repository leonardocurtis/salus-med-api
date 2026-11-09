using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.Entities;

public class Role : Entity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { }

    public static Role Create(string name, string description) =>
        new()
        {
            Name = NormalizeAndValidateName(name),
            Description = NormalizeAndValidateDescription(description),
        };

    public void UpdateDescription(string description) =>
        Description = NormalizeAndValidateDescription(description);

    private static string NormalizeAndValidateName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new DomainException("Role name cannot be empty.")
            : name.Trim();

    private static string NormalizeAndValidateDescription(string description) =>
        string.IsNullOrWhiteSpace(description)
            ? throw new DomainException("Role description cannot be empty.")
            : description.Trim();
}
