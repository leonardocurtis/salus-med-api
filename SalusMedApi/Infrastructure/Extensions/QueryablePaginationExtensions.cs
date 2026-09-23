using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Common.Pagination;

namespace SalusMedApi.Infrastructure.Extensions;

public static class QueryablePaginationExtensions
{
    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T, TKey>(
        this IQueryable<T> query,
        PagedRequest request,
        Expression<Func<T, TKey>> sortKeySelector,
        CancellationToken ct = default
    )
    {
        query = request.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? query.OrderByDescending(sortKeySelector)
            : query.OrderBy(sortKeySelector);

        var totalElements = await query.CountAsync(ct);

        var items = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(totalElements / (double)request.Size);

        return new PagedResponse<T>
        {
            Content = items,
            Page = request.Page,
            Size = request.Size,
            TotalElements = totalElements,
            TotalPages = totalPages,
            First = request.Page == 1,
            Last = request.Page >= totalPages,
        };
    }
}
