using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Patient : IAuditable
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string MotherName { get; private set; }
    public string? FatherName { get; private set; }
    public string Phone { get; private set; }
    public string Cpf { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Address Address { get; private set; }
    public PatientStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public long UserId { get; private set; }
    public User User { get; private set; }

    private Patient() { }

    public static Patient Create(
        string name,
        string motherName,
        string? fatherName,
        string phone,
        string cpf,
        Gender gender,
        DateOnly dateOfBirth,
        Address address,
        User user
    ) =>
        new Patient
        {
            Name = name.Trim(),
            MotherName = motherName.Trim(),
            FatherName = fatherName?.Trim(),
            Phone = phone.Trim(),
            Cpf = cpf.Trim(),
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Status = PatientStatus.Active,
            Address = address,
            User = user,
        };

    public void UpdateContact(string phone) => Phone = phone.Trim();

    public void UpdateAddress(Address address) => Address = address;

    public void UpdateName(string name) => Name = name.Trim();

    public void UpdateFatherName(string name) => FatherName = name.Trim();

    public void UpdateGender(Gender gender) => Gender = gender;

    public void MarkAsActive() => Status = PatientStatus.Active;

    public void MarkAsDeactivated() => Status = PatientStatus.Deactivated;

    public void MarkAsBlocked() => Status = PatientStatus.Blocked;

    public void MarkAsPending() => Status = PatientStatus.Pending;

    public void MarkAsDeceased() => Status = PatientStatus.Deceased;
}
