using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class PhysicianRepository(AppDbContext context) : IPhysicianRepository
{
    public void Add(Physician physician) => context.Physicians.Add(physician);

    public async Task<bool> MedicalRegistrationExistsAsync(Crm crm) =>
        await context.Physicians.AnyAsync(p => p.MedicalRegistration == crm);
}
