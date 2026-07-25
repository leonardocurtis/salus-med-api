using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;
using SalusMedApi.Application.Services.Base;
using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services;

public sealed class PhysicianService(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IPhysicianRepository physicianRepository,
    IEmployeeNumberGenerator employeeNumberGenerator,
    IUnitOfWork unitOfWork
)
    : EmployeeRegistrationServiceBase<
        RegisterPhysicianRequest,
        RegisterPhysicianResponse,
        Physician
    >(employeeRepository, departmentRepository, employeeNumberGenerator)
{
    protected override async Task ValidateProfessionalFieldsAsync(RegisterPhysicianRequest request)
    {
        var crm = Crm.Create(request.Crm);

        if (await physicianRepository.MedicalRegistrationExistsAsync(crm))
            throw new ConflictException(
                $"CRM {request.Crm} is already associated with an account."
            );
    }

    protected override Physician CreateEntity(
        RegisterPhysicianRequest request,
        Department department,
        string employeeNumber
    ) => request.ToEntity(department, employeeNumber, Occupation.Physician);

    protected override async Task PersistAsync(Physician physician)
    {
        physicianRepository.Add(physician);
        await unitOfWork.CommitAsync();
    }

    protected override RegisterPhysicianResponse BuildResponse(Physician physician) =>
        physician.ToResponse();

    protected override Cpf GetCpf(RegisterPhysicianRequest request) => Cpf.Create(request.Cpf);

    protected override Phone GetPhone(RegisterPhysicianRequest request) =>
        Phone.Create(request.Phone);

    protected override Email GetEmail(RegisterPhysicianRequest request) =>
        Email.Create(request.Email);

    protected override long GetDepartment(RegisterPhysicianRequest request) => request.DepartmentId;
}
