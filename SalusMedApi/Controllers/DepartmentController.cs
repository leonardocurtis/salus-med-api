using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
