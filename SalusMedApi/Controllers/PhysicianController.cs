using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Interfaces.Services;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/physicians")]
public class PhysicianController(
    IRegistrationService<RegisterPhysicianRequest, RegisterPhysicianResponse> physicianService
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterPhysicianResponse>> RegisterPhysician(
        [FromBody] RegisterPhysicianRequest dto,
        CancellationToken ct
    )
    {
        var physician = await physicianService.RegisterAsync(dto, ct);
        return StatusCode(StatusCodes.Status201Created, physician);
    }
}
