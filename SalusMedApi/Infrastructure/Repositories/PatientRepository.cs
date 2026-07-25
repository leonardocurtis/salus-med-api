using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class PatientRepository(AppDbContext context) : IPatientRepository
{
    public async Task<bool> CpfExistsAsync(Cpf cpf) =>
        await context.Patients.AnyAsync(x => x.CpfCode == cpf);

    public void Add(Patient patient) => context.Patients.Add(patient);
}
