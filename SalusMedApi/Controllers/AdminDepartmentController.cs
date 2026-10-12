using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/admin/departments")]
[Authorize(Roles = RoleNames.Admin)]
public class AdminDepartmentController(IDepartmentService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterDepartmentResponse>> RegisterDepartment(
        [FromBody] RegisterDepartmentRequest dto,
        CancellationToken ct
    )
    {
        var department = await service.RegisterDepartmentAsync(dto, ct);
        return CreatedAtAction(nameof(GetDepartment), new { publicId = department.Id }, department);
    }

    [HttpDelete("{publicId:guid}")]
    public async Task<ActionResult> DeactivateDepartment(Guid publicId, CancellationToken ct)
    {
        await service.DeactivateDepartmentAsync(publicId, ct);
        return NoContent();
    }

    [HttpPatch("{publicId:guid}")]
    public async Task<ActionResult<UpdateDepartmentResponse>> UpdateDepartment(
        Guid publicId,
        UpdateDepartmentRequest request,
        CancellationToken ct
    )
    {
        var department = await service.UpdateDepartmentAsync(publicId, request, ct);
        return Ok(department);
    }

    [HttpPatch("{publicId:guid}/activate")]
    public async Task<ActionResult> ActivateDepartment(Guid publicId, CancellationToken ct)
    {
        await service.ActivateDepartmentAsync(publicId, ct);
        return NoContent();
    }

    [HttpGet("{publicId:guid}")]
    public async Task<ActionResult<AdminDepartmentDetailsResponse>> GetDepartment(
        Guid publicId,
        CancellationToken ct
    )
    {
        var department = await service.GetAdminDepartmentDetailsAsync(publicId, ct);

        return Ok(department);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<AdminDepartmentListResponse>>> ListAllDepartment(
        [FromQuery] PagedRequest request,
        CancellationToken ct
    )
    {
        var result = await service.ListAllAdminAsync(request, ct);
        return Ok(result);
    }
}
