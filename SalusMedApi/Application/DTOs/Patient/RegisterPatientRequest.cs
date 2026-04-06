using SalusMedApi.Application.DTOs.Auth;

namespace SalusMedApi.Application.DTOs.Patient;

public record RegisterPatientRequest(CredentialRequest Credentials, CreatePatientRequest Patient);
