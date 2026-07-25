using SalusMedApi.Application.DTOs.Address;
using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Mapper;

public static class PatientMapper
{
    public static RegisterPatientResponse ToResponse(this Patient patient)
    {
        return new RegisterPatientResponse(
            patient.Id,
            patient.Name,
            patient.MotherName,
            patient.CpfCode.Formatted(),
            patient.Status,
            patient.CreatedAt
        );
    }

    public static Patient ToEntity(this RegisterPatientRequest request)
    {
        var address = MapAddress(request.Address);

        return Patient.Create(
            name: request.Name,
            motherName: request.MotherName,
            fatherName: string.IsNullOrWhiteSpace(request.FatherName) ? null : request.FatherName,
            phone: request.Phone,
            email: request.Email,
            cpf: request.Cpf,
            gender: request.Gender!.Value,
            dateOfBirth: request.DateOfBirth!.Value,
            address: address
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
