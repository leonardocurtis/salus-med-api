using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Physician : IAuditable
{
    public long Id { get; private set; }
    public string MedicalRegistration { get; private set; }
    public Specialty Specialty { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public long EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    private Physician() { }

    public static Physician Create(
        string medicalRegistration,
        Specialty specialty,
        Employee employee
    ) =>
        new Physician
        {
            MedicalRegistration = medicalRegistration.Trim(),
            Specialty = specialty,
            Employee = employee,
        };

    public void UpdateRegistration(string medicalRegistration) =>
        MedicalRegistration = medicalRegistration.Trim();

    public void UpdateSpecialty(Specialty specialty) => Specialty = specialty;
}
