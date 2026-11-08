using SalusMedApi.Application.Exceptions;
using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;
using SalusMedApi.UnitTests.Builders;

namespace SalusMedApi.UnitTests.Domain;

public class EmployeeTests
{
    [Fact]
    public void Create_WithValidData_StartsActiveAndTrimsName()
    {
        var employee = EmployeeBuilder.Build(name: "  Maria Silva  ");

        employee.Status.ShouldBe(EmployeeStatus.Active);
        employee.Name.ShouldBe("Maria Silva");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankName_Throws(string? name)
    {
        var ex = Should.Throw<DomainException>(() => EmployeeBuilder.Build(name!));

        ex.Message.ShouldContain("name");
    }

    [Fact]
    public void Create_WithNullAddress_Throws()
    {
        var ex = Should.Throw<DomainException>(() =>
            Employee.Create(
                employeeNumber: "1000001",
                name: "Maria",
                phone: "51999999999",
                email: "maria@example.com",
                cpf: "52998224725",
                dateOfBirth: new DateOnly(1990, 5, 20),
                address: null!,
                occupation: default,
                department: DepartmentBuilder.Active(),
                hiredAt: new DateOnly(2024, 1, 10)
            )
        );

        ex.Message.ShouldContain("address");
    }

    [Fact]
    public void Create_WithNullDepartment_Throws()
    {
        var ex = Should.Throw<DomainException>(() =>
            Employee.Create(
                employeeNumber: "1000001",
                name: "Maria",
                phone: "51999999999",
                email: "maria@example.com",
                cpf: "52998224725",
                dateOfBirth: new DateOnly(1990, 5, 20),
                address: AddressBuilder.Default(),
                occupation: default,
                department: null!,
                hiredAt: new DateOnly(2024, 1, 10)
            )
        );

        ex.Message.ShouldContain("department");
    }

    [Theory]
    [InlineData(EmployeeStatus.Active, EmployeeStatus.OnLeave)]
    [InlineData(EmployeeStatus.Active, EmployeeStatus.Vacation)]
    [InlineData(EmployeeStatus.Active, EmployeeStatus.Terminated)]
    [InlineData(EmployeeStatus.OnLeave, EmployeeStatus.Active)]
    [InlineData(EmployeeStatus.OnLeave, EmployeeStatus.Vacation)]
    [InlineData(EmployeeStatus.OnLeave, EmployeeStatus.Terminated)]
    [InlineData(EmployeeStatus.Vacation, EmployeeStatus.Active)]
    [InlineData(EmployeeStatus.Vacation, EmployeeStatus.OnLeave)]
    [InlineData(EmployeeStatus.Vacation, EmployeeStatus.Terminated)]
    public void Transition_Allowed_UpdatesStatus(EmployeeStatus from, EmployeeStatus to)
    {
        var employee = EmployeeBuilder.InStatus(from);

        ApplyTransition(employee, to);

        employee.Status.ShouldBe(to);
    }

    [Theory]
    [InlineData(EmployeeStatus.Active)]
    [InlineData(EmployeeStatus.OnLeave)]
    [InlineData(EmployeeStatus.Vacation)]
    public void Transition_FromTerminated_Throws(EmployeeStatus target)
    {
        var employee = EmployeeBuilder.InStatus(EmployeeStatus.Terminated);

        var ex = Should.Throw<DomainException>(() => ApplyTransition(employee, target));

        ex.Message.ShouldContain("Invalid status transition");
        employee.Status.ShouldBe(EmployeeStatus.Terminated);
    }

    [Theory]
    [InlineData(EmployeeStatus.Active)]
    [InlineData(EmployeeStatus.OnLeave)]
    [InlineData(EmployeeStatus.Vacation)]
    [InlineData(EmployeeStatus.Terminated)]
    public void Transition_ToSameStatus_Throws(EmployeeStatus status)
    {
        var employee = EmployeeBuilder.InStatus(status);

        var action = () => ApplyTransition(employee, status);

        Should.Throw<DomainException>(action);
    }

    private static void ApplyTransition(Employee employee, EmployeeStatus target)
    {
        switch (target)
        {
            case EmployeeStatus.Active:
                employee.ReturnToActive();
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
                throw new ArgumentOutOfRangeException(nameof(target), target, null);
        }
    }

    [Fact]
    public void Terminate_WhenActive_SetsStatusAndAuditFields()
    {
        var employee = EmployeeBuilder.Active();
        var before = DateTimeOffset.UtcNow;

        employee.Terminate("1000017");

        employee.Status.ShouldBe(EmployeeStatus.Terminated);
        employee.DeletedBy.ShouldBe("1000017");
        employee.DeletedAt.ShouldNotBeNull();
        employee.DeletedAt!.Value.ShouldBeGreaterThanOrEqualTo(before);

        employee.DeletedAt!.Value.ShouldBeLessThanOrEqualTo(DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Update_WithOnlyName_KeepsOtherFields()
    {
        var employee = EmployeeBuilder.Active();

        employee.Update(name: "  Ana Costa ", phone: null, email: null);

        employee.Name.ShouldBe("Ana Costa");
        employee.PhoneNumber.ShouldBe(employee.PhoneNumber);
        employee.EmailAddress.ShouldBe(employee.EmailAddress);
    }

    [Fact]
    public void Update_WhenAnyFieldIsInvalid_ChangesNothing()
    {
        var employee = EmployeeBuilder.Active();

        Should.Throw<DomainException>(() =>
            employee.Update(
                name: "New name",
                phone: "invalid phone number",
                email: "novo@example.com"
            )
        );

        employee.Name.ShouldBe(employee.Name);
        employee.PhoneNumber.ShouldBe(employee.PhoneNumber);
        employee.EmailAddress.ShouldBe(employee.EmailAddress);
    }

    [Fact]
    public void Update_WhenTerminated_ThrowsConflict()
    {
        var employee = EmployeeBuilder.InStatus(EmployeeStatus.Terminated);

        Should.Throw<ConflictException>(() => employee.Update("Other", null, null));
    }

    [Fact]
    public void AssignDepartment_WhenTerminated_KeepsOriginalDepartment()
    {
        var employee = EmployeeBuilder.InStatus(EmployeeStatus.Terminated);
        var originalDepartment = employee.Department;

        Should.Throw<ConflictException>(() =>
            employee.AssignDepartment(DepartmentBuilder.Active())
        );

        employee.Department.ShouldBe(originalDepartment);
    }

    [Fact]
    public void UpdateAddress_WhenTerminated_ThrowsConflict()
    {
        var employee = EmployeeBuilder.InStatus(EmployeeStatus.Terminated);

        Should.Throw<ConflictException>(() => employee.UpdateAddress(AddressBuilder.Default()));
    }

    [Fact]
    public void AssignDepartment_WhenTerminated_ThrowsConflict()
    {
        var employee = EmployeeBuilder.InStatus(EmployeeStatus.Terminated);

        Should.Throw<ConflictException>(() =>
            employee.AssignDepartment(DepartmentBuilder.Active())
        );
    }

    [Fact]
    public void UpdateAddress_WithNull_Throws()
    {
        var employee = EmployeeBuilder.Active();

        Should.Throw<DomainException>(() => employee.UpdateAddress(null!));
    }
}
