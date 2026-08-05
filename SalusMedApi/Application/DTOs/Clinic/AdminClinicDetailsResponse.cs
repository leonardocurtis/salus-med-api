namespace SalusMedApi.Application.DTOs.Clinic;

public record AdminClinicDetailsResponse(
    Guid ClinicId,
    string CorporateName,
    string? TradeName,
    string CnpjCode,
    DateTimeOffset CreateAt,
    DateTimeOffset? UpdatedAt,
    DateTimeOffset? DeletedAt,
    string CreatedBy,
    string? UpdatedBy,
    string? DeletedBy
);
