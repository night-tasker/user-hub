using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_info_image_id",
                table: "user_infos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "user_info_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    extension = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    user_info_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_info_images", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_infos_user_info_image_id",
                table: "user_infos",
                column: "user_info_image_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_infos_user_info_image_user_info_image_id1",
                table: "user_infos",
                column: "user_info_image_id",
                principalTable: "user_info_images",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_infos_user_info_image_user_info_image_id1",
                table: "user_infos");

            migrationBuilder.DropTable(
                name: "user_info_images");

            migrationBuilder.DropIndex(
                name: "ix_user_infos_user_info_image_id",
                table: "user_infos");

            migrationBuilder.DropColumn(
                name: "user_info_image_id",
                table: "user_infos");
        }
    }
}
