using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Clinic;

public record ClinicDetailsResponse(
    string CorporateName,
    string? TradeName,
    string Cnpj,
    ClinicStatus Status
);
