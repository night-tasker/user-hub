using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserInfoColumnToUserInUserImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_images_user_user_info_id",
                table: "user_images");

            migrationBuilder.RenameColumn(
                name: "user_info_id",
                table: "user_images",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_images_user_info_id",
                table: "user_images",
                newName: "ix_user_images_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_images_users_user_id",
                table: "user_images",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_images_users_user_id",
                table: "user_images");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "user_images",
                newName: "user_info_id");

            migrationBuilder.RenameIndex(
                name: "ix_user_images_user_id",
                table: "user_images",
                newName: "ix_user_images_user_info_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_images_user_user_info_id",
                table: "user_images",
                column: "user_info_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
