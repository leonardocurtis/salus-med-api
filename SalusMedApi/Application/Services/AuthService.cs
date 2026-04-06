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

    public AuthService(
        ITokenService tokenService,
        IUserRepository userRepository,
        IPhysicianRepository physicianRepository,
        IPatientRepository patientRepository
    )
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _physicianRepository = physicianRepository;
        _patientRepository = patientRepository;
    }

    public async Task<RegisterPhysicianResponse> RegisterPhysicianAsync(
        RegisterPhysicianRequest physicianRequest
    )
    {
        if (await _userRepository.EmailExistAsync(physicianRequest.Credentials.Email))
        {
            throw new ConflictException(
                $"Email {physicianRequest.Credentials.Email} already in use."
            );
        }

        var physician = new Physician
        {
            Name = physicianRequest.Physician.Name,
            Phone = physicianRequest.Physician.Phone,
            MedicalRegistration = physicianRequest.Physician.MedicalRegistration,
            Cpf = physicianRequest.Physician.Cpf,
            Gender = physicianRequest.Physician.Gender,
            DateOfBirth = physicianRequest.Physician.DateOfBirth,
            Specialty = physicianRequest.Physician.Specialty,
            Address = new Address
            {
                Street = physicianRequest.Physician.Address.Street,
                Number = physicianRequest.Physician.Address.Number,
                Neighborhood = physicianRequest.Physician.Address.Neighborhood,
                PostalCode = physicianRequest.Physician.Address.PostalCode,
                Complement = physicianRequest.Physician.Address.Complement,
                City = physicianRequest.Physician.Address.City,
                State = physicianRequest.Physician.Address.State,
            },
            Status = PhysicianStatus.Active,
            User = new User
            {
                Email = physicianRequest.Credentials.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    physicianRequest.Credentials.Password
                ),
                Role = Role.Physician,
                Status = AccountStatus.Active,
            },
        };

        await _physicianRepository.SaveAsync(physician);

        return new RegisterPhysicianResponse(
            physician.Id,
            physician.Name,
            physician.Phone,
            physician.MedicalRegistration,
            physician.Cpf,
            physician.Gender,
            physician.DateOfBirth,
            physician.Specialty,
            new AddressResponse(
                physician.Address.Street,
                physician.Address.Number,
                physician.Address.Complement,
                physician.Address.Neighborhood,
                physician.Address.PostalCode,
                physician.Address.City,
                physician.Address.State
            ),
            physician.Status,
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
            Name = patientRequest.Patient.Name,
            Phone = patientRequest.Patient.Phone,
            Cpf = patientRequest.Patient.Cpf,
            Gender = patientRequest.Patient.Gender!.Value,
            DateOfBirth = patientRequest.Patient.DateOfBirth!.Value,
            Address = new Address
            {
                Street = patientRequest.Patient.Address.Street,
                Number = patientRequest.Patient.Address.Number,
                Neighborhood = patientRequest.Patient.Address.Neighborhood,
                PostalCode = patientRequest.Patient.Address.PostalCode,
                Complement = patientRequest.Patient.Address.Complement,
                City = patientRequest.Patient.Address.City,
                State = patientRequest.Patient.Address.State,
            },
            Status = PatientStatus.Active,
            User = new User
            {
                Email = patientRequest.Credentials.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(patientRequest.Credentials.Password),
                Role = Role.Patient,
                Status = AccountStatus.Active,
            },
        };

        await _patientRepository.SaveAsync(patient);

        return new RegisterPatientResponse(
            patient.Id,
            patient.Name,
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
