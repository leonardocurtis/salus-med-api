using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ResetEmployeeNumberSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "employee_number",
                table: "employees",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(10)",
                oldFixedLength: true,
                oldMaxLength: 10
            );

            migrationBuilder.Sql("ALTER SEQUENCE employee_number_seq RESTART WITH 100001;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "employee_number",
                table: "employees",
                type: "character(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10
            );
        }
    }
}
