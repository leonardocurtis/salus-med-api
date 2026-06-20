using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHealthUnitIdToDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "health_unit_id",
                table: "departments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "clinics",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    corporate_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    trade_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clinics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "health_units",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cnes = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    technical_manager_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    technical_manager_council_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    clinic_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    complement = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    neighborhood = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    postal_code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    state = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_health_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_health_units_clinics_clinic_id",
                        column: x => x.clinic_id,
                        principalTable: "clinics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_departments_health_unit_id",
                table: "departments",
                column: "health_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_clinics_cnpj",
                table: "clinics",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_health_units_clinic_id",
                table: "health_units",
                column: "clinic_id");

            migrationBuilder.CreateIndex(
                name: "ix_health_units_cnes",
                table: "health_units",
                column: "cnes",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_health_units_cnpj",
                table: "health_units",
                column: "cnpj",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_departments_health_units_health_unit_id",
                table: "departments",
                column: "health_unit_id",
                principalTable: "health_units",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_departments_health_units_health_unit_id",
                table: "departments");

            migrationBuilder.DropTable(
                name: "health_units");

            migrationBuilder.DropTable(
                name: "clinics");

            migrationBuilder.DropIndex(
                name: "ix_departments_health_unit_id",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "health_unit_id",
                table: "departments");
        }
    }
}
