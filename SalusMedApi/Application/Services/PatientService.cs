using SalusMedApi.Application.DTOs.Patient;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services;

public sealed class PatientService(
    IPatientRepository patientRepository,
    IUnitOfWorkRepository unitOfWork
) : IPatientService
{
    public async Task<RegisterPatientResponse> RegisterPatientAsync(
        RegisterPatientRequest patientRequest
    )
    {
        var cpf = Cpf.Create(patientRequest.Cpf);

        if (await patientRepository.CpfExistsAsync(cpf))
            throw new ConflictException($"Cpf {patientRequest.Cpf} already in use.");

        var patient = patientRequest.ToEntity();

        patientRepository.Add(patient);
        await unitOfWork.CommitAsync();

        return patient.ToResponse();
    }
}
