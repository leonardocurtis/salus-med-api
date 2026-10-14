using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SalusMedApi.Application.Interfaces.Auth;

namespace SalusMedApi.Infrastructure.Security.Authentication;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => accessor.HttpContext?.User;
    public string? UserId => User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public string? EmployeeIdNumber => User?.FindFirstValue(JwtRegisteredClaimNames.UniqueName);

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
