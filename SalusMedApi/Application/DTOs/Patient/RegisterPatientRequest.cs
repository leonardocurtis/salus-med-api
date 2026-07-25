using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Application.DTOs.Patient;

public record RegisterPatientRequest(
    string Name,
    string MotherName,
    string? FatherName,
    string Phone,
    string Email,
    string Cpf,
    Gender? Gender,
    DateOnly? DateOfBirth,
    AddressRequest Address
);
