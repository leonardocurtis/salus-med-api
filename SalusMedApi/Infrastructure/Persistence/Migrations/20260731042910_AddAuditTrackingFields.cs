using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTrackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "users",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "users",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "users",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "physicians",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "physicians",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "physicians",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "physicians",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "patients",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "patients",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "patients",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "patients",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "health_units",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "health_units",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "health_units",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "health_units",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "employees",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "employees",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "employees",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "departments",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "departments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "departments",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "departments",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "clinics",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "deleted_at",
                table: "clinics",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deleted_by",
                table: "clinics",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "clinics",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "physicians");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "health_units");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "health_units");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "health_units");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "health_units");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "deleted_by",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "clinics");
        }
    }
}
