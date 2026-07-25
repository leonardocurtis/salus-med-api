using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Application.Interfaces.Services;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/patients")]
public class PatientController(IPatientService patientService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterPatientResponse>> RegisterPatient(
        [FromBody] RegisterPatientRequest dto
    )
    {
        var patient = await patientService.RegisterPatientAsync(dto);
        return StatusCode(StatusCodes.Status201Created, patient);
    }
}
