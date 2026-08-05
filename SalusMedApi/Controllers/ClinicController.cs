using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/clinics")]
public class ClinicController(IClinicService clinicService) : ControllerBase
{
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterClinicResponse>> RegisterClinic(
        [FromBody] RegisterClinicRequest dto,
        CancellationToken ct
    )
    {
        var clinic = await clinicService.RegisterClinicAsync(dto, ct);
        return CreatedAtAction(nameof(GetClinic), new { publicId = clinic.Id }, clinic);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ClinicListResponse>>> ListAllClinics(
        [FromQuery] PagedRequest request,
        CancellationToken ct
    )
    {
        var result = await clinicService.ListAllActiveAsync(request, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{publicId:guid}")]
    public async Task<ActionResult<ClinicDetailsResponse>> GetClinic(
        Guid publicId,
        CancellationToken ct
    )
    {
        var clinic = await clinicService.GetClinicDetailsAsync(publicId, ct);
        return Ok(clinic);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{publicId:guid}")]
    public async Task<ActionResult> DeactivateClinic(Guid publicId, CancellationToken ct)
    {
        await clinicService.DeactivateClinicAsync(publicId, ct);
        return NoContent();
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("{publicId:guid}")]
    public async Task<ActionResult<UpdateClinicResponse>> UpdateClinic(
        Guid publicId,
        UpdateClinicRequest request,
        CancellationToken ct
    )
    {
        var clinic = await clinicService.UpdateClinicAsync(publicId, request, ct);
        return Ok(clinic);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("{publicId:guid}/suspend")]
    public async Task<ActionResult> SuspendClinicAsync(Guid publicId, CancellationToken ct)
    {
        await clinicService.SuspendClinicAsync(publicId, ct);
        return NoContent();
    }

    [HttpPatch("{publicId:guid}/activate")]
    public async Task<ActionResult> ActivateClinicAsync(Guid publicId, CancellationToken ct)
    {
        await clinicService.ActivateClinicAsync(publicId, ct);
        return NoContent();
    }
}
