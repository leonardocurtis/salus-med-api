namespace SalusMedApi.Application.Common.Pagination;

public record PagedResponse<T>
{
    public IEnumerable<T> Content { get; init; } = [];
    public int Page { get; init; }
    public int Size { get; init; }
    public int TotalPages { get; init; }
    public long TotalElements { get; init; }
    public bool First { get; init; }
    public bool Last { get; init; }
}
