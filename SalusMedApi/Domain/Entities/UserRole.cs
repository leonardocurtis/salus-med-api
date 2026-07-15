using SalusMedApi.CrossCutting.Exceptions;

namespace SalusMedApi.Domain.Entities;

public class UserRole
{
    public long UserId { get; private set; }
    public long RoleId { get; private set; }

    public Role Role { get; private set; } = null!;
    public User User { get; private set; } = null!;

    private UserRole() { }

    public UserRole(long userId, long roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}
