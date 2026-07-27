using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IClinicRepository
{
    Task<bool> CnpjExistsAsync(Cnpj cnpj);
    void Add(Clinic clinic);
}