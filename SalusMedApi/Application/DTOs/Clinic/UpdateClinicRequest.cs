namespace SalusMedApi.Application.DTOs.Clinic;

public record UpdateClinicRequest(string? CorporationName, string? TradeName, string? Cnpj);
