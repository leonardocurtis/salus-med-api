using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefineEntitySchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "technical_manager_state",
                table: "health_units",
                newName: "technical_manager_crm_state"
            );

            migrationBuilder.AlterColumn<string>(
                name: "crm_number",
                table: "physicians",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6
            );

            migrationBuilder.CreateIndex(
                name: "ix_physicians_crm_number_crm_state",
                table: "physicians",
                columns: new[] { "crm_number", "crm_state" },
                unique: true
            );

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "patients",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20
            );

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "patients",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100
            );

            migrationBuilder.AlterColumn<string>(
                name: "technical_manager_crm",
                table: "health_units",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "health_units",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20
            );

            migrationBuilder.AlterColumn<string>(
                name: "technical_manager_crm_state",
                table: "health_units",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "technical_manager_crm_state",
                table: "health_units",
                newName: "technical_manager_state"
            );

            migrationBuilder.AlterColumn<string>(
                name: "crm_number",
                table: "physicians",
                type: "character varying(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9
            );

            migrationBuilder.DropIndex(
                name: "ix_physicians_crm_number_crm_state",
                table: "physicians"
            );

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "patients",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11
            );

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "patients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254
            );

            migrationBuilder.AlterColumn<string>(
                name: "technical_manager_crm",
                table: "health_units",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9
            );

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "health_units",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11
            );

            migrationBuilder.AlterColumn<string>(
                name: "technical_manager_state",
                table: "health_units",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2)",
                oldMaxLength: 2
            );
        }
    }
}
