namespace SalusMedApi.Application.DTOs.Clinic;

public record UpdateClinicResponse(Guid Id, string CorporationName, string? TradeName, string Cnpj);
