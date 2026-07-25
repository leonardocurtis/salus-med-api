using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IPhysicianRepository
{
    void Add(Physician physician);
    Task<bool> MedicalRegistrationExistsAsync(Crm medicalRegistration);
}
