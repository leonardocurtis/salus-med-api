using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IPatientRepository
{
    Task<bool> CpfExistsAsync(string cpf);
    Task<Patient> SaveAsync(Patient patient);
}
