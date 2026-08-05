using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.DTOs.Clinic;

public record ClinicListResponse(
    Guid Id,
    string CorporateName,
    string? TradeName,
    string Cnpj,
    ClinicStatus Status
);
