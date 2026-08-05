using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Interfaces.Persistence;

public interface IClinicRepository
{
    void Add(Clinic clinic);
    Task<bool> CnpjExistsAsync(Cnpj cnpj, CancellationToken ct = default);
    Task<PagedResponse<Clinic>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    );
    Task<PagedResponse<Clinic>> ListAllAsync(PagedRequest request, CancellationToken ct = default);
    Task<Clinic?> GetByPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<Clinic?> GetActiveByPublicIdAsync(Guid publicId, CancellationToken ct = default);
}
