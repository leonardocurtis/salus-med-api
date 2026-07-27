using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class ClinicRepository(AppDbContext context) : IClinicRepository
{
    public async Task<bool> CnpjExistsAsync(Cnpj cnpj) => await context.Clinics.AnyAsync(c => c.CnpjCode == cnpj);

    public void Add(Clinic clinic) => context.Clinics.Add(clinic);
}