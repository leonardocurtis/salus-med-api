using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameAndEmployeeNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
            CREATE SEQUENCE IF NOT EXISTS employee_number_seq
                START WITH 1
                INCREMENT BY 1
                NO MINVALUE
                NO MAXVALUE
                NO CYCLE
                CACHE 1;
            "
            );

            migrationBuilder.DropIndex(name: "ix_users_email", table: "users");

            migrationBuilder.DropColumn(name: "email", table: "users");

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "users",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "physicians",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "patients",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint"
            );

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "patients",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "patients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "health_units",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "employees",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint"
            );

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "employees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<string>(
                name: "employee_number",
                table: "employees",
                type: "char(11)",
                fixedLength: true,
                maxLength: 11,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "employees",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "departments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.AddColumn<Guid>(
                name: "public_id",
                table: "clinics",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")
            );

            migrationBuilder.CreateIndex(
                name: "ix_users_public_id",
                table: "users",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                table: "users",
                column: "username",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_physicians_public_id",
                table: "physicians",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_patients_public_id",
                table: "patients",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_health_units_public_id",
                table: "health_units",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_employees_email",
                table: "employees",
                column: "email",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_employees_employee_number",
                table: "employees",
                column: "employee_number",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_employees_public_id",
                table: "employees",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_departments_public_id",
                table: "departments",
                column: "public_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_clinics_public_id",
                table: "clinics",
                column: "public_id",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "ix_users_public_id", table: "users");

            migrationBuilder.DropIndex(name: "ix_users_username", table: "users");

            migrationBuilder.DropIndex(name: "ix_physicians_public_id", table: "physicians");

            migrationBuilder.DropIndex(name: "ix_patients_public_id", table: "patients");

            migrationBuilder.DropIndex(name: "ix_health_units_public_id", table: "health_units");

            migrationBuilder.DropIndex(name: "ix_employees_email", table: "employees");

            migrationBuilder.DropIndex(name: "ix_employees_employee_number", table: "employees");

            migrationBuilder.DropIndex(name: "ix_employees_public_id", table: "employees");

            migrationBuilder.DropIndex(name: "ix_departments_public_id", table: "departments");

            migrationBuilder.DropIndex(name: "ix_clinics_public_id", table: "clinics");

            migrationBuilder.DropColumn(name: "public_id", table: "users");

            migrationBuilder.DropColumn(name: "username", table: "users");

            migrationBuilder.DropColumn(name: "public_id", table: "physicians");

            migrationBuilder.DropColumn(name: "email", table: "patients");

            migrationBuilder.DropColumn(name: "public_id", table: "patients");

            migrationBuilder.DropColumn(name: "public_id", table: "health_units");

            migrationBuilder.DropColumn(name: "email", table: "employees");

            migrationBuilder.DropColumn(name: "employee_number", table: "employees");

            migrationBuilder.DropColumn(name: "public_id", table: "employees");

            migrationBuilder.DropColumn(name: "public_id", table: "departments");

            migrationBuilder.DropColumn(name: "public_id", table: "clinics");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: ""
            );

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "patients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true
            );

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true
            );

            migrationBuilder.Sql("DROP SEQUENCE IF EXISTS employee_number_seq;");
        }
    }
}
