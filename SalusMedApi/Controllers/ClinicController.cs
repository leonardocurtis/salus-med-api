using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Interfaces.Services;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/clinics")]
public class ClinicController(IClinicService clinicService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterClinicResponse>> RegisterClinic(
        [FromBody] RegisterClinicRequest dto
    )
    {
        var clinic = await clinicService.RegisterClinicAsync(dto);
        
        return StatusCode(StatusCodes.Status201Created, clinic);
    }
}