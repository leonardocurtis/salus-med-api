using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/department")]
[Authorize]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
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
}
