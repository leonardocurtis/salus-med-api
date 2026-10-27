using System.Collections.Frozen;
using System.Collections.Immutable;
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
    public DateOnly DateOfBirth { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public Address Address { get; private set; }
    public Occupation Occupation { get; private set; }
    public DateOnly HiredAt { get; private set; }

    public long? UserId { get; private set; }
    public User? User { get; private set; }

    public long DepartmentId { get; private set; }
    public Department Department { get; private set; }

    private Employee() { }

    private static readonly FrozenDictionary<
        EmployeeStatus,
        ImmutableArray<EmployeeStatus>
    > AllowedTransitions = new Dictionary<EmployeeStatus, ImmutableArray<EmployeeStatus>>
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
    }.ToFrozenDictionary();

    public static Employee Create(
        string employeeNumber,
        string name,
        string phone,
        string email,
        string cpf,
        DateOnly dateOfBirth,
        Address address,
        Occupation occupation,
        Department department,
        DateOnly hiredAt
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
            DateOfBirth = dateOfBirth,
            Status = EmployeeStatus.Active,
            Address = address,
            Occupation = occupation,
            Department = department,
            DepartmentId = department.Id,
            HiredAt = hiredAt,
        };
    }

    private bool CanTransitionTo(EmployeeStatus target) =>
        AllowedTransitions.TryGetValue(Status, out var allowed) && allowed.Contains(target);

    private void ChangeStatus(EmployeeStatus newStatus)
    {
        if (Status == newStatus)
            throw new DomainException($"Employee is already in status '{newStatus}'.");

        if (!CanTransitionTo(newStatus))
            throw new DomainException(
                $"Invalid status transition: cannot move from '{Status}' to '{newStatus}'."
            );

        Status = newStatus;
    }

    public void ReturnToActive() => ChangeStatus(EmployeeStatus.Active);

    public void StartVacation() => ChangeStatus(EmployeeStatus.Vacation);

    public void PlaceOnLeave() => ChangeStatus(EmployeeStatus.OnLeave);

    public void Terminate(string deletedBy)
    {
        if (UserId is not null && User is null)
            throw new DomainException(
                "User must be loaded to terminate an employee with credentials."
            );

        ChangeStatus(EmployeeStatus.Terminated);
        DeletedAt = DateTimeOffset.UtcNow;
        DeletedBy = deletedBy;
        User?.Deactivate();
    }

    public void UpdateAddress(Address address)
    {
        EnsureNotTerminated();

        Address = address ?? throw new DomainException("Employee address is required.");
    }

    public void AssignDepartment(Department department)
    {
        EnsureNotTerminated();

        Department = department ?? throw new DomainException("Department cannot be null.");
        DepartmentId = department.Id;
    }

    public void Update(string? name, string? phone, string? email)
    {
        EnsureNotTerminated();

        string? newName = null;
        if (name is not null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name cannot be empty.");
            newName = name.Trim();
        }

        var newPhone = phone is not null ? Phone.Create(phone) : null;
        var newEmail = email is not null ? Email.Create(email) : null;

        if (newName is not null)
            Name = newName;
        if (newPhone is not null)
            PhoneNumber = newPhone;
        if (newEmail is not null)
            EmailAddress = newEmail;
    }

    private void EnsureNotTerminated()
    {
        if (Status == EmployeeStatus.Terminated)
            throw new ConflictException("Cannot modify a terminated employee.");
    }

    public void AssignCredentials(User user)
    {
        EnsureNotTerminated();

        if (User is not null)
            throw new DomainException("Employee already has credentials.");

        User = user ?? throw new DomainException("Employee must be linked to a User.");
        UserId = user.Id;
    }
}
