using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class User : IAuditable
{
    public long Id { get; private set; }
    public Email EmailAddress { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public AccountStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash, Role role)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        return new User
        {
            EmailAddress = Email.Create(email),
            PasswordHash = passwordHash,
            Role = role,
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

    public void ChangeEmail(string newEmail)
    {
        var email = Email.Create(newEmail);

        if (email.Equals(EmailAddress))
            throw new DomainException("The new email must be different from the current one.");

        EmailAddress = email;
    }

    public void ChangeRole(Role newRole)
    {
        if (Role == newRole)
            throw new DomainException($"User already has the role '{newRole}'.");

        Role = newRole;
    }
}
