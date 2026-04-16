using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Mapper;

public static class PatientMapper
{
    public static Patient ToEntity(RegisterPatientRequest request)
    {
        var user = MapUser(request.Credentials);

        var address = MapAddress(request.Patient.Address);

        return Patient.Create(
            name: request.Patient.Name,
            motherName: request.Patient.MotherName,
            fatherName: string.IsNullOrWhiteSpace(request.Patient.FatherName)
                ? null
                : request.Patient.FatherName,
            phone: request.Patient.Phone,
            cpf: request.Patient.Cpf,
            gender: request.Patient.Gender!.Value,
            dateOfBirth: request.Patient.DateOfBirth!.Value,
            address: address,
            user: user
        );
    }

    private static User MapUser(CredentialRequest credentials) =>
        User.Create(
            email: credentials.Email,
            passwordHash: BCrypt.Net.BCrypt.HashPassword(credentials.Password),
            role: Role.Patient
        );

    private static Address MapAddress(AddressRequest address) =>
        Address.Create(
            street: address.Street,
            number: address.Number,
            neighborhood: address.Neighborhood,
            postalCode: address.PostalCode,
            complement: string.IsNullOrWhiteSpace(address.Complement) ? null : address.Complement,
            city: address.City,
            state: address.State
        );
}
