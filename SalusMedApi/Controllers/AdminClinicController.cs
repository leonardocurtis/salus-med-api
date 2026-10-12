using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/admin/clinics")]
[Authorize(Roles = RoleNames.Admin)]
public class AdminClinicController(IClinicService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<AdminClinicListResponse>>> ListAllClinics(
        [FromQuery] PagedRequest request,
        CancellationToken ct
    )
    {
        var result = await service.ListAllAdminAsync(request, ct);
        return Ok(result);
    }

    [HttpGet("{publicId:guid}")]
    public async Task<ActionResult<AdminClinicDetailsResponse>> GetClinic(
        Guid publicId,
        CancellationToken ct
    )
    {
        var clinic = await service.GetAdminClinicDetailsAsync(publicId, ct);
        return Ok(clinic);
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterClinicResponse>> RegisterClinic(
        [FromBody] RegisterClinicRequest dto,
        CancellationToken ct
    )
    {
        var clinic = await service.RegisterClinicAsync(dto, ct);
        return CreatedAtAction(nameof(GetClinic), new { publicId = clinic.Id }, clinic);
    }

    [HttpPatch("{publicId:guid}")]
    public async Task<ActionResult<UpdateClinicResponse>> UpdateClinic(
        Guid publicId,
        [FromBody] UpdateClinicRequest request,
        CancellationToken ct
    )
    {
        var clinic = await service.UpdateClinicAsync(publicId, request, ct);
        return Ok(clinic);
    }

    [HttpDelete("{publicId:guid}")]
    public async Task<ActionResult> DeactivateClinic(Guid publicId, CancellationToken ct)
    {
        await service.DeactivateClinicAsync(publicId, ct);
        return NoContent();
    }

    [HttpPatch("{publicId:guid}/suspend")]
    public async Task<ActionResult> SuspendClinic(Guid publicId, CancellationToken ct)
    {
        await service.SuspendClinicAsync(publicId, ct);
        return NoContent();
    }

    [HttpPatch("{publicId:guid}/activate")]
    public async Task<ActionResult> ActivateClinic(Guid publicId, CancellationToken ct)
    {
        await service.ActivateClinicAsync(publicId, ct);
        return NoContent();
    }
}
