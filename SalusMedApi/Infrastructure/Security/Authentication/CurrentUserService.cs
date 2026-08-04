using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SalusMedApi.Application.Interfaces.Auth;

namespace SalusMedApi.Infrastructure.Security.Authentication;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    public string? UserId => accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? EmployeeIdNumber =>
        accessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.UniqueName);

    public bool IsInRole(string role) => accessor.HttpContext?.User.IsInRole(role) ?? false;
}
