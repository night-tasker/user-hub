using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInfoAndUserImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    extension = table.Column<string>(type: "text", nullable: true),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    user_info_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_image", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    first_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    middle_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    last_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    user_info_image_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_infos", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_infos_user_image_user_info_image_id1",
                        column: x => x.user_info_image_id,
                        principalTable: "user_image",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_infos_user_info_image_id",
                table: "user_infos",
                column: "user_info_image_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_infos");

            migrationBuilder.DropTable(
                name: "user_image");
        }
    }
}
