using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services;

public class ClinicService(
    IUnitOfWorkRepository unitOfWork,
    IClinicRepository clinicRepository,
    ICurrentUserService currentUser
) : IClinicService
{
    public async Task<RegisterClinicResponse> RegisterClinicAsync(
        RegisterClinicRequest request,
        CancellationToken ct = default
    )
    {
        var cnpj = Cnpj.Create(request.Cnpj);

        if (await clinicRepository.CnpjExistsAsync(cnpj, ct))
            throw new ConflictException($"Cpf {request.Cnpj} already in use.");

        var clinic = request.ToEntity();

        clinicRepository.Add(clinic);
        await unitOfWork.CommitAsync(ct);

        return clinic.ToResponse();
    }

    public async Task<PagedResponse<ClinicListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    )
    {
        var pagedClinics = await clinicRepository.ListAllActiveAsync(request, ct);

        var dto = pagedClinics.Content.Select(c => new ClinicListResponse(
            c.PublicId,
            c.CorporateName,
            c.TradeName,
            c.CnpjCode.Value,
            c.Status
        ));

        return new PagedResponse<ClinicListResponse>()
        {
            Content = dto,
            Page = pagedClinics.Page,
            Size = pagedClinics.Size,
            TotalElements = pagedClinics.TotalElements,
            TotalPages = pagedClinics.TotalPages,
            First = pagedClinics.First,
            Last = pagedClinics.Last,
        };
    }

    public async Task<PagedResponse<AdminClinicListResponse>> ListAllAdminAsync(
        PagedRequest request,
        CancellationToken ct = default
    )
    {
        var pagedClinics = await clinicRepository.ListAllAsync(request, ct);

        var dto = pagedClinics.Content.Select(c => new AdminClinicListResponse(
            c.PublicId,
            c.CorporateName,
            c.TradeName,
            c.CnpjCode.Value,
            c.Status,
            c.CreatedAt,
            c.UpdatedAt,
            c.DeletedAt,
            c.CreatedBy,
            c.UpdatedBy,
            c.DeletedBy
        ));

        return new PagedResponse<AdminClinicListResponse>()
        {
            Content = dto,
            Page = pagedClinics.Page,
            Size = pagedClinics.Size,
            TotalElements = pagedClinics.TotalElements,
            TotalPages = pagedClinics.TotalPages,
            First = pagedClinics.First,
            Last = pagedClinics.Last,
        };
    }

    public async Task<ClinicDetailsResponse> GetClinicDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    )
    {
        var clinic =
            await clinicRepository.GetActiveByPublicIdAsync(publicId, ct)
            ?? throw new ResourceNotFoundException("Clinic not found");

        return clinic.ToDetails();
    }

    public async Task<AdminClinicDetailsResponse> GetAdminClinicDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    )
    {
        var clinic =
            await clinicRepository.GetByPublicIdAsync(publicId, ct)
            ?? throw new ResourceNotFoundException("Clinic not found");

        return clinic.ToDetailsAdmin();
    }

    public async Task DeactivateClinicAsync(Guid clinicId, CancellationToken ct = default)
    {
        var clinic =
            await clinicRepository.GetByPublicIdAsync(clinicId, ct)
            ?? throw new ResourceNotFoundException("Clinic not found");

        clinic.Deactivate(currentUser.EmployeeIdNumber ?? "SYSTEM");
        await unitOfWork.CommitAsync(ct);
    }

    public async Task<UpdateClinicResponse> UpdateClinicAsync(
        Guid clinicId,
        UpdateClinicRequest request,
        CancellationToken ct = default
    )
    {
        var clinic =
            await clinicRepository.GetActiveByPublicIdAsync(clinicId, ct)
            ?? throw new ResourceNotFoundException($"Clinic '{clinicId}' not found.");

        clinic.Update(request.CorporationName, request.TradeName, request.Cnpj);
        await unitOfWork.CommitAsync(ct);

        return clinic.ToUpdate();
    }

    public async Task SuspendClinicAsync(Guid clinicId, CancellationToken ct = default)
    {
        var clinic =
            await clinicRepository.GetByPublicIdAsync(clinicId, ct)
            ?? throw new ResourceNotFoundException("Clinic not found");

        clinic.Suspend();
        await unitOfWork.CommitAsync(ct);
    }

    public async Task ActivateClinicAsync(Guid clinicId, CancellationToken ct = default)
    {
        var clinic =
            await clinicRepository.GetByPublicIdAsync(clinicId, ct)
            ?? throw new ResourceNotFoundException("Clinic not found");

        clinic.Activate();
        await unitOfWork.CommitAsync(ct);
    }
}
