using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.UnitTests.Builders;

public static class EmployeeBuilder
{
    public static Employee Active() => Build();

    public static Employee InStatus(EmployeeStatus status)
    {
        var employee = Build();

        switch (status)
        {
            case EmployeeStatus.Active:
                break;
            case EmployeeStatus.OnLeave:
                employee.PlaceOnLeave();
                break;
            case EmployeeStatus.Vacation:
                employee.StartVacation();
                break;
            case EmployeeStatus.Terminated:
                employee.Terminate("test");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(status), status, null);
        }

        return employee;
    }

    public static Employee Build(string name = "Maria Silva", Department? department = null) =>
        Employee.Create(
            employeeNumber: "1000001",
            name: name,
            phone: "51999999999",
            email: "maria@example.com",
            cpf: "52998224725",
            dateOfBirth: new DateOnly(1990, 5, 20),
            address: AddressBuilder.Default(),
            occupation: Occupation.Pharmacist,
            department: department ?? DepartmentBuilder.Active(),
            hiredAt: new DateOnly(2024, 1, 10)
        );
}
