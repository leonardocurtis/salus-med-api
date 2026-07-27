using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;
using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services;

public class ClinicService(IUnitOfWorkRepository unitOfWork, IClinicRepository clinicRepository) : IClinicService
{
    public async Task<RegisterClinicResponse> RegisterClinicAsync(RegisterClinicRequest request)
    {
        var cnpj = Cnpj.Create(request.Cnpj);

        if (await clinicRepository.CnpjExistsAsync(cnpj))
            throw new ConflictException($"Cpf {request.Cnpj} already in use.");

        var clinic = request.ToEntity();

        clinicRepository.Add(clinic);
        await unitOfWork.CommitAsync();

        return clinic.ToResponse();
    }
}