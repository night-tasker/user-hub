using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_organization_user_organization_organization_id",
                table: "organization_user");

            migrationBuilder.DropForeignKey(
                name: "fk_organization_user_user_info_user_info_id",
                table: "organization_user");

            migrationBuilder.DropForeignKey(
                name: "fk_user_image_user_info_user_info_id",
                table: "user_image");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_image",
                table: "user_image");

            migrationBuilder.DropPrimaryKey(
                name: "pk_organization_user",
                table: "organization_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_organization",
                table: "organization");

            migrationBuilder.RenameTable(
                name: "user_image",
                newName: "user_images");

            migrationBuilder.RenameTable(
                name: "organization_user",
                newName: "organization_users");

            migrationBuilder.RenameTable(
                name: "organization",
                newName: "organizations");

            migrationBuilder.RenameIndex(
                name: "ix_user_image_user_info_id",
                table: "user_images",
                newName: "ix_user_images_user_info_id");

            migrationBuilder.RenameIndex(
                name: "ix_organization_user_user_id",
                table: "organization_users",
                newName: "ix_organization_users_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_organization_user_role",
                table: "organization_users",
                newName: "ix_organization_users_role");

            migrationBuilder.AlterColumn<string>(
                name: "file_name",
                table: "user_images",
                type: "character varying(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "extension",
                table: "user_images",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "user_images",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_images",
                table: "user_images",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_organization_users",
                table: "organization_users",
                columns: new[] { "organization_id", "user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_organizations",
                table: "organizations",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_organization_users_organizations_organization_id",
                table: "organization_users",
                column: "organization_id",
                principalTable: "organizations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_organization_users_organizations_organization_id",
                table: "organization_users");

            migrationBuilder.DropForeignKey(
                name: "fk_organization_users_user_info_user_info_id",
                table: "organization_users");

            migrationBuilder.DropForeignKey(
                name: "fk_user_images_user_info_user_info_id",
                table: "user_images");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_images",
                table: "user_images");

            migrationBuilder.DropPrimaryKey(
                name: "pk_organizations",
                table: "organizations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_organization_users",
                table: "organization_users");

            migrationBuilder.RenameTable(
                name: "user_images",
                newName: "user_image");

            migrationBuilder.RenameTable(
                name: "organizations",
                newName: "organization");

            migrationBuilder.RenameTable(
                name: "organization_users",
                newName: "organization_user");

            migrationBuilder.RenameIndex(
                name: "ix_user_images_user_info_id",
                table: "user_image",
                newName: "ix_user_image_user_info_id");

            migrationBuilder.RenameIndex(
                name: "ix_organization_users_user_id",
                table: "organization_user",
                newName: "ix_organization_user_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_organization_users_role",
                table: "organization_user",
                newName: "ix_organization_user_role");

            migrationBuilder.AlterColumn<string>(
                name: "file_name",
                table: "user_image",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "extension",
                table: "user_image",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "content_type",
                table: "user_image",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_image",
                table: "user_image",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_organization",
                table: "organization",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_organization_user",
                table: "organization_user",
                columns: new[] { "organization_id", "user_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_organization_user_organization_organization_id",
                table: "organization_user",
                column: "organization_id",
                principalTable: "organization",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_organization_user_user_info_user_info_id",
                table: "organization_user",
                column: "user_id",
                principalTable: "user_infos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_image_user_info_user_info_id",
                table: "user_image",
                column: "user_info_id",
                principalTable: "user_infos",
                principalColumn: "id");
        }
    }
}
