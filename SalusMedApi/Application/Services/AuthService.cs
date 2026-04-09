using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Application.DTOs.Physician;
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

    public AuthService(
        ITokenService tokenService,
        IUserRepository userRepository,
        IPhysicianRepository physicianRepository,
        IPatientRepository patientRepository,
        IEmployeeRepository employeeRepository
    )
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _physicianRepository = physicianRepository;
        _patientRepository = patientRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<RegisterPhysicianResponse> RegisterPhysicianAsync(
        RegisterPhysicianRequest physicianRequest
    )
    {
        if (await _userRepository.EmailExistAsync(physicianRequest.Credentials.Email))
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

        var physician = new Physician
        {
            MedicalRegistration = physicianRequest.Physician.MedicalRegistration.Trim(),
            Specialty = physicianRequest.Physician.Specialty!.Value,
            Employee = new Employee
            {
                Name = physicianRequest.Physician.Name.Trim(),
                Phone = physicianRequest.Physician.Phone,
                Cpf = physicianRequest.Physician.Cpf.Trim(),
                Gender = physicianRequest.Physician.Gender!.Value,
                DateOfBirth = physicianRequest.Physician.DateOfBirth!.Value,
                Status = EmployeeStatus.Active,
                Address = new Address
                {
                    Street = physicianRequest.Physician.Address.Street.Trim(),
                    Number = physicianRequest.Physician.Address.Number.Trim(),
                    Neighborhood = physicianRequest.Physician.Address.Neighborhood.Trim(),
                    PostalCode = physicianRequest.Physician.Address.PostalCode.Trim(),
                    Complement = string.IsNullOrWhiteSpace(
                        physicianRequest.Physician.Address.Complement
                    )
                        ? null
                        : physicianRequest.Physician.Address.Complement.Trim(),
                    City = physicianRequest.Physician.Address.City.Trim(),
                    State = physicianRequest.Physician.Address.State.Trim(),
                },
                User = new User
                {
                    Email = physicianRequest.Credentials.Email.ToLower().Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                        physicianRequest.Credentials.Password
                    ),
                    Role = Role.Physician,
                    Status = AccountStatus.Active,
                },
            },
        };

        await _physicianRepository.SaveAsync(physician);

        return new RegisterPhysicianResponse(
            physician.Id,
            physician.Employee.Name,
            physician.Employee.Phone,
            physician.MedicalRegistration,
            physician.Employee.Cpf,
            physician.Employee.Gender,
            physician.Employee.DateOfBirth,
            physician.Specialty,
            new AddressResponse(
                physician.Employee.Address.Street,
                physician.Employee.Address.Number,
                physician.Employee.Address.Complement,
                physician.Employee.Address.Neighborhood,
                physician.Employee.Address.PostalCode,
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
        if (await _userRepository.EmailExistAsync(patientRequest.Credentials.Email))
            throw new ConflictException(
                $"Email {patientRequest.Credentials.Email} already in use."
            );

        if (await _patientRepository.CpfExistsAsync(patientRequest.Patient.Cpf))
            throw new ConflictException($"Cpf {patientRequest.Patient.Cpf} already in use.");

        var patient = new Patient
        {
            Name = patientRequest.Patient.Name.Trim(),
            MotherName = patientRequest.Patient.MotherName.Trim(),
            FatherName = string.IsNullOrWhiteSpace(patientRequest.Patient.FatherName)
                ? null
                : patientRequest.Patient.FatherName.Trim(),
            Phone = patientRequest.Patient.Phone.Trim(),
            Cpf = patientRequest.Patient.Cpf.Trim(),
            Gender = patientRequest.Patient.Gender!.Value,
            DateOfBirth = patientRequest.Patient.DateOfBirth!.Value,
            Address = new Address
            {
                Street = patientRequest.Patient.Address.Street.Trim(),
                Number = patientRequest.Patient.Address.Number.Trim(),
                Neighborhood = patientRequest.Patient.Address.Neighborhood.Trim(),
                PostalCode = patientRequest.Patient.Address.PostalCode.Trim(),
                Complement = string.IsNullOrWhiteSpace(patientRequest.Patient.Address.Complement)
                    ? null
                    : patientRequest.Patient.Address.Complement.Trim(),
                City = patientRequest.Patient.Address.City.Trim(),
                State = patientRequest.Patient.Address.State.Trim(),
            },
            Status = PatientStatus.Active,
            User = new User
            {
                Email = patientRequest.Credentials.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(patientRequest.Credentials.Password),
                Role = Role.Patient,
                Status = AccountStatus.Active,
            },
        };

        await _patientRepository.SaveAsync(patient);

        return new RegisterPatientResponse(
            patient.Id,
            patient.Name,
            patient.MotherName,
            patient.Phone,
            patient.Cpf,
            patient.Gender,
            patient.DateOfBirth,
            new AddressResponse(
                patient.Address.Street,
                patient.Address.Number,
                patient.Address.Complement,
                patient.Address.Neighborhood,
                patient.Address.PostalCode,
                patient.Address.City,
                patient.Address.State
            ),
            patient.Status,
            patient.CreatedAt
        );
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user =
            await _userRepository.GetUserByEmailAsync(loginRequest.Email.ToLower())
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (user.Status == AccountStatus.Deactivated)
            throw new ForbiddenException("Account is inactive.");

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid credentials.");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse(user.Id, user.Email, token.UserToken, token.ExpiresAt);
    }
}
