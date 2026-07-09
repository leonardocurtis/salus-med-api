using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Application.Mapper;
using SalusMedApi.Application.Services.Interfaces;
using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPhysicianRepository _physicianRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public AuthService(
        ITokenService tokenService,
        IUserRepository userRepository,
        IPhysicianRepository physicianRepository,
        IPatientRepository patientRepository,
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository
    )
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _physicianRepository = physicianRepository;
        _patientRepository = patientRepository;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<RegisterPhysicianResponse> RegisterPhysicianAsync(
        RegisterPhysicianRequest physicianRequest
    )
    {
        var email = Email.Create(physicianRequest.Credentials.Email);

        if (await _userRepository.EmailExistAsync(email))
            throw new ConflictException(
                $"Email {physicianRequest.Credentials.Email} already in use."
            );

        if (await _employeeRepository.CpfExistsAsync(physicianRequest.Physician.Cpf))
            throw new ConflictException($"Cpf {physicianRequest.Physician.Cpf} already in use.");

        if (
            await _physicianRepository.MedicalRegistrationExistsAsync(
                physicianRequest.Physician.MedicalRegistration
            )
        )
            throw new ConflictException(
                $"Medical registration number {physicianRequest.Physician.MedicalRegistration} is already associated with an account."
            );

        var department =
            await _departmentRepository.GetDepartmentByIdAsync(
                physicianRequest.Physician.DepartmentId
            ) ?? throw new ResourceNotFoundException("Department not found.");

        var physician = PhysicianMapper.ToEntity(physicianRequest, department);

        await _physicianRepository.SaveAsync(physician);

        return new RegisterPhysicianResponse(
            physician.Id,
            physician.Employee.Name,
            physician.Employee.PhoneNumber.Formatted(),
            physician.MedicalRegistration.Formatted(),
            physician.Employee.CpfNumber.Formatted(),
            physician.Employee.Gender,
            physician.Employee.DateOfBirth,
            physician.Specialty,
            new AddressResponse(
                physician.Employee.Address.Street,
                physician.Employee.Address.Number,
                physician.Employee.Address.Complement,
                physician.Employee.Address.Neighborhood,
                physician.Employee.Address.PostalCode.Formatted(),
                physician.Employee.Address.City,
                physician.Employee.Address.State
            ),
            physician.Employee.Status,
            physician.CreatedAt
        );
    }

    public async Task<RegisterPatientResponse> RegisterPatientAsync(
        RegisterPatientRequest patientRequest
    )
    {
        var email = Email.Create(patientRequest.Credentials.Email);

        if (await _userRepository.EmailExistAsync(email))
            throw new ConflictException(
                $"Email {patientRequest.Credentials.Email} already in use."
            );

        if (await _patientRepository.CpfExistsAsync(patientRequest.Patient.Cpf))
            throw new ConflictException($"Cpf {patientRequest.Patient.Cpf} already in use.");

        var patient = PatientMapper.ToEntity(patientRequest);

        await _patientRepository.SaveAsync(patient);

        return new RegisterPatientResponse(
            patient.Id,
            patient.Name,
            patient.MotherName,
            patient.PhoneNumber.Formatted(),
            patient.CpfCode.Formatted(),
            patient.Gender,
            patient.DateOfBirth,
            new AddressResponse(
                patient.Address.Street,
                patient.Address.Number,
                patient.Address.Complement,
                patient.Address.Neighborhood,
                patient.Address.PostalCode.Formatted(),
                patient.Address.City,
                patient.Address.State
            ),
            patient.Status,
            patient.CreatedAt
        );
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var email = Email.Create(loginRequest.Email);

        var user =
            await _userRepository.GetUserByEmailAsync(email)
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (user.Status == AccountStatus.Deactivated)
            throw new ForbiddenException("Account is inactive.");

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse(
            user.Id,
            user.EmailAddress.ToString(),
            token.UserToken,
            token.ExpiresAt
        );
    }
}
