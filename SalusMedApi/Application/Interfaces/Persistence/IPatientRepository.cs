using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IPatientRepository
{
    Task<bool> CpfExistsAsync(string cpf);
    Task<Patient> SaveAsync(Patient patient);
}
