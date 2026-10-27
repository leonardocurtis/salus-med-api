using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Employee;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Constants;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/employees")]
[Authorize(Roles = RoleNames.Admin)]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost("{publicId:guid}/credentials")]
    public async Task<ActionResult> CreateCredentials(
        Guid publicId,
        [FromBody] CreateEmployeeCredentialsRequest dto,
        CancellationToken ct
    )
    {
        await employeeService.CreateCredentialsAsync(publicId, dto, ct);

        return StatusCode(StatusCodes.Status201Created);
    }
}
