using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Application.DTOs.Auth;
using SalusMedApi.Application.DTOs.Physician;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Mapper;

public static class PhysicianMapper
{
    public static RegisterPhysicianResponse ToResponse(this Physician physician)
    {
        return new RegisterPhysicianResponse(
            physician.PublicId,
            physician.Employee.EmployeeNumber,
            physician.Employee.Name,
            physician.MedicalRegistration.Formatted,
            physician.Specialty,
            physician.Employee.Status,
            physician.CreatedAt
        );
    }

    public static Physician ToEntity(
        this RegisterPhysicianRequest request,
        Department department,
        string employeeNumber,
        Occupation occupation
    )
    {
        var address = MapAddress(request.Address);

        var employee = Employee.Create(
            employeeNumber: employeeNumber,
            name: request.Name,
            phone: request.Phone,
            email: request.Email,
            cpf: request.Cpf,
            gender: request.Gender!.Value,
            dateOfBirth: request.DateOfBirth!.Value,
            address: address,
            occupation: occupation,
            department: department
        );

        return Physician.Create(
            medicalRegistration: request.Crm,
            specialty: request.Specialty!.Value,
            employee: employee
        );
    }

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
