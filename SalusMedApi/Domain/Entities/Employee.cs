using SalusMedApi.Domain.Enums;
using SalusMedApi.Domain.ValueObjects;
using SalusMedApi.Infrastructure.Repositories.Interfaces;

namespace SalusMedApi.Domain.Entities;

public class Employee : IAuditable
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string Phone { get; private set; }
    public string Cpf { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public EmployeeStatus Status { get; private set; }
    public Address Address { get; private set; }
    public EmployeeRole Role { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    public long UserId { get; private set; }
    public User User { get; private set; }

    public long DepartmentId { get; private set; }
    public Department Department { get; private set; }

    private Employee() { }

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
    ) =>
        new Employee
        {
            Name = name.Trim(),
            Phone = phone.Trim(),
            Cpf = cpf.Trim(),
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Status = EmployeeStatus.Active,
            Address = address,
            User = user,
            Role = role,
            Department = department,
        };

    public void UpdateContact(string phone) => Phone = phone.Trim();

    public void UpdateName(string name) => Name = name.Trim();

    public void UpdateAddress(Address address) => Address = address;

    public void ChangeGender(Gender gender) => Gender = gender;

    public void AssignDepartment(int departmentId) => DepartmentId = departmentId;

    public void MarkAsActive() => Status = EmployeeStatus.Active;

    public void MarkAsTerminated() => Status = EmployeeStatus.Terminated;

    public void PlaceOnLeave() => Status = EmployeeStatus.OnLeave;
}
