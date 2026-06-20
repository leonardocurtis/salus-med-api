using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class HealthUnit : IAuditable
{
    public long Id { get; private set; }
    public string Cnes { get; private set; }
    public string Cnpj { get; private set; }
    public string TechnicalManagerName { get; private set; }
    public string TechnicalManagerCouncilNumber { get; private set; }
    public Address Address { get; private set; }
    public string Phone { get; private set; }
    public HealthUnitStatus Status { get; private set; }

    public long ClinicId { get; private set; }
    public Clinic Clinic { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private HealthUnit() { }

    public static HealthUnit Create(
        string cnes,
        string cnpj,
        string managerName,
        string managerCouncilNumber,
        Address address,
        string phone
    ) =>
        new HealthUnit
        {
            Cnes = cnes.Trim(),
            Cnpj = cnpj.Trim(),
            TechnicalManagerName = managerName.Trim(),
            TechnicalManagerCouncilNumber = managerCouncilNumber.Trim(),
            Address = address,
            Phone = phone.Trim(),
            Status = HealthUnitStatus.Active,
        };

    public void MarkAsActive() => Status = HealthUnitStatus.Active;

    public void MarkAsDeactivated() => Status = HealthUnitStatus.Deactivated;
}
