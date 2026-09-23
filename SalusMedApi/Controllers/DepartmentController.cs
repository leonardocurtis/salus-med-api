using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/department")]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost("register")]
    public async Task<ActionResult<RegisterDepartmentResponse>> RegisterDepartment(
        [FromBody] RegisterDepartmentRequest dto,
        CancellationToken ct
    )
    {
        var department = await departmentService.RegisterDepartmentAsync(dto, ct);
        return CreatedAtAction(nameof(GetDepartment), new { publicId = department.Id }, department);
    }

    [Authorize]
    [HttpGet("{publicId:guid}")]
    public async Task<ActionResult<DepartmentDetailsResponse>> GetDepartment(
        Guid publicId,
        CancellationToken ct
    )
    {
        var department = await departmentService.GetDepartmentDetailsAsync(publicId, ct);
        return Ok(department);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<DepartmentListResponse>>> ListAllDepartments(
        [FromQuery] PagedRequest request,
        CancellationToken ct
    )
    {
        var result = await departmentService.ListAllActiveAsync(request, ct);
        return Ok(result);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{publicId:guid}")]
    public async Task<ActionResult> DeactivateClinic(Guid publicId, CancellationToken ct)
    {
        await departmentService.DeactivateDepartmentAsync(publicId, ct);
        return NoContent();
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("{publicId:guid}")]
    public async Task<ActionResult<UpdateDepartmentResponse>> UpdateDepartment(
        Guid publicId,
        UpdateDepartmentRequest request,
        CancellationToken ct
    )
    {
        var department = await departmentService.UpdateDepartmentAsync(publicId, request, ct);
        return Ok(department);
    }

    [HttpPatch("{publicId:guid}/activate")]
    public async Task<ActionResult> ActivateClinicAsync(Guid publicId, CancellationToken ct)
    {
        await departmentService.ActivateDepartmentAsync(publicId, ct);
        return NoContent();
    }
}
