using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounts.Repository.MySQL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grants",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    clientId = table.Column<string>(nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    removedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(nullable: false),
                    code = table.Column<string>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    desc = table.Column<string>(nullable: true),
                    action = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grants", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    clientId = table.Column<string>(nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    removedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(nullable: false),
                    title = table.Column<string>(nullable: false),
                    desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    clientId = table.Column<string>(nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "now()"),
                    removedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    username = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    locationId = table.Column<string>(nullable: true),
                    lastLogin = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "profile_grants",
                columns: table => new
                {
                    ProfileID = table.Column<int>(nullable: false),
                    GrantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profile_grants", x => new { x.ProfileID, x.GrantID });
                    table.ForeignKey(
                        name: "FK_profile_grants_grants_GrantID",
                        column: x => x.GrantID,
                        principalTable: "grants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_profile_grants_profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "profiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_grants",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    GrantID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_grants", x => new { x.UserID, x.GrantID });
                    table.ForeignKey(
                        name: "FK_user_grants_grants_GrantID",
                        column: x => x.GrantID,
                        principalTable: "grants",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_grants_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    ProfileID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => new { x.UserID, x.ProfileID });
                    table.ForeignKey(
                        name: "FK_user_profiles_profiles_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "profiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_profiles_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_grants_clientId",
                table: "grants",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_grants_code",
                table: "grants",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profile_grants_GrantID",
                table: "profile_grants",
                column: "GrantID");

            migrationBuilder.CreateIndex(
                name: "IX_profiles_clientId",
                table: "profiles",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_user_grants_GrantID",
                table: "user_grants",
                column: "GrantID");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_ProfileID",
                table: "user_profiles",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_users_clientId",
                table: "users",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_users_username_password",
                table: "users",
                columns: new[] { "username", "password" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profile_grants");

            migrationBuilder.DropTable(
                name: "user_grants");

            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropTable(
                name: "grants");

            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
