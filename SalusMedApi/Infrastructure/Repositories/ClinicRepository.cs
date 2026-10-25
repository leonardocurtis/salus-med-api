using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Extensions;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Repositories;

public class ClinicRepository(AppDbContext context) : IClinicRepository
{
    public async Task<bool> CnpjExistsAsync(Cnpj cnpj, CancellationToken ct = default) =>
        await context.Clinics.AnyAsync(c => c.CnpjCode == cnpj, ct);

    public void Add(Clinic clinic) => context.Clinics.Add(clinic);

    public async Task<Clinic?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default) =>
        await context.Clinics.ByPublicId(publicId).FirstOrDefaultAsync(ct);

    public async Task<Clinic?> GetActiveByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) => await context.Clinics.ActiveByPublicId(publicId).FirstOrDefaultAsync(ct);
}
