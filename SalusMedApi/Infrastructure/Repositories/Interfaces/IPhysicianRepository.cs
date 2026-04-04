using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Infrastructure.Repositories.Interfaces;

public interface IPhysicianRepository
{
    Task<Physician> SaveAsync(Physician physician);
}
