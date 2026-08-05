using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Domain.Entities;

namespace SalusMedApi.Application.Mapper;

public static class ClinicMapper
{
    public static Clinic ToEntity(this RegisterClinicRequest request) =>
        Clinic.Create(
            corporateName: request.CorporateName,
            tradeName: request.TradeName,
            cnpj: request.Cnpj
        );

    extension(Clinic clinic)
    {
        public RegisterClinicResponse ToResponse() =>
            new(clinic.PublicId, clinic.CorporateName, clinic.CnpjCode.Formatted);

        public ClinicDetailsResponse ToDetails() =>
            new(
                clinic.CorporateName,
                clinic.CorporateName,
                clinic.CnpjCode.Formatted,
                clinic.Status
            );

        public UpdateClinicResponse ToUpdate() =>
            new(clinic.PublicId, clinic.CorporateName, clinic.TradeName, clinic.CnpjCode.Formatted);

        public AdminClinicDetailsResponse ToDetailsAdmin() =>
            new(
                clinic.PublicId,
                clinic.CorporateName,
                clinic.TradeName,
                clinic.CnpjCode.Formatted,
                clinic.CreatedAt,
                clinic.UpdatedAt,
                clinic.DeletedAt,
                clinic.CreatedBy,
                clinic.UpdatedBy,
                clinic.DeletedBy
            );
    }
}
