using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Application.DTOs.Physician;

namespace SalusMedApi.Application.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterPhysicianResponse> RegisterPhysicianAsync(
        RegisterPhysicianRequest physicianRequest
    );
    Task<RegisterPatientResponse> RegisterPatientAsync(RegisterPatientRequest patientRequest);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
}
