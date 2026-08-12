using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services.Base;

public abstract class EmployeeRegistrationServiceBase<TRequest, TResponse, TEntity>(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IEmployeeNumberGenerator employeeNumberGenerator
) : IRegistrationService<TRequest, TResponse>
{
    public async Task<TResponse> RegisterAsync(TRequest request, CancellationToken ct = default)
    {
        var employeeNumber = await employeeNumberGenerator.GenerateAsync(ct);

        await ValidateCpfAsync(GetCpf(request), ct);
        await ValidateEmailAsync(GetEmail(request), ct);
        await ValidatePhoneAsync(GetPhone(request), ct);
        await ValidateProfessionalFieldsAsync(request);

        var department = await GetDepartmentAsync(GetDepartment(request), ct);

        var entity = CreateEntity(request, department, employeeNumber);

        await PersistAsync(entity);

        return BuildResponse(entity);
    }

    private async Task ValidatePhoneAsync(Phone phone, CancellationToken ct = default)
    {
        if (await employeeRepository.PhoneExistsAsync(phone, ct))
            throw new ConflictException($"Phone {phone} already in use.");
    }

    private async Task ValidateEmailAsync(Email email, CancellationToken ct = default)
    {
        if (await employeeRepository.EmailExistsAsync(email, ct))
            throw new ConflictException($"Email {email} already in use.");
    }

    private async Task ValidateCpfAsync(Cpf cpf, CancellationToken ct = default)
    {
        if (await employeeRepository.CpfExistsAsync(cpf, ct))
            throw new ConflictException($"CPF {cpf} already in use.");
    }

    private async Task<Department> GetDepartmentAsync(
        Guid departmentPublicId,
        CancellationToken ct = default
    )
    {
        return await departmentRepository.GetByPublicIdAsync(departmentPublicId, ct)
            ?? throw new ResourceNotFoundException("Department not found.");
    }

    protected virtual Task ValidateProfessionalFieldsAsync(TRequest request) => Task.CompletedTask;

    protected abstract TEntity CreateEntity(
        TRequest request,
        Department department,
        string employeeNumber
    );
    protected abstract Task PersistAsync(TEntity entity);
    protected abstract TResponse BuildResponse(TEntity entity);
    protected abstract Cpf GetCpf(TRequest request);
    protected abstract Phone GetPhone(TRequest request);
    protected abstract Email GetEmail(TRequest request);
    protected abstract Guid GetDepartment(TRequest request);
}
