using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Infrastructure.Extensions;

public static class QueryablePaginationExtensions
{
    public static async Task<PagedResponse<TDto>> ToPagedResponseAsync<TEntity, TDto, TKey, TTie>(
        this IQueryable<TEntity> query,
        PagedRequest request,
        Expression<Func<TEntity, TKey>> sortKeySelector,
        Expression<Func<TEntity, TTie>> tieBreakerSelector,
        Expression<Func<TEntity, TDto>> projection,
        CancellationToken ct = default
    )
    {
        EnsureValid(request);

        var ordered = IsDescending(request)
            ? query.OrderByDescending(sortKeySelector).ThenByDescending(tieBreakerSelector)
            : query.OrderBy(sortKeySelector).ThenBy(tieBreakerSelector);

        var totalElements = await ordered.LongCountAsync(ct);

        var items = await ordered.ApplyPage(request).Select(projection).ToListAsync(ct);

        return BuildResponse(items, totalElements, request);
    }

    private static bool IsDescending(PagedRequest request) => request.SortDir == SortDirection.Desc;

    private static void EnsureValid(PagedRequest request)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(request.Page, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(request.Page, PagedRequest.MaxPage);
        ArgumentOutOfRangeException.ThrowIfLessThan(request.Size, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(request.Size, PagedRequest.MaxSize);
    }

    private static IQueryable<T> ApplyPage<T>(this IQueryable<T> query, PagedRequest request) =>
        query.Skip((request.Page - 1) * request.Size).Take(request.Size);

    private static PagedResponse<T> BuildResponse<T>(
        List<T> items,
        long totalElements,
        PagedRequest request
    )
    {
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
