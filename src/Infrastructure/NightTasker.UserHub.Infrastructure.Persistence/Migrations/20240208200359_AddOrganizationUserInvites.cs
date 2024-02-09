using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NightTasker.UserHub.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationUserInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organization_user_invites",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    invited_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    inviter_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: true),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: true),
                    message = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    created_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date_time_offset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organization_user_invites", x => x.id);
                    table.ForeignKey(
                        name: "fk_organization_user_invites_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organization_user_invites_user_invited_user_id",
                        column: x => x.invited_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organization_user_invites_user_inviter_user_id",
                        column: x => x.inviter_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_organization_user_invites_invited_user_id",
                table: "organization_user_invites",
                column: "invited_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_organization_user_invites_inviter_user_id",
                table: "organization_user_invites",
                column: "inviter_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_organization_user_invites_organization_id",
                table: "organization_user_invites",
                column: "organization_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organization_user_invites");
        }
    }
}
