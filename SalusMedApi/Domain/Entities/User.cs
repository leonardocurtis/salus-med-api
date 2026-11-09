using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.Exceptions;

namespace SalusMedApi.Domain.Entities;

public class User : AuditableEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public AccountStatus Status { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTimeOffset? LockedUntil { get; private set; }

    private readonly List<UserRole> _userRoles = [];
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public IEnumerable<string> RoleNames => _userRoles.Select(ur => ur.Role.Name);

    private User() { }

    public static User Create(string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

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

    public void EnsureDeactivated()
    {
        if (Status != AccountStatus.Deactivated)
            Deactivate();
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash cannot be empty.");

        PasswordHash = newPasswordHash;
    }

    public bool IsLockedOut(DateTimeOffset now) => LockedUntil is { } until && until > now;

    public void RegisterFailedLogin(
        DateTimeOffset now,
        int maxAttempts,
        TimeSpan baseLockout,
        TimeSpan maxLockout
    )
    {
        if (maxAttempts < 1)
            throw new DomainException("Max attempts must be at least 1.");

        if (baseLockout <= TimeSpan.Zero)
            throw new DomainException("Base lockout must be positive.");

        if (maxLockout < baseLockout)
            throw new DomainException("Max lockout cannot be shorter than base lockout.");

        if (IsLockedOut(now))
            return;

        FailedLoginAttempts++;

        if (FailedLoginAttempts < maxAttempts)
            return;

        var doublings = Math.Min(FailedLoginAttempts - maxAttempts, 30);
        var seconds = Math.Min(
            baseLockout.TotalSeconds * Math.Pow(2, doublings),
            maxLockout.TotalSeconds
        );

        LockedUntil = now + TimeSpan.FromSeconds(seconds);
    }

    public void RegisterSuccessfulLogin()
    {
        FailedLoginAttempts = 0;
        LockedUntil = null;
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
