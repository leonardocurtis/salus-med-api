using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/clinics")]
[Authorize]
public class ClinicController(IClinicService clinicService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ClinicListResponse>>> ListAllClinics(
        [FromQuery] PagedRequest request,
        CancellationToken ct
    )
    {
        var result = await clinicService.ListAllActiveAsync(request, ct);
        return Ok(result);
    }

    [HttpGet("{publicId:guid}")]
    public async Task<ActionResult<ClinicDetailsResponse>> GetClinic(
        Guid publicId,
        CancellationToken ct
    )
    {
        var clinic = await clinicService.GetClinicDetailsAsync(publicId, ct);
        return Ok(clinic);
    }
}
