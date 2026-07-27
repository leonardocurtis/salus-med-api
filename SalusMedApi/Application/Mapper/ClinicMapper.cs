using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Mapper;

public static class ClinicMapper
{
    public static Clinic ToEntity(this RegisterClinicRequest request)
    {
        return Clinic.Create(corporateName: request.CorporateName, 
            tradeName: request.TradeName, cnpj: request.Cnpj);
    }

    public static RegisterClinicResponse ToResponse(this Clinic clinic)
    {
        return new RegisterClinicResponse(
            clinic.PublicId,
            clinic.CorporateName,
            clinic.CnpjCode.Formatted
        );
    }
}