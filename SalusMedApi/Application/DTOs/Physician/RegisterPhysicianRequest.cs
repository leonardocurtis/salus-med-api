using SalusMedApi.Application.DTOs.Auth;

namespace SalusMedApi.Application.DTOs.Physician;

public record RegisterPhysicianRequest(
    CredentialRequest Credentials,
    CreatePhysicianRequest Physician
) { }
