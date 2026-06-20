using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixDepartmentNameUniqueIndexToComposite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_departments_health_unit_id",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_departments_name",
                table: "departments");

            migrationBuilder.CreateIndex(
                name: "ix_departments_health_unit_id_name",
                table: "departments",
                columns: new[] { "health_unit_id", "name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_departments_health_unit_id_name",
                table: "departments");

            migrationBuilder.CreateIndex(
                name: "ix_departments_health_unit_id",
                table: "departments",
                column: "health_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_departments_name",
                table: "departments",
                column: "name",
                unique: true);
        }
    }
}
