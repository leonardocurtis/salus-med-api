namespace SalusMedApi.Application.Interfaces.Auth;

public interface ICurrentUserService
{
    string? EmployeeIdNumber { get; }
    string? UserId { get; }
    bool IsInRole(string role);
}
