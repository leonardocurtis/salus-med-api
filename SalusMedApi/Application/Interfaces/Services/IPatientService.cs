using SalusMedApi.Application.DTOs.Patient;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IPatientService
{
    Task<RegisterPatientResponse> RegisterPatientAsync(RegisterPatientRequest patientRequest);
}
