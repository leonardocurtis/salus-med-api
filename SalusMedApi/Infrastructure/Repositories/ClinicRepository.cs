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

    public Task<PagedResponse<Clinic>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) =>
        context
            .Clinics.Where(c => c.Status != ClinicStatus.Deactivated)
            .ToPagedResponseAsync(request, c => c.CorporateName, ct);

    public Task<PagedResponse<Clinic>> ListAllAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) => context.Clinics.ToPagedResponseAsync(request, c => c.CorporateName, ct);

    public async Task<Clinic?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default) =>
        await context.Clinics.FirstOrDefaultAsync(c => c.PublicId == publicId, ct);

    public async Task<Clinic?> GetActiveByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await context.Clinics.FirstOrDefaultAsync(
            c => c.PublicId == publicId && c.Status == ClinicStatus.Active,
            ct
        );
}
