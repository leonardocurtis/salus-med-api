using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Department;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Infrastructure.Extensions;
using SalusMedApi.Infrastructure.Persistence;

namespace SalusMedApi.Infrastructure.Queries;

public class DepartmentQueries(AppDbContext context) : IDepartmentQueries
{
    public async Task<DepartmentDetailsResponse?> GetActiveDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await context
            .Departments.AsNoTracking()
            .ActiveByPublicId(publicId)
            .Select(d => new DepartmentDetailsResponse(d.PublicId, d.Name))
            .FirstOrDefaultAsync(ct);

    public async Task<AdminDepartmentDetailsResponse?> GetAdminDetailsByPublicIdAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await context
            .Departments.AsNoTracking()
            .ByPublicId(publicId)
            .Select(department => new AdminDepartmentDetailsResponse(
                department.PublicId,
                department.Name,
                department.Status,
                department.CreatedAt,
                department.UpdatedAt,
                department.DeletedAt,
                department.CreatedBy,
                department.UpdatedBy,
                department.DeletedBy
            ))
            .FirstOrDefaultAsync(ct);

    public async Task<PagedResponse<DepartmentListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) =>
        await context
            .Departments.AsNoTracking()
            .Active()
            .Select(d => new DepartmentListResponse(d.PublicId, d.Name, d.Status))
            .ToPagedResponseAsync(request, d => d.Name, ct);

    public async Task<PagedResponse<AdminDepartmentListResponse>> ListAllByAnyStatusAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) =>
        await context
            .Departments.AsNoTracking()
            .Select(d => new AdminDepartmentListResponse(d.PublicId, d.Name, d.Status, d.CreatedAt))
            .ToPagedResponseAsync(request, d => d.Name, ct);
}
