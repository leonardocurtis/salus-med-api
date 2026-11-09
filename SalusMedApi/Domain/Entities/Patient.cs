using SalusMedApi.Application.Exceptions;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Domain.Entities;

public class Patient : AuditableEntity
{
    public string Name { get; private set; }
    public string MotherName { get; private set; }
    public string? FatherName { get; private set; }
    public Phone PhoneNumber { get; private set; }
    public Email EmailAddress { get; private set; }
    public Cpf CpfCode { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Address Address { get; private set; }
    public PatientStatus Status { get; private set; }

    public long? UserId { get; private set; }
    public User? User { get; private set; }

    private Patient() { }

    // Implementar metodo de atualização de dados para falecidos
    public static Patient Create(
        string name,
        string motherName,
        string? fatherName,
        string phone,
        string email,
        string cpf,
        Gender gender,
        DateOnly dateOfBirth,
        Address address
    )
    {
        EnsureValidDateOfBirth(dateOfBirth);
        EnsureValidGender(gender);

        if (address is null)
            throw new DomainException("Patient address is required.");

        return new Patient
        {
            Name = NormalizeAndValidateName(name),
            MotherName = NormalizeAndValidateMotherName(motherName),
            FatherName = NormalizeAndValidateFatherName(fatherName),
            PhoneNumber = Phone.Create(phone),
            EmailAddress = Email.Create(email),
            CpfCode = Cpf.Create(cpf),
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Status = PatientStatus.Active,
            Address = address,
        };
    }

    public void UpdateAddress(Address address)
    {
        EnsureNotDeceased();
        Address = address ?? throw new DomainException("Address is required.");
    }

    public void ChangeName(string name)
    {
        EnsureNotDeceased();
        Name = NormalizeAndValidateName(name);
    }

    public void ChangeParents(string motherName, string? fatherName)
    {
        EnsureNotDeceased();
        var newMotherName = NormalizeAndValidateMotherName(motherName);
        var newFatherName = NormalizeAndValidateFatherName(fatherName);

        MotherName = newMotherName;
        FatherName = newFatherName;
    }

    public void ChangeGender(Gender gender)
    {
        EnsureNotDeceased();
        EnsureValidGender(gender);
        Gender = gender;
    }

    public void ChangePhone(string phone)
    {
        EnsureNotDeceased();
        PhoneNumber = Phone.Create(phone);
    }

    public void ChangeEmail(string email)
    {
        EnsureNotDeceased();
        EmailAddress = Email.Create(email);
    }

    public void MarkAsDeceased()
    {
        if (Status == PatientStatus.Deceased)
            throw new DomainException("Patient is already deceased.");

        Status = PatientStatus.Deceased;
    }

    public void RegisterAsDeceased()
    {
        if (Status == PatientStatus.Deceased)
            throw new DomainException("Patient is already registered as deceased.");

        Status = PatientStatus.Deceased;
    }

    public void AssignCredentials(User user)
    {
        EnsureNotDeceased();

        if (user is null)
            throw new DomainException("Patient must be linked to a User.");

        if (UserId is not null && User is null)
            throw new DomainException("Patient already has credentials.");

        User = user ?? throw new DomainException("Patient must be linked to a User.");
    }

    private void EnsureNotDeceased()
    {
        if (Status == PatientStatus.Deceased)
            throw new DomainException("Cannot modify a deceased patient.");
    }

    private static string NormalizeAndValidateName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new DomainException("Patient name is required.")
            : name.Trim();

    private static string NormalizeAndValidateMotherName(string motherName) =>
        string.IsNullOrWhiteSpace(motherName)
            ? throw new DomainException("Mother's name is required.")
            : motherName.Trim();

    private static string? NormalizeAndValidateFatherName(string? fatherName) =>
        string.IsNullOrWhiteSpace(fatherName) ? null : fatherName.Trim();

    private static void EnsureValidGender(Gender gender)
    {
        if (!Enum.IsDefined(gender))
            throw new DomainException("Invalid gender.");
    }

    private static void EnsureValidDateOfBirth(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (dateOfBirth > today)
            throw new DomainException("Date of birth cannot be in the future.");
    }
}
