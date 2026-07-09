using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class HealthUnit : IAuditable
{
    public long Id { get; private set; }
    public Cnes? CnesCode { get; private set; }
    public Cnpj CnpjCode { get; private set; }
    public string TechnicalManager { get; private set; }
    public Crm TechnicalManagerCouncilNumber { get; private set; }
    public Address Address { get; private set; }
    public Phone PhoneNumber { get; private set; }
    public HealthUnitStatus Status { get; private set; }

    public long ClinicId { get; private set; }
    public Clinic Clinic { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private HealthUnit() { }

    private static readonly Dictionary<HealthUnitStatus, HealthUnitStatus[]> AllowedTransitions =
        new()
        {
            [HealthUnitStatus.PendingRegistration] =
            [
                HealthUnitStatus.Active,
                HealthUnitStatus.Deactivated,
            ],
            [HealthUnitStatus.Active] = [HealthUnitStatus.Deactivated],
            [HealthUnitStatus.Deactivated] = [],
        };

    public static HealthUnit Create(
        string cnes,
        string cnpj,
        string managerName,
        string managerNumber,
        Address address,
        string phone
    )
    {
        if (string.IsNullOrWhiteSpace(managerName))
            throw new DomainException("Technical manager name is required.");

        var cnesVo = string.IsNullOrWhiteSpace(cnes) ? null : Cnes.Create(cnes);

        return new HealthUnit
        {
            CnesCode = cnesVo,
            CnpjCode = Cnpj.Create(cnpj),
            TechnicalManager = managerName.Trim(),
            TechnicalManagerCouncilNumber = Crm.Create(managerNumber),
            Address = address,
            PhoneNumber = Phone.Create(phone),
            Status = cnesVo is null
                ? HealthUnitStatus.PendingRegistration
                : HealthUnitStatus.Active,
        };
    }

    public static void EnsureCanTransition(HealthUnitStatus current, HealthUnitStatus target)
    {
        if (!AllowedTransitions[current].Contains(target))
            throw new DomainException($"Cannot transition HealthUnit from {current} to {target}.");
    }

    public void Active()
    {
        EnsureCanTransition(Status, HealthUnitStatus.Active);

        if (CnesCode is null)
            throw new DomainException("Cannot activate a HealthUnit without a registered CNES.");

        Status = HealthUnitStatus.Active;
    }

    public void Deactivate()
    {
        EnsureCanTransition(Status, HealthUnitStatus.Deactivated);
        Status = HealthUnitStatus.Deactivated;
    }
}
