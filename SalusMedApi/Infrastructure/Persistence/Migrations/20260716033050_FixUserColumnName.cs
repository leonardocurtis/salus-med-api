using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalusMedApi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUserColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email_address",
                table: "users",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "ix_users_email_address",
                table: "users",
                newName: "ix_users_email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "email_address");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "ix_users_email_address");
        }
    }
}
