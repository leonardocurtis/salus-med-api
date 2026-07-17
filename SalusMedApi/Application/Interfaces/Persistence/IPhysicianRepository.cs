using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IPhysicianRepository
{
    Task<Physician> SaveAsync(Physician physician);
    Task<bool> MedicalRegistrationExistsAsync(string medicalRegistration);
}
