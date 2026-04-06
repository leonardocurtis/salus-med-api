using Microsoft.EntityFrameworkCore;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Infrastructure.Persistence;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CpfExistsAsync(string cpf) =>
        await _context.Patients.AnyAsync(x => x.Cpf == cpf);

    public async Task<Patient> SaveAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }
}
