using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class User : IAuditable
{
    public long Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public AccountStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public User() { }

    public static User Create(string email, string passwordHash, Role role) =>
        new User
        {
            Email = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            Role = role,
            Status = AccountStatus.Active,
        };

    public void MarkAsActive() => Status = AccountStatus.Active;

    public void MarkAsDeactivate() => Status = AccountStatus.Deactivated;

    public void ChangePassword(string newPasswordHash) => PasswordHash = newPasswordHash;

    public void ChangeEmail(string email) => Email = email.Trim().ToLower();

    public void ChangeRole(Role role) => Role = role;
}
