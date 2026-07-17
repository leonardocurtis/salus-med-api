using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Domain.Entities;

public class Physician : AuditableEntity
{
    public Crm MedicalRegistration { get; private set; }
    public Specialty Specialty { get; private set; }

    public long EmployeeId { get; private set; }
    public Employee Employee { get; private set; }

    private Physician() { }

    public static Physician Create(
        string medicalRegistration,
        Specialty specialty,
        Employee employee
    )
    {
        if (employee is null)
            throw new DomainException("A physician must be linked to an existing employee.");

        EnsureValidSpecialty(specialty);

        return new Physician
        {
            MedicalRegistration = Crm.Create(medicalRegistration),
            Specialty = specialty,
            Employee = employee,
            EmployeeId = employee.Id,
        };
    }

    public void UpdateRegistration(string medicalRegistration) =>
        MedicalRegistration = Crm.Create(medicalRegistration);

    public void UpdateSpecialty(Specialty specialty)
    {
        EnsureValidSpecialty(specialty);
        Specialty = specialty;
    }

    private static void EnsureValidSpecialty(Specialty specialty)
    {
        if (!Enum.IsDefined(specialty))
            throw new DomainException($"'{specialty}' is not a valid medical specialty.");
    }
}
