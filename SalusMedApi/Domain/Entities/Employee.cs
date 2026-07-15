using SalusMedApi.CrossCutting.Exceptions;
using SalusMedApi.Domain.Common;
using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Employee : AuditableEntity
{
    public string Name { get; private set; }
    public Phone PhoneNumber { get; private set; }
    public Cpf CpfNumber { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public Address Address { get; private set; }
    public EmployeeRole Role { get; private set; }

    public long UserId { get; private set; }
    public User User { get; private set; }

    public long DepartmentId { get; private set; }
    public Department Department { get; private set; }

    private Employee() { }

    private static readonly Dictionary<EmployeeStatus, EmployeeStatus[]> AllowedTransitions = new()
    {
        [EmployeeStatus.Active] = [EmployeeStatus.OnLeave, EmployeeStatus.Terminated],
        [EmployeeStatus.OnLeave] = [EmployeeStatus.Active, EmployeeStatus.Terminated],
        [EmployeeStatus.Terminated] = [],
    };

    public static Employee Create(
        string name,
        string phone,
        string cpf,
        Gender gender,
        DateOnly dateOfBirth,
        Address address,
        User user,
        EmployeeRole role,
        Department department
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Employee name cannot be empty.");

        if (address is null)
            throw new DomainException("Employee address is required.");

        if (user is null)
            throw new DomainException("Employee must be linked to a User.");

        if (department is null)
            throw new DomainException("Employee must be linked to a Department.");

        return new Employee
        {
            Name = name.Trim(),
            PhoneNumber = Phone.Create(phone),
            CpfNumber = Cpf.Create(cpf),
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Status = EmployeeStatus.Active,
            Address = address,
            User = user,
            UserId = user.Id,
            Role = role,
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

    public void ReturnFromLeave() => ChangeStatus(EmployeeStatus.Active);

    public void PlaceOnLeave() => ChangeStatus(EmployeeStatus.OnLeave);

    public void Terminate() => ChangeStatus(EmployeeStatus.Terminated);

    public void UpdateAddress(Address address) => Address = address;

    public void ChangeGender(Gender gender) => Gender = gender;

    public void AssignDepartment(Department department) =>
        Department = department ?? throw new DomainException("Department cannot be null.");

    public void UpdateContact(string phone) => PhoneNumber = Phone.Create(phone);

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty.");

        Name = name.Trim();
    }
}
