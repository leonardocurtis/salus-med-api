using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Domain.Entities;

public class Clinic : AuditableEntity
{
    public string CorporateName { get; private set; }
    public string? TradeName { get; private set; }
    public Cnpj CnpjCode { get; private set; }
    public ClinicStatus Status { get; private set; }

    private Clinic() { }

    private static readonly Dictionary<ClinicStatus, ClinicStatus[]> AllowedTransitions = new()
    {
        [ClinicStatus.Active] = [ClinicStatus.Suspended, ClinicStatus.Deactivated],
        [ClinicStatus.Suspended] = [ClinicStatus.Active, ClinicStatus.Deactivated],
        [ClinicStatus.Deactivated] = [],
    };

    public static Clinic Create(string corporateName, string? tradeName, string cnpj)
    {
        if (string.IsNullOrWhiteSpace(corporateName))
            throw new DomainException("Corporate name is required.");

        return new Clinic
        {
            CorporateName = corporateName.Trim(),
            TradeName = string.IsNullOrWhiteSpace(tradeName) ? null : tradeName.Trim(),
            CnpjCode = Cnpj.Create(cnpj),
            Status = ClinicStatus.Active,
        };
    }

    private void ChangeStatus(ClinicStatus newStatus)
    {
        if (Status == newStatus)
            throw new DomainException($"Clinic is already in status '{newStatus}'.");

        if (!AllowedTransitions[Status].Contains(newStatus))
            throw new DomainException(
                $"Invalid status transition: cannot move from '{Status}' to '{newStatus}'."
            );

        Status = newStatus;
    }

    public void Activate() => ChangeStatus(ClinicStatus.Active);

    public void Deactivate() => ChangeStatus(ClinicStatus.Deactivated);

    public void Suspend() => ChangeStatus(ClinicStatus.Suspended);
}
