using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateEmployeeAndUpdatePhysicianRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_physicians_users_user_id",
                table: "physicians");

            migrationBuilder.DropIndex(
                name: "ix_physicians_cpf",
                table: "physicians");

            migrationBuilder.DropIndex(
                name: "ix_physicians_phone",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "city",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "complement",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "cpf",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "name",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "neighborhood",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "number",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "state",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "status",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "street",
                table: "physicians");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "physicians",
                newName: "employee_id");

            migrationBuilder.RenameIndex(
                name: "ix_physicians_user_id",
                table: "physicians",
                newName: "ix_physicians_employee_id");

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    complement = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    neighborhood = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    postal_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_employees_cpf",
                table: "employees",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_phone",
                table: "employees",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_user_id",
                table: "employees",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_physicians_employees_employee_id",
                table: "physicians",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_physicians_employees_employee_id",
                table: "physicians");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.RenameColumn(
                name: "employee_id",
                table: "physicians",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "ix_physicians_employee_id",
                table: "physicians",
                newName: "ix_physicians_user_id");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "physicians",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "complement",
                table: "physicians",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cpf",
                table: "physicians",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "date_of_birth",
                table: "physicians",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "physicians",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "physicians",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "neighborhood",
                table: "physicians",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "number",
                table: "physicians",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "physicians",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "physicians",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "physicians",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "physicians",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street",
                table: "physicians",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_physicians_cpf",
                table: "physicians",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_physicians_phone",
                table: "physicians",
                column: "phone",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_physicians_users_user_id",
                table: "physicians",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
