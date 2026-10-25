using System.ComponentModel.DataAnnotations;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.Common.Pagination;

public record PagedRequest
{
    public const int MaxPage = 1_000_000;
    public const int MaxSize = 100;

    [Range(1, MaxPage)]
    public int Page { get; init; } = 1;

    [Range(1, MaxSize)]
    public int Size { get; init; } = 10;

    [EnumDataType(typeof(SortDirection))]
    public SortDirection SortDir { get; init; } = SortDirection.Asc;
}
