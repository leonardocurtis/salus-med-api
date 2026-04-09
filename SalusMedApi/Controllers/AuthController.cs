using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Services.Interfaces;
using LoginRequest = SalusMedApi.Application.DTOs.Auth.LoginRequest;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RegisterPhysicianResponse>> RegisterPhysician(
        [FromBody] RegisterPhysicianRequest dto
    )
    {
        var physician = await _authService.RegisterPhysicianAsync(dto);
        return StatusCode(StatusCodes.Status201Created, physician);
    }

    [HttpPost("register/patient")]
    [AllowAnonymous]
    public async Task<ActionResult<RegisterPatientResponse>> RegisterPatient(
        [FromBody] RegisterPatientRequest dto
    )
    {
        var patient = await _authService.RegisterPatientAsync(dto);
        return StatusCode(StatusCodes.Status201Created, patient);
    }
}
