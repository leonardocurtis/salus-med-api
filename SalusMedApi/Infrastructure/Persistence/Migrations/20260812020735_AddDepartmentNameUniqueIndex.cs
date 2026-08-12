using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_departments_health_unit_id_name",
                table: "departments"
            );

            migrationBuilder.CreateIndex(
                name: "ix_departments_health_unit_id",
                table: "departments",
                column: "health_unit_id"
            );

            migrationBuilder.Sql(
                """
                CREATE UNIQUE INDEX "ix_departments_health_unit]_id_name_ci"
                ON departments ("health_unit_id", LOWER("name"));
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP INDEX IF EXISTS "ix_departments_health_unit_id_name_ci";
                """
            );

            migrationBuilder.DropIndex(name: "ix_departments_health_unit_id", table: "departments");

            migrationBuilder.CreateIndex(
                name: "ix_departments_health_unit_id_name",
                table: "departments",
                columns: new[] { "health_unit_id", "name" },
                unique: true
            );
        }
    }
}
