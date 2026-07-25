namespace SalusMedApi.Application.DTOs.Employee;

public record CreateEmployeeCredentialsRequest(string Password, string ConfirmPassword);
