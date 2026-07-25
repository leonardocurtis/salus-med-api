using SalusMedApi.Application.DTOs.Employee;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Security;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Constants;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Services;

public sealed class EmployeeService(
    IEmployeeRepository employeeRepository,
    IPasswordHasher passwordHasher,
    IRoleRepository roleRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : IEmployeeService
{
    public async Task CreateCredentialsAsync(
        string employeeId,
        CreateEmployeeCredentialsRequest request
    )
    {
        var employee =
            await employeeRepository.GetEmployeeByEmployeeNumberAsync(employeeId)
            ?? throw new ResourceNotFoundException("Employee not found");

        if (employee.User is not null)
            throw new DomainException("Employee already has credentials.");

        var passwordHash = passwordHasher.Hash(request.Password);

        var role =
            await roleRepository.GetByNameAsync(RoleNames.Staff)
            ?? throw new ResourceNotFoundException("Role not found");

        var user = User.Create(employee.EmployeeNumber, passwordHash);

        user.AssignRole(role);
        employee.AssignCredentials(user);

        userRepository.Add(user);
        await unitOfWork.CommitAsync();
    }
}
