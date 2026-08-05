using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
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
        ListAsync(
            context.Clinics.Where(c => c.Status != ClinicStatus.Deactivated).AsQueryable(),
            request,
            ct
        );

    public Task<PagedResponse<Clinic>> ListAllAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) => ListAsync(context.Clinics, request, ct);

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

    private static async Task<PagedResponse<Clinic>> ListAsync(
        IQueryable<Clinic> query,
        PagedRequest request,
        CancellationToken ct
    )
    {
        query = request.SortDir.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
            ? query.OrderByDescending(c => c.CorporateName)
            : query.OrderBy(c => c.CorporateName);

        var totalElements = await query.CountAsync(ct);

        var clinics = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(ct);

        return new PagedResponse<Clinic>
        {
            Content = clinics,
            Page = request.Page,
            Size = request.Size,
            TotalElements = totalElements,
            TotalPages = (int)Math.Ceiling(totalElements / (double)request.Size),
            First = request.Page == 1,
            Last = request.Page >= (int)Math.Ceiling(totalElements / (double)request.Size),
        };
    }
}
