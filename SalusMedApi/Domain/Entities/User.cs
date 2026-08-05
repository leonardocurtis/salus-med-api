using SalusMedApi.Application.Exceptions;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public AccountStatus Status { get; private set; }

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public IEnumerable<string> RoleNames => _userRoles.Select(ur => ur.Role.Name);

    private User() { }

    public static User Create(string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        return new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Status = AccountStatus.Active,
        };
    }

    public void Activate()
    {
        if (Status == AccountStatus.Active)
            throw new DomainException("User account is already active.");

        Status = AccountStatus.Active;
    }

    public void Deactivate()
    {
        if (Status == AccountStatus.Deactivated)
            throw new DomainException("User account is already deactivated.");

        Status = AccountStatus.Deactivated;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash cannot be empty.");

        PasswordHash = newPasswordHash;
    }

    public void AssignRole(Role role)
    {
        if (_userRoles.Any(ur => ur.RoleId == role.Id))
            return;

        _userRoles.Add(new UserRole(this, role));
    }

    public void RemoveRole(Role role)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == role.Id);

        if (userRole is null)
            throw new DomainException($"User does not have the role '{role.Name}'.");

        _userRoles.Remove(userRole);
    }

    public bool HasRole(string roleName) => _userRoles.Any(ur => ur.Role.Name == roleName);
}
