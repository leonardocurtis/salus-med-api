using Microsoft.EntityFrameworkCore;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Infrastructure.Repositories;

public class PhysicianRepository : IPhysicianRepository
{
    private readonly AppDbContext _context;

    public PhysicianRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Physician> SaveAsync(Physician physician)
    {
        _context.Physicians.Add(physician);
        await _context.SaveChangesAsync();
        return physician;
    }

    public async Task<bool> MedicalRegistrationExistsAsync(string medicalRegistration) =>
        await _context.Physicians.AnyAsync(p => p.MedicalRegistration == medicalRegistration);
}
