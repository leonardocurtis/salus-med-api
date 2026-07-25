using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Interfaces.Services;
using LoginRequest = SalusMedApi.Application.DTOs.Auth.LoginRequest;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> SignIn([FromBody] LoginRequest dto)
    {
        var response = await authService.LoginAsync(dto);
        return Ok(response);
    }
}
