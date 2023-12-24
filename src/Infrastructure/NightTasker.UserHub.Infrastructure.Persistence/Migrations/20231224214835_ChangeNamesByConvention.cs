using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamesByConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_info",
                table: "user_info");

            migrationBuilder.RenameTable(
                name: "user_info",
                newName: "user_infos");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_infos",
                table: "user_infos",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_infos",
                table: "user_infos");

            migrationBuilder.RenameTable(
                name: "user_infos",
                newName: "user_info");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_info",
                table: "user_info",
                column: "id");
        }
    }
}
