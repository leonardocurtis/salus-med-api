using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Services.Interfaces;
using LoginRequest = SalusMedApi.Application.DTOs.Auth.LoginRequest;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> SignIn([FromBody] LoginRequest dto)
    {
        var response = await _authService.LoginAsync(dto);
        return Ok(response);
    }

    [HttpPost("register/physician")]
    [AllowAnonymous]
    public async Task<ActionResult<RegisterPhysicianResponse>> RegisterPhysician(
        [FromBody] RegisterPhysicianRequest dto
    )
    {
        var physician = await _authService.RegisterPhysicianAsync(dto);
        return StatusCode(StatusCodes.Status201Created, physician);
    }
}
