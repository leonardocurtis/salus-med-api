using SalusMedApi.CrossCutting.Exceptions;
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
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Patient name is required.");

        if (string.IsNullOrWhiteSpace(motherName))
            throw new DomainException("Mother's name is required.");

        if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException("Date of birth cannot be in the future.");

        return new Patient
        {
            Name = name.Trim(),
            MotherName = motherName.Trim(),
            FatherName = string.IsNullOrWhiteSpace(fatherName) ? null : fatherName.Trim(),
            PhoneNumber = Phone.Create(phone),
            EmailAddress = Email.Create(email),
            CpfCode = Cpf.Create(cpf),
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Status = PatientStatus.Active,
            Address = address,
        };
    }

    public void UpdateContact(string phone) => PhoneNumber = Phone.Create(phone);

    public void UpdateEmail(string email) => EmailAddress = Email.Create(email);

    public void UpdateAddress(Address address)
    {
        Address = address ?? throw new DomainException("Address is required.");
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Patient name is required.");

        Name = name.Trim();
    }

    public void UpdateFatherName(string? name)
    {
        FatherName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
    }

    public void UpdateGender(Gender gender) => Gender = gender;

    public void Activate()
    {
        if (Status == PatientStatus.Deceased)
            throw new DomainException("Cannot change the status of a deceased patient.");

        if (Status == PatientStatus.Active)
            throw new DomainException("Patient is already active.");

        Status = PatientStatus.Active;
    }

    public void RegisterAsDeceased()
    {
        if (Status == PatientStatus.Deceased)
            throw new DomainException("Patient is already registered as deceased.");

        Status = PatientStatus.Deceased;
    }

    private void AssignCredentials(User user)
    {
        if (User is not null)
            throw new DomainException("Patient already has credentials.");

        User = user ?? throw new DomainException("Patient must be linked to a User.");
        UserId = user.Id;
    }
}
