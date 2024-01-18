using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddManyImagesForOneUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_infos_user_image_user_info_image_id1",
                table: "user_infos");

            migrationBuilder.DropIndex(
                name: "ix_user_infos_user_info_image_id",
                table: "user_infos");

            migrationBuilder.DropColumn(
                name: "user_info_image_id",
                table: "user_infos");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "user_image",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_user_image_user_info_id",
                table: "user_image",
                column: "user_info_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_image_user_info_user_info_id",
                table: "user_image",
                column: "user_info_id",
                principalTable: "user_infos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_image_user_info_user_info_id",
                table: "user_image");

            migrationBuilder.DropIndex(
                name: "ix_user_image_user_info_id",
                table: "user_image");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "user_image");

            migrationBuilder.AddColumn<Guid>(
                name: "user_info_image_id",
                table: "user_infos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_infos_user_info_image_id",
                table: "user_infos",
                column: "user_info_image_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_infos_user_image_user_info_image_id1",
                table: "user_infos",
                column: "user_info_image_id",
                principalTable: "user_image",
                principalColumn: "id");
        }
    }
}
