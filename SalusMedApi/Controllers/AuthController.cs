using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.CrossCutting.Extensions;
using LoginRequest = SalusMedApi.Application.DTOs.Auth.LoginRequest;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitingPolicies.Login)]
    public async Task<ActionResult<LoginResponse>> SignIn(
        [FromBody] LoginRequest request,
        CancellationToken ct
    ) => Ok(await authService.LoginAsync(request, ct));

    [HttpPost("refresh")]
    [AllowAnonymous]
    [EnableRateLimiting(RateLimitingPolicies.Login)]
    public async Task<ActionResult<RefreshResponse>> Refresh(
        RefreshRequest request,
        CancellationToken ct
    ) => Ok(await authService.RefreshAsync(request, ct));

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken ct)
    {
        await authService.LogoutAsync(request, ct);
        return NoContent();
    }
}
