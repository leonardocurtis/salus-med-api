using SalusMedApi.Application.Common.Pagination;
using SalusMedApi.Application.DTOs.Clinic;
using SalusMedApi.Application.Exceptions;
using SalusMedApi.Application.Interfaces.Auth;
using SalusMedApi.Application.Interfaces.Persistence;
using SalusMedApi.Application.Interfaces.Services;
using SalusMedApi.Application.Mapper;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Application.Services;

public sealed class ClinicService(
    IUnitOfWorkRepository unitOfWork,
    IClinicRepository clinicRepository,
    ICurrentUserService currentUser,
    IClinicQueries clinicQueries
) : IClinicService
{
    public async Task<RegisterClinicResponse> RegisterClinicAsync(
        RegisterClinicRequest request,
        CancellationToken ct = default
    )
    {
        var cnpj = Cnpj.Create(request.Cnpj);

        if (await clinicRepository.CnpjExistsAsync(cnpj, ct))
            throw new ConflictException($"Cnpj {request.Cnpj} already in use.");

        var clinic = request.ToEntity();

        clinicRepository.Add(clinic);
        await unitOfWork.CommitAsync(ct);

        return clinic.ToResponse();
    }

    public Task<PagedResponse<ClinicListResponse>> ListAllActiveAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) => clinicQueries.ListAllActiveAsync(request, ct);

    public Task<PagedResponse<AdminClinicListResponse>> ListAllAdminAsync(
        PagedRequest request,
        CancellationToken ct = default
    ) => clinicQueries.ListAllByAnyStatusAsync(request, ct);

    public async Task<ClinicDetailsResponse> GetClinicDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await clinicQueries.GetActiveDetailsByPublicIdAsync(publicId, ct)
        ?? throw new ResourceNotFoundException("Clinic not found");

    public async Task<AdminClinicDetailsResponse> GetAdminClinicDetailsAsync(
        Guid publicId,
        CancellationToken ct = default
    ) =>
        await clinicQueries.GetAdminDetailsByPublicIdAsync(publicId, ct)
        ?? throw new ResourceNotFoundException("Clinic not found");

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
