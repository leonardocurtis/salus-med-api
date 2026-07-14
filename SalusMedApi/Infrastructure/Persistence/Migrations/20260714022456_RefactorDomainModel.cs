using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "fk_employees_users_user_id", table: "employees");

            migrationBuilder.DropForeignKey(name: "fk_patients_users_user_id", table: "patients");

            migrationBuilder.DropForeignKey(
                name: "fk_physicians_employees_employee_id",
                table: "physicians"
            );

            migrationBuilder.DropIndex(
                name: "ix_physicians_medical_registration",
                table: "physicians"
            );

            migrationBuilder.DropIndex(name: "ix_health_units_cnes", table: "health_units");

            migrationBuilder.DropColumn(
                name: "technical_manager_council_number",
                table: "health_units"
            );

            migrationBuilder.RenameColumn(name: "email", table: "users", newName: "email_address");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "ix_users_email_address"
            );

            migrationBuilder.RenameColumn(
                name: "medical_registration",
                table: "physicians",
                newName: "crm_number"
            );

            migrationBuilder.RenameColumn(
                name: "technical_manager_name",
                table: "health_units",
                newName: "technical_manager"
            );

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255
            );

            migrationBuilder.AddColumn<string>(
                name: "crm_state",
                table: "physicians",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AlterColumn<string>(
                name: "cnes",
                table: "health_units",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(7)",
                oldMaxLength: 7
            );

            migrationBuilder.AddColumn<string>(
                name: "technical_manager_crm",
                table: "health_units",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "technical_manager_state",
                table: "health_units",
                type: "text",
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "ix_health_units_cnes",
                table: "health_units",
                column: "cnes",
                unique: true,
                filter: "cnes IS NOT NULL"
            );

            migrationBuilder.AddForeignKey(
                name: "fk_employees_users_user_id",
                table: "employees",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "fk_patients_users_user_id",
                table: "patients",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "fk_physicians_employees_employee_id",
                table: "physicians",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.CreateIndex(
                name: "ix_physicians_crm_unique",
                table: "physicians",
                columns: new[] { "crm_number", "crm_state" },
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_physicians_crm_unique", table: "physicians");

            migrationBuilder.DropForeignKey(name: "fk_employees_users_user_id", table: "employees");

            migrationBuilder.DropForeignKey(name: "fk_patients_users_user_id", table: "patients");

            migrationBuilder.DropForeignKey(
                name: "fk_physicians_employees_employee_id",
                table: "physicians"
            );

            migrationBuilder.DropIndex(name: "ix_health_units_cnes", table: "health_units");

            migrationBuilder.DropColumn(name: "crm_state", table: "physicians");

            migrationBuilder.DropColumn(name: "technical_manager_crm", table: "health_units");

            migrationBuilder.DropColumn(name: "technical_manager_state", table: "health_units");

            migrationBuilder.RenameColumn(name: "email_address", table: "users", newName: "email");

            migrationBuilder.RenameIndex(
                name: "ix_users_email_address",
                table: "users",
                newName: "ix_users_email"
            );

            migrationBuilder.RenameColumn(
                name: "crm_number",
                table: "physicians",
                newName: "medical_registration"
            );

            migrationBuilder.RenameColumn(
                name: "technical_manager",
                table: "health_units",
                newName: "technical_manager_name"
            );

            migrationBuilder.AlterColumn<string>(
                name: "password_hash",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60
            );

            migrationBuilder.AlterColumn<string>(
                name: "cnes",
                table: "health_units",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(7)",
                oldMaxLength: 7,
                oldNullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "technical_manager_council_number",
                table: "health_units",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.CreateIndex(
                name: "ix_physicians_medical_registration",
                table: "physicians",
                column: "medical_registration",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_health_units_cnes",
                table: "health_units",
                column: "cnes",
                unique: true
            );

            migrationBuilder.AddForeignKey(
                name: "fk_employees_users_user_id",
                table: "employees",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_patients_users_user_id",
                table: "patients",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "fk_physicians_employees_employee_id",
                table: "physicians",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
