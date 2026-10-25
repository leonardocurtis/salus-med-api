using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Infrastructure.Extensions;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Queries;

public class ClinicQueries(AppDbContext context) : IClinicQueries
{
    public async Task<PagedResponse<AdminClinicListResponse>> ListAllByAnyStatusAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) =>
        await context
            .Clinics.AsNoTracking()
            .ToPagedResponseAsync(
                request,
                c => c.CorporateName,
                c => c.Id,
                c => new AdminClinicListResponse(
                    c.PublicId,
                    c.CorporateName,
                    c.TradeName,
                    c.CnpjCode.Value,
                    c.Status,
                    c.CreatedAt
                ),
                ct
            );

    public async Task<PagedResponse<ClinicListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) =>
        await context
            .Clinics.AsNoTracking()
            .Active()
            .ToPagedResponseAsync(
                request,
                c => c.CorporateName,
                c => c.Id,
                c => new ClinicListResponse(
                    c.PublicId,
                    c.CorporateName,
                    c.TradeName,
                    c.CnpjCode.Value,
                    c.Status
                ),
                ct
            );

    public async Task<ClinicDetailsResponse?> GetActiveDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await context
            .Clinics.AsNoTracking()
            .ActiveByPublicId(publicId)
            .Select(c => new ClinicDetailsResponse(
                c.CorporateName,
                c.TradeName,
                c.CnpjCode.Value,
                c.Status
            ))
            .FirstOrDefaultAsync(ct);

    public async Task<AdminClinicDetailsResponse?> GetAdminDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await context
            .Clinics.AsNoTracking()
            .ByPublicId(publicId)
            .Select(clinic => new AdminClinicDetailsResponse(
                clinic.PublicId,
                clinic.CorporateName,
                clinic.TradeName,
                clinic.CnpjCode.Value,
                clinic.Status,
                clinic.CreatedAt,
                clinic.UpdatedAt,
                clinic.DeletedAt,
                clinic.CreatedBy,
                clinic.UpdatedBy,
                clinic.DeletedBy
            ))
            .FirstOrDefaultAsync(ct);
}
