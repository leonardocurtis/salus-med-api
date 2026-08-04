using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services.Base;

public abstract class EmployeeRegistrationServiceBase<TRequest, TResponse, TEntity>
    : IRegistrationService<TRequest, TResponse>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IEmployeeNumberGenerator _employeeNumberGenerator;

    protected EmployeeRegistrationServiceBase(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        IEmployeeNumberGenerator employeeNumberGenerator
    )
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _employeeNumberGenerator = employeeNumberGenerator;
    }

    public async Task<TResponse> RegisterAsync(TRequest request, CancellationToken ct)
    {
        var employeeNumber = await _employeeNumberGenerator.GenerateAsync(ct);

        await ValidateCpfAsync(GetCpf(request));
        await ValidateEmailAsync(GetEmail(request));
        await ValidatePhoneAsync(GetPhone(request));
        await ValidateProfessionalFieldsAsync(request);

        var department = await GetDepartmentAsync(GetDepartment(request));

        var entity = CreateEntity(request, department, employeeNumber);

        await PersistAsync(entity);

        return BuildResponse(entity);
    }

    private async Task ValidatePhoneAsync(Phone phone)
    {
        if (await _employeeRepository.PhoneExistsAsync(phone))
            throw new ConflictException($"Phone {phone} already in use.");
    }

    private async Task ValidateEmailAsync(Email email)
    {
        if (await _employeeRepository.EmailExistsAsync(email))
            throw new ConflictException($"Email {email} already in use.");
    }

    private async Task ValidateCpfAsync(Cpf cpf)
    {
        if (await _employeeRepository.CpfExistsAsync(cpf))
            throw new ConflictException($"CPF {cpf} already in use.");
    }

    private async Task<Department> GetDepartmentAsync(long departmentId)
    {
        return await _departmentRepository.GetDepartmentByIdAsync(departmentId)
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
    protected abstract long GetDepartment(TRequest request);
}
