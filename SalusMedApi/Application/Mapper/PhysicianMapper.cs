using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Mapper;

public static class PhysicianMapper
{
    public static Physician ToEntity(RegisterPhysicianRequest request, Department department)
    {
        var user = MapUser(request.Credentials);

        var address = MapAddress(request.Physician.Address);

        var employee = Employee.Create(
            name: request.Physician.Name,
            phone: request.Physician.Phone,
            cpf: request.Physician.Cpf,
            gender: request.Physician.Gender!.Value,
            dateOfBirth: request.Physician.DateOfBirth!.Value,
            address: address,
            user: user,
            role: EmployeeRole.Physician,
            department: department
        );

        return Physician.Create(
            medicalRegistration: request.Physician.MedicalRegistration,
            specialty: request.Physician.Specialty!.Value,
            employee: employee
        );
    }

    private static User MapUser(CredentialRequest credentials) =>
        User.Create(
            email: credentials.Email,
            passwordHash: BCrypt.Net.BCrypt.HashPassword(credentials.Password),
            role: Role.Physician
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
