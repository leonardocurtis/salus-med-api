using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IPatientRepository
{
    Task<bool> CpfExistsAsync(Cpf cpf);
    void Add(Patient patient);
}
