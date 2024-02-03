using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserInfosTableToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_organization_users_user_info_user_info_id",
                table: "organization_users");

            migrationBuilder.DropForeignKey(
                name: "fk_user_images_user_info_user_info_id",
                table: "user_images");

            migrationBuilder.DropTable(
                name: "user_infos");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    first_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    middle_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    last_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    created_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_organization_users_user_user_id",
                table: "organization_users",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_images_user_user_info_id",
                table: "user_images",
                column: "user_info_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_organization_users_user_user_id",
                table: "organization_users");

            migrationBuilder.DropForeignKey(
                name: "fk_user_images_user_user_info_id",
                table: "user_images");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.CreateTable(
                name: "user_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    first_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    last_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    middle_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    updated_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_infos", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_organization_users_user_info_user_info_id",
                table: "organization_users",
                column: "user_id",
                principalTable: "user_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_images_user_info_user_info_id",
                table: "user_images",
                column: "user_info_id",
                principalTable: "user_infos",
                principalColumn: "id");
        }
    }
}
