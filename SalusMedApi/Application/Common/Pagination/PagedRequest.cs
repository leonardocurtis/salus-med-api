namespace SalusMedApi.Application.Common.Pagination;

public record PagedRequest
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 10;
    public string SortBy { get; init; } = "Name";
    public string SortDir { get; init; } = "asc";
}
