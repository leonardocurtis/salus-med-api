using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/admin/clinics")]
[Authorize(Roles = RoleNames.Admin)]
public class AdminClinicController(ClinicService service) : ControllerBase
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
}
