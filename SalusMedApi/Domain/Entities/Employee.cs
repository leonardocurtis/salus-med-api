using SalusMedApi.Application.Exceptions;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;

namespace SalusMedApi.Domain.Entities;

public class Employee : AuditableEntity
{
    public string EmployeeNumber { get; private set; }
    public string Name { get; private set; }
    public Phone PhoneNumber { get; private set; }
    public Email EmailAddress { get; private set; }
    public Cpf CpfNumber { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public Address Address { get; private set; }
    public Occupation Occupation { get; private set; }

    public long? UserId { get; private set; }
    public User? User { get; private set; }

    public long DepartmentId { get; private set; }
    public Department Department { get; private set; }

    private Employee() { }

    private static readonly Dictionary<EmployeeStatus, EmployeeStatus[]> AllowedTransitions = new()
    {
        [EmployeeStatus.Active] =
        [
            EmployeeStatus.OnLeave,
            EmployeeStatus.Terminated,
            EmployeeStatus.Vacation,
        ],
        [EmployeeStatus.OnLeave] =
        [
            EmployeeStatus.Active,
            EmployeeStatus.Terminated,
            EmployeeStatus.Vacation,
        ],
        [EmployeeStatus.Vacation] =
        [
            EmployeeStatus.OnLeave,
            EmployeeStatus.Active,
            EmployeeStatus.Terminated,
        ],
        [EmployeeStatus.Terminated] = [],
    };

    public static Employee Create(
        string employeeNumber,
        string name,
        string phone,
        string email,
        string cpf,
        Gender gender,
        DateOnly dateOfBirth,
        Address address,
        Occupation occupation,
        Department department
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Employee name cannot be empty.");

        if (address is null)
            throw new DomainException("Employee address is required.");

        if (department is null)
            throw new DomainException("Employee must be linked to a Department.");

        return new Employee
        {
            EmployeeNumber = employeeNumber,
            Name = name.Trim(),
            PhoneNumber = Phone.Create(phone),
            EmailAddress = Email.Create(email),
            CpfNumber = Cpf.Create(cpf),
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Status = EmployeeStatus.Active,
            Address = address,
            Occupation = occupation,
            Department = department,
            DepartmentId = department.Id,
        };
    }

    private void ChangeStatus(EmployeeStatus newStatus)
    {
        if (Status == newStatus)
            throw new DomainException($"Employee is already in status '{newStatus}'.");

        if (!AllowedTransitions[Status].Contains(newStatus))
            throw new DomainException(
                $"Invalid status transition: cannot move from '{Status}' to '{newStatus}'."
            );

        Status = newStatus;
    }

    public void ReturnToActive() => ChangeStatus(EmployeeStatus.Active);

    public void StartVacation() => ChangeStatus(EmployeeStatus.Vacation);

    public void PlaceOnLeave() => ChangeStatus(EmployeeStatus.OnLeave);

    public void Terminate() => ChangeStatus(EmployeeStatus.Terminated);

    public void UpdateAddress(Address address) => Address = address;

    public void ChangeGender(Gender gender) => Gender = gender;

    public void AssignDepartment(Department department) =>
        Department = department ?? throw new DomainException("Department cannot be null.");

    public void UpdateContact(string phone) => PhoneNumber = Phone.Create(phone);

    public void UpdateEmail(string email) => EmailAddress = Email.Create(email);

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty.");

        Name = name.Trim();
    }

    public void AssignCredentials(User user)
    {
        if (User is not null)
            throw new DomainException("Employee already has credentials.");

        User = user ?? throw new DomainException("Employee must be linked to a User.");
        UserId = user.Id;
    }
}
