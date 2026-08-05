using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Clinic;

public record AdminClinicListResponse(
    Guid ClinicId,
    string CorporateName,
    string? TradeName,
    string CnpjCode,
    ClinicStatus Status,
    DateTimeOffset CreateAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt,
    string CreatedBy,
    string? UpdatedBy,
    string? DeletedBy
);
