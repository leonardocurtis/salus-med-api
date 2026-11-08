using SalusMedApi.Domain.Entities;

namespace SalusMedApi.UnitTests.Builders;

public static class DepartmentBuilder
{
    public static Department Active() => Build();

    public static Department Build(string name = "CARDIOLOGY", HealthUnit? healthUnit = null) =>
        Department.Create(name: name, healthUnit: healthUnit ?? HealthUnitBuilder.Active());
}
