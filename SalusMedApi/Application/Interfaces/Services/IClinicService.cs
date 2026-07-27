using SalusMedApi.Application.DTOs.Clinic;

namespace SalusMedApi.Application.Interfaces.Services;

public interface IClinicService
{
    Task<RegisterClinicResponse> RegisterClinicAsync(RegisterClinicRequest request);
}