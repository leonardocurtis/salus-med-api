using Microsoft.AspNetCore.Mvc;
using SalusMedApi.Application.DTOs.Employee;
using SalusMedApi.Application.Interfaces.Services;

namespace SalusMedApi.Controllers;

[ApiController]
[Route("api/v1/employees")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost("{employeeId}/credentials")]
    public async Task<ActionResult> CreateCredentials(
        string employeeId,
        [FromBody] CreateEmployeeCredentialsRequest dto
    )
    {
        await employeeService.CreateCredentialsAsync(employeeId, dto);

        return StatusCode(StatusCodes.Status201Created);
    }
}
