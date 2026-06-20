using SalusMedApi.Domain.Enums;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Clinic : IAuditable
{
    public long Id { get; private set; }
    public string CorporateName { get; private set; }
    public string? TradeName { get; private set; }
    public string Cnpj { get; private set; }
    public ClinicStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Clinic() { }

    public static Clinic Create(string corporateName, string? tradeName, string cnpj) =>
        new Clinic
        {
            CorporateName = corporateName.Trim(),
            TradeName = tradeName?.Trim(),
            Cnpj = cnpj.Trim(),
            Status = ClinicStatus.Active,
        };

    public void MarkAsActive() => Status = ClinicStatus.Active;

    public void MarkAsDeactivated() => Status = ClinicStatus.Deactivated;
}
