using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Physician : IAuditable
{
    public long Id { get; set; }
    public string MedicalRegistration { get; set; }
    public Specialty Specialty { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public long EmployeeId { get; set; }
    public Employee Employee { get; set; }
}
